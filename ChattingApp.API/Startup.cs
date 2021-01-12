using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using ChattingApp.Foundation;
using ChattingApp.Foundation.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace ChattingApp.API
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; private set; }

        public static ILifetimeScope AutofacContainer { get; private set; }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var connAndMig = GetConnectionStringAndMigrationAssembly();

            builder.RegisterModule(new ApiModule(connAndMig.connectionString, connAndMig.migrationAssembly));
            builder.RegisterModule(new FoundationModule(connAndMig.connectionString, connAndMig.migrationAssembly));

        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connAndMig = GetConnectionStringAndMigrationAssembly();

            services.AddDbContext<ChattingContext>(options =>
                options.UseSqlServer(connAndMig.connectionString, b => b.MigrationsAssembly(connAndMig.migrationAssembly)));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                    .GetBytes(Configuration.GetSection("jwt:token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddControllers()
               .AddNewtonsoftJson(options => {
                   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
               });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddCors();
            services.AddOptions();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private (string connectionString, string migrationAssembly) GetConnectionStringAndMigrationAssembly()
        {
            var connectionStringName = GetConnectionStringName();
            var connectionString = Configuration.GetConnectionString(connectionStringName);
            var migrationAssemblyName = typeof(Startup).Assembly.FullName;

            return (connectionString, migrationAssemblyName);
        }

        private string GetConnectionStringName()
        {
            var connectionStringName = "DefaultConnection";
            return connectionStringName;
        }
    }
}
