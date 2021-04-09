using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAA.Model
{
  public class AppResponse<T>
  {
    public int Code { get; set; }
    public string Message { get; set; }
    public T Payload { get; set; }
  }
}
