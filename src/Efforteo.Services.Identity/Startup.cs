using Efforteo.Common.Auth;
using Efforteo.Common.Commands;
using Efforteo.Common.Mongo;
using Efforteo.Common.RabbitMq;
using Efforteo.Services.Identity.Domain.Repositories;
using Efforteo.Services.Identity.Domain.Services;
using Efforteo.Services.Identity.Handlers;
using Efforteo.Services.Identity.Repositories;
using Efforteo.Services.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Efforteo.Services.Identity
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
            services.AddMvcCore()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonFormatters();
            services.AddMongoDb(Configuration);
            services.AddJwt(Configuration);
//            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddRabbitMq(Configuration);
            services.AddScoped<ICommandHandler<CreateUser>, CreateUserHandler>();
            services.AddScoped<IEncrypter, Encrypter>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
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

            app.ApplicationServices.GetService<IDatabaseSeeder>().SeedAsync();
            app.ApplicationServices.GetService<IUserService>().RegisterAsync("user2@user.com", "Stronk111", "User");
//            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
