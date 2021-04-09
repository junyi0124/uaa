using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAA.Entity;

namespace UAA.AuthApi.Services
{
  public class UserService : IUserService
  {
    public UserAccount Authenticate(string username, string password)
    {
      throw new NotImplementedException();
    }

    public UserAccount Create(UserAccount user, string password)
    {
      throw new NotImplementedException();
    }

    public void Delete(int id)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<UserAccount> GetAll()
    {
      throw new NotImplementedException();
    }

    public UserAccount GetById(int id)
    {
      throw new NotImplementedException();
    }

    public void Update(UserAccount user, string password = null)
    {
      throw new NotImplementedException();
    }
  }
}
