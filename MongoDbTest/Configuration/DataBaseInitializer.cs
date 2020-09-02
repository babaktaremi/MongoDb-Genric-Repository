using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbTest.GenericRepository;

namespace MongoDbTest.Configuration
{
    public interface IDataBaseInitializer
    {
        Task InitDatabase();
    }

    public class DataBaseInitializer:IDataBaseInitializer
    {
        private readonly IOptions<MongoDbConfiguration> _configuration;

        public IMongoClient MongoClient { get; private set; }
        public IMongoDatabase Db { get; private set; }

        public DataBaseInitializer(IOptions<MongoDbConfiguration> configuration)
        {
            _configuration = configuration;

            MongoClient = new MongoClient(_configuration.Value.ConnectionString);
            Db = MongoClient.GetDatabase(_configuration.Value.DataBaseName);
        }

        public async Task InitDatabase()
        {
            var type = typeof(IMongoModel);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));

            foreach (var item in types)
            {
                if(item.Name.Equals(nameof(IMongoModel)))
                    continue;

                if (!await this.CollectionExistsAsync(item.Name))
                    await Db.CreateCollectionAsync(item.Name);
            }
        }


        private async Task<bool> CollectionExistsAsync(string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            //filter by collection name
            var collections = await Db.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });
            //check for existence
            return await collections.AnyAsync();
        }
    }
}
