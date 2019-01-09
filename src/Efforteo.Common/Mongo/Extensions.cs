using System;
using System.Linq;
using Autofac;
using Efforteo.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Polly;

namespace Efforteo.Common.Mongo
{
    public static class Extensions
    {
        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            var settings = configuration.GetSettings<MongoSettings>();
            services.Configure<MongoSettings>(configuration.GetSection("mongo"));
            MongoClient client = new MongoClient(settings.ConnectionString);
            services.AddSingleton<MongoClient>(c => client);
            Policy.Handle<Exception>()
                .WaitAndRetryForever(r => TimeSpan.FromSeconds(7.5),
                    (ex, ts) =>
                    {
                        logger.LogError($"Failed to configure MongoDb. Retrying in {ts}. Reason=[{ex.Message}]");
                    })
                .Execute(() =>
                {
                    var serviceDescriptor = services.FirstOrDefault(descriptor =>
                        descriptor.ServiceType == typeof(IMongoDatabase));
                    if (serviceDescriptor != null)
                    {
                        services.Remove(serviceDescriptor);
                    }
                    
                    var database = client.GetDatabase(settings.Database);

                    bool isMongoLive = database.RunCommandAsync((Command<BsonDocument>) "{ping:1}").Wait(1000);

                    if (!isMongoLive)
                    {
                        throw new Exception("Mongo database connection not available");
                    }

                    services.AddScoped<IMongoDatabase>(c => database);
                });
            services.AddScoped<IDatabaseInitializer, MongoInitializer>();
            services.AddScoped<IDatabaseSeeder, MongoSeeder>();
        }
    }
}