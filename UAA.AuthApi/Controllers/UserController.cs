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
  public class UserController : BaseController
  {
    private readonly IUserService _userSrv;
    private readonly IMapper _mapper;

    public UserController(IUserService userSrv,
      IMapper mapper)
    {
      _userSrv = userSrv;
      _mapper = mapper;
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetUserById([FromRoute]long id)
    {
      //var uid = Guid.Parse(id);
      var user = await _userSrv.GetById(id, false);

      var um = _mapper.Map<UserModel>(user);
      if (user == null) return Ok(
        new AppResponse<UserModel> { 
          Code = 404, 
          Message = "user not found" 
        });

      return Ok(
        new AppResponse<UserModel> {
          Code = 200,
          Message = "",
          Payload = um
        });
    }

    //[HttpGet("{name:alpha}")]
    //public async Task<IActionResult> GetUserByName([FromRoute] string name)
    //{
    //  var user = await _userSrv.GetByName(name, false);

    //  var um = _mapper.Map<UserModel>(user);
    //  if (user == null) return Ok(
    //    new AppResponse<UserModel>
    //    {
    //      Code = 404,
    //      Message = "user not found"
    //    });

    //  return Ok(
    //    new AppResponse<UserModel>
    //    {
    //      Code = 200,
    //      Message = "",
    //      Payload = um
    //    });
    //}
  }
}
