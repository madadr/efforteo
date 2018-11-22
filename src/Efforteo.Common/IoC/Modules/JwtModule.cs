using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using Efforteo.Common.Auth;
using Efforteo.Common.Commands;
using Microsoft.Extensions.Configuration;

namespace Efforteo.Common.IoC.Modules
{
    public class JwtModule : Autofac.Module
    {
//        private readonly IConfiguration _configuration;
//        private readonly JwtSettings _options;

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JwtHandler>()
                .As<IJwtHandler>()
                .SingleInstance();
        }
    }
}