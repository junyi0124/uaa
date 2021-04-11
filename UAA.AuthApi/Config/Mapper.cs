using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using UAA.Entity;
using UAA.Model.User;

namespace UAA.AuthApi.Config
{
  public class Mapper : Profile
  {
    public Mapper()
    {
      CreateMap<UserAccount, UserModel>().ReverseMap();
      CreateMap<UserAccount, UpdateModel>().ReverseMap();
      CreateMap<UserAccount, RegisterModel>().ReverseMap();

    }
  }
}
