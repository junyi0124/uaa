using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UAA.AuthApi.Data;
using UAA.Entity;
using UAA.Model;

namespace UAA.AuthApi.Services
{
  public class UserService : IUserService
  {
    private readonly UaaDbContext _context;

    public UserService(UaaDbContext context)
    {
      _context = context;
    }

    public async Task<UserAccount> Authenticate(string username, string password)
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
      return user;
    }


    public Task<UserAccount> GetById(long id, bool trakcing = false)
    {
      var where = trakcing ? _context.Users.AsTracking() : _context.Users.AsNoTracking();
      return where.FirstOrDefaultAsync(c => c.Id == id);
    }

    public Task<UserAccount> GetByName(string name, bool trakcing = false)
    {
      var where = trakcing ? _context.Users.AsTracking() : _context.Users.AsNoTracking();
      return where.FirstOrDefaultAsync(c => c.UserName == name);
    }

    public async Task<UserAccount> Create(UserAccount user, string password)
    {
      var u = await GetByName(user.UserName, true);
      if(u==null)
      {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
      }
      else
      {
        throw new AppException("user name in use");
      }
    }

    public async Task<int> Update(UserAccount user)
    {
      var u = await GetById(user.Id, true);
      if (u == null) return 0;


      u.DisplayName = user.DisplayName;
      u.Email = user.Email;

      return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdatePassword(string username, string password, string newPass)
    {
      var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);

      // check if username exists
      if (user == null)
        return 0;

      // check if password is correct
      if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        return 0;

      user.PasswordHash = RenewPasswordHash(password, user.PasswordSalt);

      return await _context.SaveChangesAsync();
    }


    public async Task<int> Delete(long id)
    {
      var user = await GetById(id, true);
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
