using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAA.Entity;

namespace UAA.AuthApi.Services
{
  public interface IUserService
  {
    //Task<IEnumerable<UserAccount>> GetAll();
    Task<UserAccount> GetById(long id, bool trakcing = false);
    Task<UserAccount> GetByName(string name, bool trakcing = false);
    Task<UserAccount> Authenticate(string username, string password);
    Task<UserAccount> Create(UserAccount user, string password);
    Task<int> Update(UserAccount user);
    Task<int> UpdatePassword(string username, string password, string newPass);
    Task<int> Delete(long id);
  }
}
