using CQRSCommand.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CQRSCommand.Database
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        public IMongoClient Client { get; }

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            Client = new MongoClient(settings.Value.ConnectionString);
            _database = Client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<OrderQuery> Orders => _database.GetCollection<OrderQuery>("Orders");
    }
}
