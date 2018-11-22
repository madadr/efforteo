using System;
using System.Collections.Generic;
using System.Text;
using Efforteo.Common.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Efforteo.Common.Auth
{
    public static class Extensions
    {
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
//            var options = new JwtSettings();
//            var section = configuration.GetSection("jwt");
//            section.Bind(options);
            var sets = configuration.GetSettings<JwtSettings>();
//            services.Configure<JwtSettings>(section);
//            services.AddSingleton<IJwtHandler, JwtHandler>();
            services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidIssuer = sets.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sets.SecretKey))
                    };
                });
        }
    }
}
