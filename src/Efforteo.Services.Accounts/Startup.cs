using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Efforteo.Common.Auth;
using Efforteo.Common.Commands;
using Efforteo.Common.Exceptions;
using Efforteo.Common.IoC.Modules;
using Efforteo.Common.Mongo;
using Efforteo.Common.RabbitMq;
using Efforteo.Services.Accounts.Domain.DTO;
using Efforteo.Services.Accounts.Domain.Models;
using Efforteo.Services.Accounts.Domain.Repositories;
using Efforteo.Services.Accounts.Domain.Services;
using Efforteo.Services.Accounts.Handlers;
using Efforteo.Services.Accounts.Repositories;
using Efforteo.Services.Accounts.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Efforteo.Services.Accounts
{
    public class Startup
    {
        private readonly ILogger _logger;
        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Configuring services");
            services.AddMvcCore()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonFormatters();

            var builder = new ContainerBuilder();

            _logger.LogDebug("Configuring misc services");
            builder.RegisterInstance(new MapperConfiguration(cfg => { cfg.CreateMap<User, UserDto>(); }).CreateMapper())
                .SingleInstance();

            builder.RegisterModule<DispatcherModule>();

            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .SingleInstance();
            builder.RegisterType<UserService>()
                .As<IUserService>()
                .SingleInstance();

            _logger.LogDebug("Configuring JWT");
            services.AddJwt(Configuration);
            services.AddScoped<IEncrypter, Encrypter>();

            _logger.LogDebug("Configuring MongoDb");
            services.AddMongoDb(Configuration, _logger);

            _logger.LogDebug("Configuring RabbitMQ");
            services.AddRabbitMq(Configuration, _logger);

            builder.Populate(services);

            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<BasicExceptionHandlingMiddleware>();
            app.UseMvc();
        }
    }
}