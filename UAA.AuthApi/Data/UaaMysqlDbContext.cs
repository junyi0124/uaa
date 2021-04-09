using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;

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
        x =>
        {
          x.CharSet(CharSet.Utf8Mb4);
          x.CharSetBehavior(CharSetBehavior.NeverAppend);
          x.ServerVersion(new Version(5, 7, 17), ServerType.MySql);
        })
        .EnableServiceProviderCaching();
    }
  }
}
