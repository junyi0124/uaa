using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UAA.Model.User;

namespace UAA.Model
{
  [ApiController]
  [Route("[controller]")]
  public class BaseController : ControllerBase
  {
    // returns the current authenticated account (null if not logged in)
    public UserModel Account => (UserModel)HttpContext.Items["Account"];
  }
}
