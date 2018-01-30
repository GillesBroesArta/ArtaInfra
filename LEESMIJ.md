# ArtaInfra
Language: C#
Framework: .Net CORE

ArtaInfra is a helpfolder to setup WebApi's with the Mediator design pattern made in C#.
This folder initializes following features:
- Mediator
- Validators and Decorators
- HttpResult Wrapper
- Logging
- Exception Logging

# Installing
Create a WebApi project through command line or VisualStudio

* Clone or download the reposiory 
* Add it as submodule to your reposiory (preferred) 
```
git submodule add git@github.com:Artilium/ArtaInfra.git
```

## Build


Install following NuGet packages to the project containing this folder
* AutoMapper
* AutoMapper.Extensions.Microsoft.DependencyInjection
* Microsoft.EntityFrameworkCore
* Microsoft.EntityFrameworkCore.Relational
* Microsoft.EntityFrameworkCore.SqlServer
* Scrutor
* Serilog
* Serilog.Extensions.Logging
* Serilog.Settings.Configuration
* Serilog.Sinks.RollingFile
* Swashbuckle.AspNetCore
* Swashbuckle.AspNetCore.Swagger
* Swashbuckle.AspNetCore.SwaggerGen

The Infrastructure contains a decorator for a DbConext, this is not implemented in this repo.

## Config
Startup
```C#
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using Arta.BulkChangeMSISDN.Api.Entities.DataAccess;
using Arta.BulkChangeMSISDN.Api.Features;
using Arta.Infrastructure;
using Arta.Infrastructure.Logging;
using Arta.Infrastructure.Middleware;
using Arta.Infrastructure.Validation;
using AutoMapper;
using BugsBunny;

namespace Arta.GucciGang.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBugsBunny();
            services.AddMvcCore()
            .AddApiExplorer()
            .AddCors()
            .AddAuthorization()
            .AddJsonFormatters();

            services.AddAutoMapper();

            ConfigureSwagger(services);
            Configure(services);
            ConfigureEntityFramework(services);
            ConfigureHandlers(services);
            ConfigureDecorators(services);
            ConfigureValidators(services);

            services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = "http://ids:5003";
                options.RequireHttpsMetadata = false;
                options.ApiName = "arta.bulkchangemsisdn.api";
            });            

            ConfigureConfiguration(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
        app.UseCors(builder =>
               builder.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()

            );

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Arta BulkChangeMSISDN Api");
            });

            app.UseCustomExceptionHandler();
            app.UseRequestResponseLogging();
            app.UseAuthentication();
            app.UseBugsBunny();
            app.UseMvc();
        }

        private void ConfigureEntityFramework(IServiceCollection services)
        {
            services.AddDbContext<ResellerContext>();
        }

        private void Configure(IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();
            services.AddScoped<IApiLogger, ApiLogger>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHttpService, HttpService>();
            services.AddScoped<IPartnerFetcher, PartnerFetcher>();
        }

        private void ConfigureConfiguration(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
        }

        private void ConfigureHandlers(IServiceCollection services)
        {
            services.Scan(scan => scan.FromEntryAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            services.Scan(scan => scan.FromEntryAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            services.Scan(scan => scan.FromEntryAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IAsyncRequestHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );
            
            services.Scan(scan => scan.FromEntryAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IAsyncRequestHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );
        }

        //Order of registration is important. The decorator that gets registered last will be the first to execute
        private static void ConfigureDecorators(IServiceCollection services)
        {
            //services.AddScoped(typeof(IRequestHandlerDecorator<,>), typeof(TransactionDecorator<,>));
            services.AddScoped(typeof(IRequestHandlerDecorator<,>), typeof(ValidationDecorator<,>));
        }

        private void ConfigureValidators(IServiceCollection services)
        {
            services.Scan(scan => scan.FromEntryAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Arta BulkChangeMSISDN Api", Version = "v1" });
                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Arta.BulkChangeMSISDN.Api.xml");
                c.IncludeXmlComments(filePath);
                c.CustomSchemaIds(x => x.FullName);
            });
        }
    }
}

```
appsettings
```
"Serilog": {
    "WriteTo": [
      {
        "Name": "RollingFile",
        "Args": { "pathFormat": "C:/temp/microservices/ServiceTemplate-{Date}.txt" }
      }
    ]
  },

  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost;Initial Catalog=GucciGang;User ID=sa;Password=pwd;Trusted_Connection=no;"
  }
}
```
