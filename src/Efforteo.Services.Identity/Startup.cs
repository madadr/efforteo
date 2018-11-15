using AutoMapper;
using Efforteo.Common.Auth;
using Efforteo.Common.Commands;
using Efforteo.Common.Exceptions;
using Efforteo.Common.Mongo;
using Efforteo.Common.RabbitMq;
using Efforteo.Services.Identity.Domain.DTO;
using Efforteo.Services.Identity.Domain.Models;
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
using Microsoft.Extensions.Logging;

namespace Efforteo.Services.Identity
{
    public class Startup
    {
        private readonly ILogger _logger;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Configuring services...");
            services.AddMvcCore()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonFormatters();
            _logger.LogDebug("Configuring MongoDb...");
            services.AddMongoDb(Configuration);
            _logger.LogDebug("Configuring JWT...");
            services.AddJwt(Configuration);
            services.AddScoped<IEncrypter, Encrypter>();
            _logger.LogDebug("Configuring RabbitMQ...");
            services.AddRabbitMq(Configuration);

            _logger.LogDebug("Configuring other services...");
            services.AddSingleton<IMapper>(
                new MapperConfiguration(cfg => { cfg.CreateMap<User, UserDto>(); })
                    .CreateMapper());
            services.AddScoped<ICommandHandler<CreateUser>, CreateUserHandler>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            _logger.LogInformation("Configured all services");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            _logger.LogInformation("Configuring HTTP request pipeline...");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

//            app.ApplicationServices.GetService<IDatabaseSeeder>().SeedAsync();
//            app.ApplicationServices.GetService<IUserService>().RegisterAsync("user2@user.com", "Stronk111", "User");

            app.UseMiddleware<BasicExceptionHandlingMiddleware>();
            app.UseMvc();

            _logger.LogInformation("Configured HTTP request pipeline");
        }
    }
}