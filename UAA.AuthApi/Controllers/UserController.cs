using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UAA.AuthApi.Services;
using UAA.Entity;
using UAA.Model;
using UAA.Model.User;

namespace UAA.AuthApi.Controllers
{
  /// <summary>
  /// user account crud controller
  /// </summary>
  public class UserController : BaseController
  {
    private readonly IUserService _userSrv;
    private readonly IMapper _mapper;

    public UserController(
      IUserService userSrv,
      IMapper mapper)
    {
      _userSrv = userSrv;
      _mapper = mapper;
    }

    [HttpGet("{name:regex(^[[A-Za-z0-9-_.]]+$)}")]
    public async Task<IActionResult> GetUserByName([FromRoute] string name)
    {
      try
      {
        var user = await _userSrv.GetByName(name, false);

        var um = _mapper.Map<UserModel>(user);
        if (user == null) return Ok(
          new AppResponse<UserModel>
          {
            Code = 404,
            Message = "user not found"
          });

        return Ok(
          new AppResponse<UserModel>
          {
            Code = 200,
            Message = "",
            Payload = um
          });
      }
      catch (Exception e)
      {
        return Ok(
          new AppResponse<UserModel>
          {
            Code = 500,
            Message = e.Message
          });
      }
    }

    [HttpGet]
    public async Task<IActionResult> GetUserList(int page = 1, int pageSite = 10)
    {
      var list = await _userSrv.GetAll(page, pageSite);
      if (list.Count == 0) return Ok(new AppResponse<object> { Code = 404, Message = "not found" });
      return Ok(new AppResponse<List<UserModel>> { Code = 200, Payload = _mapper.Map<List< UserModel >>( list) });
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
      var errMsg = CheckRegisterModel(model);
      if (!string.IsNullOrEmpty(errMsg))
      {
        return Ok(new AppResponse<UserModel>
        {
          Code = 400,
          Message = errMsg,
        });
      }

      var user = await _userSrv.Create(model);
      return Ok(new AppResponse<object> { Code = 201, Message = "account created" });
    }



    [HttpPut("{name:alpha}")]
    public async Task<IActionResult> Update([FromRoute] string name, [FromBody] UpdateModel model)
    {
      try
      {
        model.UserName = name;
        var count = await _userSrv.Update(model);
        if (count > 0) 
          return Ok(new AppResponse<object> { Code = 200, Message = "user changed" });
        else
          return Ok(new AppResponse<object> { Code = 200, Message = "changed effect none data" });
      }
      catch (Exception e)
      {
        return Ok(new AppResponse<object> { Code = 400, Message = e.Message });
      }
    }

    [HttpDelete("{name:alpha}")]
    public async Task<IActionResult> Delete([FromRoute] string name)
    {
      try
      {
        var count = await _userSrv.Delete(name);
        if (count > 0)
          return Ok(new AppResponse<object> { Code = 200, Message = "user deleted" });
        else
          return Ok(new AppResponse<object> { Code = 200, Message = "changed effect none data" });
      }
      catch (Exception e)
      {
        return Ok(new AppResponse<object> { Code = 400, Message = e.Message });
      }
    }

    private static string CheckRegisterModel(RegisterModel model)
    {
      if (string.IsNullOrEmpty(model.UserName)) return "UserName is empry";
      if (string.IsNullOrEmpty(model.Email)) return "Email is empry";
      if (string.IsNullOrEmpty(model.DisplayName)) return "DisplayName is empry";
      if (string.IsNullOrEmpty(model.Password)) return "Password is empry";
      if (!model.Password.Equals(model.ConfirmPassword)) return "Password not same";
      return string.Empty;
    }

  }
}
