using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Efforteo.Common.Auth;
using Efforteo.Common.Exceptions;
using Efforteo.Common.Mongo;
using Efforteo.Common.RabbitMq;
using Efforteo.Services.Accounts.IoC.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Efforteo.Services.Accounts
{
    public class Startup
    {
        private readonly ILogger _logger;
        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

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

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
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

            app.UseMiddleware<BasicExceptionHandlingMiddleware>();
            app.UseMvc();
        }
    }
}