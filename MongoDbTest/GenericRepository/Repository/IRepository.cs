using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDbTest.GenericRepository.Repository
{
   public interface IRepository<TEntity> where TEntity:  IMongoModel,new()
    {
        Task<IEnumerable<TEntity>> GetCollection();
        Task Create(TEntity entity);
        Task<bool> Update(TEntity entity,FilterDefinition<TEntity> filter);
        Task<List<TEntity>> GetWithFilter(FilterDefinition<TEntity> filter);
        Task<bool> Delete(FilterDefinition<TEntity> filter);
    }
}
