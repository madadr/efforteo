using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using Efforteo.Common.Commands;

namespace Efforteo.Common.IoC.Modules
{
    public class DispatcherModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<CommandDispatcher>()
                .As<ICommandDispatcher>()
                .InstancePerLifetimeScope();
        }
    }
}