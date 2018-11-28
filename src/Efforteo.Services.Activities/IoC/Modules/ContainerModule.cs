using Autofac;
using AutoMapper;
using Efforteo.Common.Commands;
using Efforteo.Common.IoC.Modules;
using Efforteo.Common.Mongo;
using Efforteo.Services.Activities.Domain.DTO;
using Efforteo.Services.Activities.Domain.Models;
using Efforteo.Services.Activities.Domain.Repositories;
using Efforteo.Services.Activities.Handlers;
using Efforteo.Services.Activities.Repositories;
using Efforteo.Services.Activities.Services;

namespace Efforteo.Services.Activities.IoC.Modules
{
    public class ContainerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new MapperConfiguration(cfg => { cfg.CreateMap<Activity, ActivityDto>(); }).CreateMapper())
                .SingleInstance();

//            builder.RegisterInstance(new MapperConfiguration(cfg => { cfg.CreateMap<Activity, ActivityDto>(); }).CreateMapper())
//                .SingleInstance();

            builder.RegisterType<CustomMongoSeeder>()
                .As<IDatabaseSeeder>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ActivityRepository>()
                .As<IActivityRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CategoryRepository>()
                .As<ICategoryRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ActivityService>()
                .As<IActivityService>()
                .InstancePerLifetimeScope();

            builder.RegisterModule<DispatcherModule>();

            // Dispatcher not working well for now, so ...
            builder.RegisterType<CreateActivityHandler>()
                .As<ICommandHandler<CreateActivity>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<RemoveActivityHandler>()
                .As<ICommandHandler<RemoveActivity>>()
                .InstancePerLifetimeScope();
        }
    }
}
