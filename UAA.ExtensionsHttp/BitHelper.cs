using System;

namespace UAA.ExtensionsHttp
{
  public static class BitHelper
  {
    /// <summary>
    /// check status value if the BIT is 1 (logic true) or 0 (logic false)
    /// </summary>
    /// <param name="number">status value</param>
    /// <param name="bit">the Nth bit that we care about</param>
    /// <param name="checkTrueOrFalse"></param>
    /// <returns></returns>
    public static bool CheckStatus(this int number, int bit = 1)
    {
      if (bit > 32 || bit < 1) throw new ArgumentOutOfRangeException(nameof(bit),"from 1 to 32");

      return ((number >> (bit - 1)) & 1) == 1;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="number"></param>
    /// <param name="bit"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public static int SetStatus(this int number, int bit, bool input)
    {
      if (bit > 32 || bit < 1) throw new ArgumentOutOfRangeException(nameof(bit), "from 1 to 32");

      if (input)
      {
        // set Nth bit to 1
        return number | (1 << bit);
      }
      else
      {
        // set Nth bit to 0
        return number & (~(1 << bit));
      }
    }
  }
}
