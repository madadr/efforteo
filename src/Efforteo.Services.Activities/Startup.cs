using System;
using Efforteo.Common.Auth;
using Efforteo.Common.Commands;
using Efforteo.Common.Exceptions;
using Efforteo.Common.Mongo;
using Efforteo.Common.RabbitMq;
using Efforteo.Services.Activities.Domain.Repositories;
using Efforteo.Services.Activities.Handlers;
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

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Configuring services");
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            _logger.LogDebug("Configuring MongoDb.");
            services.AddMongoDb(Configuration, _logger);

            _logger.LogDebug("Configuring RabbitMQ.");
            services.AddRabbitMq(Configuration, _logger);


            _logger.LogDebug("Configuring JWT");
            services.AddJwt(Configuration);

            _logger.LogDebug("Configuring other services");
            services.AddScoped<IDatabaseSeeder, CustomMongoSeeder>();
            services.AddScoped<ICommandHandler<CreateActivity>, CreateActivityHandler>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IActivityService, ActivityService>();

            _logger.LogInformation("Configured all services");
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