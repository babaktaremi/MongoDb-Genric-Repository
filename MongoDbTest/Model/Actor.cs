using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbTest.GenericRepository;

namespace MongoDbTest.Model
{
    public class Actor:IMongoModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }
    }
}
