using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Efforteo.Common.IoC.Modules;
using Efforteo.Common.Mongo;
using Efforteo.Services.Activities.Domain.Repositories;
using Efforteo.Services.Activities.Repositories;
using Efforteo.Services.Activities.Services;

namespace Efforteo.Services.Activities.IoC.Modules
{
    public class ContainerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
//            builder.RegisterInstance(new MapperConfiguration(cfg => { cfg.CreateMap<User, UserDto>(); }).CreateMapper())
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
        }
    }
}
