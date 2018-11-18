using System;
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
            services.Configure<MongoOptions>(configuration.GetSection("mongo"));
            services.AddSingleton<MongoClient>(c =>
            {
                var options = c.GetService<IOptions<MongoOptions>>();
                return new MongoClient(options.Value.ConnectionString);
            });
            Policy.Handle<Exception>()
                .WaitAndRetryForever(r => TimeSpan.FromSeconds(7.5),
                    (ex, ts) =>
                    {
                        logger.LogError($"Failed to configure MongoDb. Retrying in {ts}. Reason=[{ex.Message}]");
                    })
                .Execute(() =>
                {
                    services.AddScoped<IMongoDatabase>(c =>
                    {
                        var options = c.GetService<IOptions<MongoOptions>>();
                        var client = c.GetService<MongoClient>();

                        var database = client.GetDatabase(options.Value.Database);

                        bool isMongoLive = database.RunCommandAsync((Command<BsonDocument>) "{ping:1}").Wait(1000);

                        if (!isMongoLive)
                        {
                            throw new Exception("Mongo database connection not available");
                        }

                        return database;
                    });
                });
            services.AddScoped<IDatabaseInitializer, MongoInitializer>();
            services.AddScoped<IDatabaseSeeder, MongoSeeder>();
        }
    }
}