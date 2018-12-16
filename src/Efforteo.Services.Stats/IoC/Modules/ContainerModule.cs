using Autofac;
using AutoMapper;
using Efforteo.Common.Events;
using Efforteo.Common.IoC.Modules;
using Efforteo.Services.Stats.Domain.DTO;
using Efforteo.Services.Stats.Domain.Models;
using Efforteo.Services.Stats.Domain.Repositories;
using Efforteo.Services.Stats.Repositories;
using Efforteo.Services.Stats.Services;

namespace Efforteo.Services.Stats.IoC.Modules
{
    public class ContainerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new MapperConfiguration(cfg => {
                    cfg.CreateMap<Stat, StatDto>();
                    cfg.CreateMap<CategoryDetails, CategoryDetailsDto>();
                    cfg.CreateMap<CategoryTotal, CategoryTotalDto>();
                    cfg.CreateMap<Stat, ActivityPointer>();
            }).CreateMapper())
                .SingleInstance();

            builder.RegisterType<StatsRepository>()
                .As<IStatsRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<StatService>()
                .As<IStatService>()
                .InstancePerLifetimeScope();

            builder.RegisterModule<DispatcherModule>();

            // Dispatcher not working well for now, so ...
//            builder.RegisterType<UserCreatedHandler>()
//                .As<IEventHandler<UserCreated>>()
//                .InstancePerLifetimeScope();
//            builder.RegisterType<UserAuthenticatedHandler>()
//                .As<IEventHandler<UserAuthenticated>>()
//                .InstancePerLifetimeScope();
//            builder.RegisterType<UserRemovedHandler>()
//                .As<IEventHandler<UserRemoved>>()
//                .InstancePerLifetimeScope();
        }
    }
}
