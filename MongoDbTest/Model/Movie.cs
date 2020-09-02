using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbTest.GenericRepository;
using Newtonsoft.Json;

namespace MongoDbTest.Model
{
    [BsonIgnoreExtraElements]
    public class Movie : IMongoModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }
        
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("test")]
        public string Test { get; set; }
    }
}
