using Autofac;
using AutoMapper;
using Efforteo.Common.Events;
using Efforteo.Common.IoC.Modules;
using Efforteo.Services.Stats.Domain.DTO;
using Efforteo.Services.Stats.Domain.Models;
using Efforteo.Services.Stats.Domain.Repositories;
using Efforteo.Services.Stats.Repositories;
using Efforteo.Services.Stats.Services;
using Efforteo.Services.Stats.Handlers;

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
                    cfg.CreateMap<CategoryDetailedStats, CategoryDetailedStatsDto>();
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
            builder.RegisterType<ActivityCreatedHandler>()
                .As<IEventHandler<ActivityCreated>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ActivityUpdatedHandler>()
                .As<IEventHandler<ActivityUpdated>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ActivityRemovedHandler>()
                .As<IEventHandler<ActivityRemoved>>()
                .InstancePerLifetimeScope();
        }
    }
}
