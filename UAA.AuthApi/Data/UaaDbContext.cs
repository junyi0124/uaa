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
        e.Property(x => x.Id)
          .HasColumnName("id");

        e.Property(x => x.UserName)
          .HasColumnName("username");
        e.HasIndex(x => x.UserName).IsUnique(true);

        e.Property(x => x.DisplayName)
          .HasColumnName("display");

        e.Property(x => x.Email)
          .HasColumnName("email");

        e.Property(x => x.PasswordHash)
          .HasColumnName("pwd_hash");

        e.Property(x => x.PasswordSalt)
          .HasColumnName("pwd_salt");

        e.Property(x => x.Status)
          .HasColumnName("status")
          .HasDefaultValue(0);

        e.Property(x => x.Created)
          .HasColumnType("timestamp")
          .HasColumnName("created")
          .ValueGeneratedOnAdd()
          .HasDefaultValueSql("CURRENT_TIMESTAMP()");

        e.Property(x => x.LastUpdate)
          .HasColumnType("timestamp")
          .HasColumnName("lastupdate")
          .ValueGeneratedOnAddOrUpdate()
          .HasDefaultValueSql("CURRENT_TIMESTAMP() ON UPDATE CURRENT_TIMESTAMP()");


      });
    }

  }
}
