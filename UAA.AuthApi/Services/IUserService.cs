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
    UserAccount Authenticate(string username, string password);
    IEnumerable<UserAccount> GetAll();
    UserAccount GetById(int id);
    UserAccount Create(UserAccount user, string password);
    void Update(UserAccount user, string password = null);
    void Delete(int id);
  }
}
