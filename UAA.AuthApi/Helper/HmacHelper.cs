using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAA.Model;

namespace UAA.AuthApi.Helper
{
  public static class HmacHelper
  {
    private static (byte[] hash, byte[] salt, AppException e) CreatePasswordHash(string password)
    {
      (byte[] hash, byte[] salt, AppException e) val = (null, null, new AppException("password empty"));
      if (string.IsNullOrEmpty(password)) return val;
      //if (string.IsNullOrWhiteSpace(password))
      //  throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

      try
      {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
          var cleanBytes = System.Text.Encoding.UTF8.GetBytes(password);
          val = (hmac.ComputeHash(cleanBytes), hmac.Key, null);
          return val;
        }
      }
      catch(Exception e)
      {
        val = (null, null, new AppException(e.Message, 3));
        return val;
      }
    }

    private static byte[] RenewHash(string password, byte[] key)
    {
      if (string.IsNullOrEmpty(password)) throw new AppException("password empty");

      using (var hmac = new System.Security.Cryptography.HMACSHA512(key))
      {
        return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }
    }

    private static bool Verify(string password, byte[] Hash, byte[] Salt)
    {
      if (string.IsNullOrEmpty(password)) throw new AppException("password empty");
      //if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
      if (Hash.Length != 64) throw new ArgumentException("Invalid length, 64 bytes expected.", "Hash");
      if (Salt.Length != 128) throw new ArgumentException("Invalid length, 128 bytes expected.", "Salt");

      using (var hmac = new System.Security.Cryptography.HMACSHA512(Salt))
      {
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        for (int i = 0; i < computedHash.Length; i++)
        {
          if (computedHash[i] != Hash[i]) return false;
        }
      }

      return true;
    }
  }
}
