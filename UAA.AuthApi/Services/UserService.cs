﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UAA.AuthApi.Data;
using UAA.Entity;
using UAA.Model;
using UAA.Model.User;

namespace UAA.AuthApi.Services
{
  public class UserService : IUserService
  {
    private readonly UaaDbContext _context;
    private readonly IMapper _mapper;

    public UserService(UaaDbContext context,
      IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<UserModel> Authenticate(string username, string password)
    {
      if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        return null;

      var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);

      // check if username exists
      if (user == null)
        return null;

      // check if password is correct
      if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        return null;

      // authentication successful
      return _mapper.Map<UserModel>(user);
    }

    public Task<List<UserAccount>> GetAll(int page, int pageSize)
    {
      return _context.Users.AsNoTracking().Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }



    public Task<UserAccount> GetById(long id, bool trakcing = false)
    {
      var where = trakcing ? _context.Users.AsTracking() : _context.Users.AsNoTracking();
      return where.FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <summary>
    ///按业务逻辑查找用户，应用场景中尽量使用这个
    /// </summary>
    /// <param name="name"></param>
    /// <param name="trakcing"></param>
    /// <returns></returns>
    public Task<UserAccount> GetByName(string name, bool trakcing = false)
    {
      var where = trakcing ? _context.Users.AsTracking() : _context.Users.AsNoTracking();
      //.ProjectTo<UserModel>(_mapper.ConfigurationProvider)
      return where.FirstOrDefaultAsync(c => c.UserName == name);
    }

    public async Task<UserAccount> Create(RegisterModel user)
    {
      //todo: use more less effort sql to fit requirment.
      var nullIfNameExist = await GetByName(user.UserName, false);

      // username not exist
      if(nullIfNameExist==null)
      {
        UserAccount newAccount = _mapper.Map<UserAccount>(user);
        _context.Users.Add(newAccount);
        await _context.SaveChangesAsync();
        return newAccount;
      }
      else
      {
        throw new AppException("user name in use");
      }
    }

    public async Task<int> Update(UpdateModel user)
    {
      var userInDb = await GetByName(user.UserName, true);
      if (userInDb == null) return 0;


      userInDb.DisplayName = user.DisplayName;
      userInDb.Email = user.Email;

      return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdatePassword(ChangePasswordModel user)
    {
      var userInDb = await _context.Users.FirstOrDefaultAsync(x => x.UserName == user.UserName);

      // check if username exists
      if (userInDb == null)
        return 0;

      // check if password is correct
      if (!VerifyPasswordHash(user.Password, userInDb.PasswordHash, userInDb.PasswordSalt))
        return 0;

      userInDb.PasswordHash = RenewPasswordHash(user.Password, userInDb.PasswordSalt);

      return await _context.SaveChangesAsync();
    }


    public async Task<int> Delete(string username)
    {
      var user = await GetByName(username, true);
      if (user == null) return 0;
      user.Status = 2;
      return await _context.SaveChangesAsync();
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      if (string.IsNullOrEmpty(password)) throw new AppException("password empty");
      //if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

      using (var hmac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }
    }

    private static byte[] RenewPasswordHash(string password, byte[] key)
    {
      if (string.IsNullOrEmpty(password)) throw new AppException("password empty");

      using (var hmac = new System.Security.Cryptography.HMACSHA512(key))
      {
        return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }
    }

    private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
      if (string.IsNullOrEmpty(password)) throw new AppException("password empty");
      //if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
      if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
      if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

      using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
      {
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        for (int i = 0; i < computedHash.Length; i++)
        {
          if (computedHash[i] != storedHash[i]) return false;
        }
      }

      return true;
    }


  }
}
