using Efforteo.Common.Auth;
using Efforteo.Common.Commands;
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

namespace Efforteo.Services.Activities
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
//                .AddJsonFormatters();
            services.AddMongoDb(Configuration);
            services.AddJwt(Configuration);
            //            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddScoped<IDatabaseSeeder, CustomMongoSeeder>();
            services.AddRabbitMq(Configuration);
            services.AddScoped<ICommandHandler<CreateActivity>, CreateActivityHandler>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IActivityService, ActivityService>();
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

            app.UseMvc();
        }
    }
}
