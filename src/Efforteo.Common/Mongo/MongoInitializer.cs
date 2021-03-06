﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;

namespace Efforteo.Common.Mongo
{
    public class MongoInitializer : IDatabaseInitializer
    {
        private bool _initialized;
        private readonly bool _seed;
        private readonly IDatabaseSeeder _seeder;

        public MongoInitializer(IOptions<MongoSettings> options, IDatabaseSeeder seeder, bool initialized = false)
        {
            _seed = options.Value.Seed;
            _seeder = seeder;
            _initialized = initialized;
        }

        public async Task InitializeAsync()
        {
            if (_initialized)
            {
                return;
            }

            RegisterConventions();
            _initialized = true;

            if (!_seed)
            {
                return;
            }

            await _seeder.SeedAsync();
        }

        private void RegisterConventions()
        {
            ConventionRegistry.Register("MongoConventions", new MongoConventions(), x => true);
        }

        private class MongoConventions : IConventionPack
        {
            public IEnumerable<IConvention> Conventions => new List<IConvention>
            {
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String),
                new CamelCaseElementNameConvention()
            };
        }
    }
}