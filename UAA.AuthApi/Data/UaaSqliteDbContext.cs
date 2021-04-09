using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UAA.AuthApi.Data
{
  public class UaaSqliteDbContext : UaaDbContext
  {
    public UaaSqliteDbContext(IConfiguration configuration) : base(configuration)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      // connect to sql server database
      options.UseMySql(Configuration.GetConnectionString("uaa_sqlite_conn"));
    }
  }
}
