using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UAA.AuthApi.Data;
using UAA.AuthApi.Helper;
using UAA.Entity;
using UAA.Model;

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
      if (nullIfNameExist == null)
      {
        UserAccount newAccount = _mapper.Map<UserAccount>(user);
        // create password hash and salt
        var hashTuple = HmacHelper.CreatePasswordHash(user.Password);
        if (hashTuple.e != null)
        {
          throw hashTuple.e;
        }
        newAccount.PasswordHash = hashTuple.hash;
        newAccount.PasswordSalt = hashTuple.salt;

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
      if (!HmacHelper.Verify(user.Password, userInDb.PasswordHash, userInDb.PasswordSalt))
        return 0;

      userInDb.PasswordHash = HmacHelper.RenewHash(user.Password, userInDb.PasswordSalt);

      return await _context.SaveChangesAsync();
    }


    public async Task<int> Delete(string username)
    {
      var user = await GetByName(username, true);
      if (user == null) return 0;
      user.Status = 2;
      return await _context.SaveChangesAsync();
    }




  }
}
