using System;
using System.Globalization;

namespace UAA.Model
{
  public class AppException : Exception
  {
    /// <summary>
    ///   Verbose = 1,
    ///   Information = 2,
    ///   Warning = 3,
    ///   Error = 4,
    ///   Critical = 5
    /// </summary>
    public int Level { get; set; }

    public AppException() : base() { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message">error message</param>
    /// <param name="level">
    ///   Verbose = 1,
    ///   Information = 2,
    ///   Warning = 3,
    ///   Error = 4,
    ///   Critical = 5
    /// </param>
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
