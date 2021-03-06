﻿using System;
using Efforteo.Common.Commands;
using Efforteo.Common.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Instantiation;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;

namespace Efforteo.Common.RabbitMq
{
    public static class Extensions
    {
        public static Task WithCommandHandlerAsync<TCommand>(this IBusClient bus, ICommandHandler<TCommand> handler)
            where TCommand : ICommand =>
            bus.SubscribeAsync<TCommand>(
                msg => handler.HandleAsync(msg),
                ctx => ctx.UseSubscribeConfiguration(
                    cfg => cfg.FromDeclaredQueue(q => q.WithName(GetQueueName<TCommand>()))));

        public static Task WithEventHandlerAsync<TEvent>(this IBusClient bus, IEventHandler<TEvent> handler)
            where TEvent : IEvent =>
            bus.SubscribeAsync<TEvent>(msg => handler.HandleAsync(msg),
                ctx => ctx.UseSubscribeConfiguration(
                    cfg => cfg.FromDeclaredQueue(q => q.WithName(GetQueueName<TEvent>()))));

        private static string GetQueueName<T>() => $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}";

        public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            var options = new RabbitMqOptions();
            var section = configuration.GetSection("rabbitmq");
            section.Bind(options);
            Policy.Handle<Exception>()
                .WaitAndRetryForever(r => TimeSpan.FromSeconds(5),
                    (ex, ts) =>
                    {
                        logger.LogError($"Failed to configure RabbitMQ. Retrying in {ts}. Reason=[{ex.Message}]");
                    })
                .Execute(() =>
                {
                    var client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions {ClientConfiguration = options});
                    services.AddSingleton<IBusClient>(_ => client);
                });
        }
    }
}