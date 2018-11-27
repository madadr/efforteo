using Autofac;
using AutoMapper;
using Efforteo.Common.IoC.Modules;
using Efforteo.Services.Authentication.Domain.DTO;
using Efforteo.Services.Authentication.Domain.Models;
using Efforteo.Services.Authentication.Domain.Repositories;
using Efforteo.Services.Authentication.Domain.Services;
using Efforteo.Services.Authentication.Repositories;
using Efforteo.Services.Authentication.Services;

namespace Efforteo.Services.Authentication.IoC.Modules
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

            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UserService>()
                .As<IUserService>()
                .InstancePerLifetimeScope();

            builder.RegisterModule<DispatcherModule>();
        }
    }
}
