﻿using CQRSQuery.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CQRSQuery.Database
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<OrderQuery> Orders => _database.GetCollection<OrderQuery>("Orders");
    }
}
