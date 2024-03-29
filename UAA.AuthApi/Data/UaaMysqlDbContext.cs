﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace UAA.AuthApi.Data
{
  public class UaaMysqlDbContext : UaaDbContext
  {
    public UaaMysqlDbContext(IConfiguration configuration) : base(configuration)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      // connect to sql server database
      options.UseMySql(
        Configuration.GetConnectionString("uaa_mysql_conn"),
        ServerVersion.FromString("8.0.23"),
        x =>
        {
          x.CharSet(CharSet.Utf8Mb4);
          x.CharSetBehavior(CharSetBehavior.NeverAppend);
        })
        .LogTo(Console.WriteLine)
        .EnableSensitiveDataLogging()
        .EnableServiceProviderCaching();
    }
  }
}
