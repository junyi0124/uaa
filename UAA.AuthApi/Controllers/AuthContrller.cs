using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using UAA.AuthApi.Services;
using UAA.Entity;
using UAA.Model;
using UAA.Model.User;
using System.IdentityModel.Tokens.Jwt;

namespace UAA.AuthApi.Controllers
{
  public class AuthContrller : BaseController
  {
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;
    public AuthContrller(IAccountService accountService,
      IMapper mapper)
    {
      _accountService = accountService;
      _mapper = mapper;
  }

    //[HttpPost("authenticate")]
    //public async Task<IActionResult> Login()
    //{
    //  var response = _accountService.Authenticate(model, ipAddress());
    //  setTokenCookie(response.RefreshToken);
    //  return Ok(response);
    //}

    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthenticateRequest model)
    {
      var response = _accountService.Authenticate(model, ipAddress());
      setTokenCookie(response.RefreshToken);
      return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult> RefreshToken()
    {
      var refreshToken = Request.Cookies["refreshToken"];
      var response = _accountService.RefreshToken(refreshToken, ipAddress());
      setTokenCookie(response.RefreshToken);
      return Ok(response);
    }

    private void setTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }

    private string ipAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"];
        else
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }


  }
}
