using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using FastDeepCloner;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDbTest.Configuration;
using MongoDbTest.Extensions;

namespace MongoDbTest.GenericRepository.Repository
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : IMongoModel,new()
    {
        private readonly MongoDbConfiguration _configuration;
        public IMongoClient MongoClient { get; private set; }
        public IMongoDatabase Db { get; private set; }

        public IMongoCollection<TEntity> Collection { get;  }

        
        public Repository(IOptions<MongoDbConfiguration> configuration)
        {
            _configuration = configuration.Value;

            MongoClient = new MongoClient(_configuration.ConnectionString);
            Db = MongoClient.GetDatabase(_configuration.DataBaseName);

            var tableName = typeof(TEntity).Name;
            Collection = Db.GetCollection<TEntity>(tableName);

            #region Initialization Of Class Maps

            SetProperAttributes();
            #endregion
        }

        public async Task<IEnumerable<TEntity>> GetCollection()
        {
           
           
            var dataList = await Collection.Find(FilterDefinition<TEntity>.Empty).ToListAsync();
            return dataList;
        }

        public async Task Create(TEntity entity)
        {
            await Collection.InsertOneAsync(entity);
        }

        public async Task<bool> Update(TEntity entity, FilterDefinition<TEntity> filter)
        {
          var result=  await Collection.ReplaceOneAsync(filter, entity);

          return result.IsAcknowledged;
        }

        public async Task<List<TEntity>> GetWithFilter(FilterDefinition<TEntity> filter)
        { 
            var result = await Collection.Find(filter).ToListAsync();
            return result;
        }

        public async Task<bool> Delete(FilterDefinition<TEntity> filter)
        {
            var result = await Collection.DeleteOneAsync(filter);
            return result.IsAcknowledged;
        }


        #region PrivateMethods (Currently Useless Attempts)

        private void SetProperAttributes()
        {
            #region Some Attempts With Reflection

          //  Entity = new TEntity();

          //  if (!typeof(TEntity).IsClass)
          //      throw new Exception("Given Type is not a class");

          //  PropertyOverridingTypeDescriptor ctd = new PropertyOverridingTypeDescriptor(TypeDescriptor.GetProvider(typeof(TEntity)).GetTypeDescriptor(typeof(TEntity)));

          //  foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(TEntity)))
          //  {
          //      if (pd.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase) && pd.PropertyType == typeof(string))
          //      {

          //          PropertyDescriptor pd2 =
          //              TypeDescriptor.CreateProperty(
          //                  typeof(TEntity), // or just _settings, if it's already a type
          //                  pd, // base property descriptor to which we want to add attributes
          //                      // The PropertyDescriptor which we'll get will just wrap that
          //                      // base one returning attributes we need.
          //                  new BsonIdAttribute(),
          //                  new BsonRepresentationAttribute(BsonType.ObjectId)
          //              // this method really can take as many attributes as you like,
          //              // not just one
          //              );


          //          ctd.OverrideProperty(pd2);

          //          continue;
          //      }

          //      var bsonElementName = pd.Name.ToLowerFirstChar();


          //      PropertyDescriptor pd3 =
          //          TypeDescriptor.CreateProperty(
          //              typeof(TEntity), // or just _settings, if it's already a type
          //              pd, // base property descriptor to which we want to add attributes
          //                  // The PropertyDescriptor which we'll get will just wrap that
          //                  // base one returning attributes we need.
          //          new BsonElementAttribute(bsonElementName)
          //          // this method really can take as many attributes as you like,
          //          // not just one
          //          );


          //      ctd.OverrideProperty(pd3);

          //  }

          //  // then we add new descriptor provider that will return our descriptor instead of default
          //  TypeDescriptor.AddProvider(new TypeDescriptorOverridingProvider(ctd), Entity);

          //var c=

          var props = typeof(TEntity).GetFastDeepClonerProperties();

          foreach (var prop in props)
          {
              if(prop.PropertyType==typeof(ObjectId))
                  continue;

              var propName = prop.Name.ToLowerFirstChar();

              prop.Attributes.Add(new BsonElementAttribute(propName));
          }


          foreach (var propertyInfo in typeof(TEntity).GetProperties())
          {
              var tt = propertyInfo.GetCustomAttributes<BsonElementAttribute>();
          }

          #endregion

        }




        #endregion
    }
}
