using Autofac;
using AutoMapper;
using Efforteo.Common.Events;
using Efforteo.Common.IoC.Modules;
using Efforteo.Services.Accounts.Domain.DTO;
using Efforteo.Services.Accounts.Domain.Models;
using Efforteo.Services.Accounts.Domain.Repositories;
using Efforteo.Services.Accounts.Handlers;
using Efforteo.Services.Accounts.Repositories;
using Efforteo.Services.Accounts.Services;

namespace Efforteo.Services.Accounts.IoC.Modules
{
    public class ContainerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new MapperConfiguration(cfg => { cfg.CreateMap<Account, AccountDto>(); }).CreateMapper())
                .SingleInstance();

            builder.RegisterType<AccountRepository>()
                .As<IAccountRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<AccountService>()
                .As<IAccountService>()
                .InstancePerLifetimeScope();

            builder.RegisterModule<DispatcherModule>();

            // Dispatcher not working well for now, so ...
            builder.RegisterType<UserCreatedHandler>()
                .As<IEventHandler<UserCreated>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UserAuthenticatedHandler>()
                .As<IEventHandler<UserAuthenticated>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UserRemovedHandler>()
                .As<IEventHandler<UserRemoved>>()
                .InstancePerLifetimeScope();
        }
    }
}
