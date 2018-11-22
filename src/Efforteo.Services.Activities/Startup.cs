using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Efforteo.Common.Auth;
using Efforteo.Common.Commands;
using Efforteo.Common.Exceptions;
using Efforteo.Common.IoC.Modules;
using Efforteo.Common.Mongo;
using Efforteo.Common.RabbitMq;
using Efforteo.Common.Settings;
using Efforteo.Services.Activities.Domain.Repositories;
using Efforteo.Services.Activities.Handlers;
using Efforteo.Services.Activities.IoC.Modules;
using Efforteo.Services.Activities.Repositories;
using Efforteo.Services.Activities.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace Efforteo.Services.Activities
{
    public class Startup
    {
        private readonly ILogger _logger;
        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Configuring services");

            services.AddMvcCore()
                .AddJsonFormatters()
                .AddDataAnnotations()
                .AddAuthorization();

            services.AddJwt(Configuration);
            services.AddMongoDb(Configuration, _logger);
            services.AddRabbitMq(Configuration, _logger);

            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterModule<ContainerModule>();

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

            app.UseAuthentication();
            app.ApplicationServices.GetService<IDatabaseSeeder>().SeedAsync();

            app.UseMiddleware<BasicExceptionHandlingMiddleware>();
            app.UseMvc();

            _logger.LogInformation("Configured application");
        }
    }
}