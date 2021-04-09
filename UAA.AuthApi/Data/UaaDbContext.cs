using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UAA.Entity;

namespace UAA.AuthApi.Data
{
  public class UaaDbContext : DbContext
  {
    //public UaaDbContext(DbContextOptions<UaaDbContext> opt) : base(opt)
    //{

    //}

    public UaaDbContext(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    protected readonly IConfiguration Configuration;

    public DbSet<UserAccount> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      //base.OnModelCreating(modelBuilder);
      builder.Entity<UserAccount>(e =>
      {
        e.ToTable("t_auth_users");
        e.HasKey(x => x.Id);
        e.Property(x => x.Created)
          .HasColumnType("timestamp")
          .HasColumnName("created")
          .HasDefaultValueSql("CURRENT_TIMESTAMP");
      });
    }

  }
}
