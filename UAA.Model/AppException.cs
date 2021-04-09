using System;
using System.Globalization;

namespace UAA.Model
{
  public class AppException : Exception
  {
    public int Level { get; set; }

    public AppException() : base() { }

    public AppException(string message, int level = 2) : base(message) 
    {
      Level = level;
    }

    public AppException(string message, int level = 2, params object[] args)
        : base(String.Format(CultureInfo.CurrentCulture, message, args))
    {
      Level = level;
    }
  }

  public class AppSettings
  {
    public string Secret { get; set; }
  }
}
