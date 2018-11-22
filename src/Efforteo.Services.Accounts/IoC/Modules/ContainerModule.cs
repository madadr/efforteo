using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Efforteo.Common.IoC.Modules;
using Efforteo.Services.Accounts.Domain.DTO;
using Efforteo.Services.Accounts.Domain.Models;
using Efforteo.Services.Accounts.Domain.Repositories;
using Efforteo.Services.Accounts.Domain.Services;
using Efforteo.Services.Accounts.Repositories;
using Efforteo.Services.Accounts.Services;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Efforteo.Services.Accounts.IoC.Modules
{
    public class ContainerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new MapperConfiguration(cfg => { cfg.CreateMap<User, UserDto>(); }).CreateMapper())
                .SingleInstance();

            builder.RegisterType<Encrypter>()
                .As<IEncrypter>()
                .SingleInstance();

            builder.RegisterModule<DispatcherModule>();

            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UserService>()
                .As<IUserService>()
                .InstancePerLifetimeScope();

        }
    }
}
