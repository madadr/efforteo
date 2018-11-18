using System;
using Efforteo.Api.Handlers;
using Efforteo.Common.Auth;
using Efforteo.Common.Events;
using Efforteo.Common.Exceptions;
using Efforteo.Common.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace Efforteo.Api
{
    public class Startup
    {
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Configuring services");
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            _logger.LogDebug("Configuring RabbitMQ");
            services.AddRabbitMq(Configuration, _logger);

            _logger.LogDebug("Configuring JWT");
            services.AddJwt(Configuration);

            _logger.LogDebug("Configuring other services");
            services.AddScoped<IEventHandler<ActivityCreated>, ActivityCreatedHandler>();

            _logger.LogInformation("Configured all services");
        }

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

            _logger.LogInformation("Configured application");
        }
    }
}