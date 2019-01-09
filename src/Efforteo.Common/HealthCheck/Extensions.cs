using System;
using System.Collections.Generic;
using System.Text;
using BeatPulse;
using Efforteo.Common.Mongo;
using Efforteo.Common.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Efforteo.Common.HealthCheck
{
    public static class Extensions
    {
        public static void AddHealthCheck(this IServiceCollection services, IConfiguration configuration,
            ILogger logger)
        {
            services.AddBeatPulse(setup =>
            {
                setup.AddRabbitMQ(FetchRabbitMqConnectionString(configuration, logger));
                setup.AddMongoDb(FetchMongoDbConnectionString(configuration, logger));
            });
        }

        private static string FetchRabbitMqConnectionString(IConfiguration configuration, ILogger logger)
        {
            var options = new RabbitMqOptions();
            var section = configuration.GetSection("rabbitmq");
            section.Bind(options);

            var connectionString =
                $"amqp://{options.Username}:{options.Password}@{options.Hostnames[0]}:{options.Port}/{options.VirtualHost}";

            logger.Log(LogLevel.Information, $"Fetched RabbitMQ ConnectionString = {connectionString}");

            return connectionString;
        }

        private static string FetchMongoDbConnectionString(IConfiguration configuration, ILogger logger)
        {
            var options = new MongoSettings();
            var section = configuration.GetSection("mongo");
            section.Bind(options);

            var connectionString = options.ConnectionString;

            logger.Log(LogLevel.Information, $"Fetched MongoDB ConnectionString = {connectionString}");

            return connectionString;
        }
    }
}
