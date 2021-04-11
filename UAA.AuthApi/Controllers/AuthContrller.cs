using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UAA.Model;

namespace UAA.AuthApi.Controllers
{
  public class AuthContrller : BaseController
  {

    public AuthContrller(AuthContrller)
    {

    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Login()
    {
      var response = _accountService.Authenticate(model, ipAddress());
      setTokenCookie(response.RefreshToken);
      return Ok(response);
    }


  }
}
