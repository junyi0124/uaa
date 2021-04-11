using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UAA.AuthApi.Data;
using UAA.AuthApi.Services;
using UAA.Model;

namespace UAA.AuthApi
{
  public class Startup
  {
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
      _configuration = configuration;
      _env = env;
    }

    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      //if (_env.IsProduction())
      //  services.AddDbContext<UaaDbContext, UaaMysqlDbContext>();
      //else
      //  services.AddDbContext<UaaDbContext, UaaSqliteDbContext>();
      services.AddDbContext<UaaDbContext, UaaMysqlDbContext>();

      services.AddCors();
      services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "UAA.AuthApi", Version = "v1" });
      });

      // configure strongly typed settings object
      services.Configure<AppSettings>(_configuration.GetSection("AppSettings"));

      services.AddScoped<IUserService, UserService>();
      //services.AddAutoMapper(this.GetType().Assembly);
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UAA.AuthApi v1"));
      }

      app.UseRouting();

      // global cors policy
      app.UseCors(x => x
          .SetIsOriginAllowed(origin => true)
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials());

      app.UseAuthorization();

      // custom jwt auth middleware
      app.UseMiddleware<JwtMiddleware>();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
