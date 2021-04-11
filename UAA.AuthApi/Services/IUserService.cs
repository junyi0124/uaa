using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAA.Entity;
using UAA.Model.User;

namespace UAA.AuthApi.Services
{
  public interface IUserService
  {
    // query
    Task<List<UserAccount>> GetAll(int page, int pageSize);
    Task<UserAccount> GetById(long id, bool trakcing = false);
    Task<UserAccount> GetByName(string name, bool trakcing = false);

    // commands
    Task<UserAccount> Create(RegisterModel user);
    Task<int> UpdatePassword(ChangePasswordModel user);
    Task<int> Update(UpdateModel user);
    Task<int> Delete(string username);

    // auth
    Task<UserModel> Authenticate(string username, string password);
  }
}
