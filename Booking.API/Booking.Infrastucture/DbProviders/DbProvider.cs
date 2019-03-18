using Booking.Domains.Filters;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Infrastucture.DbProviders
{
    public abstract class DbProvider<T, TDto> : IDbProvider
    {
        protected IMongoDatabase db { get; }

        protected DbProvider(IOptions<Configuration> config)
        {
            var settings = new MongoClientSettings
            {
                Credential = MongoCredential.CreateCredential(
                    "admin",
                    config.Value.MongoUsername,
                    config.Value.MongoPassword),
                Server = new MongoServerAddress(config.Value.MongoIp, config.Value.MongoPort)
            };
            var mongoClient = new MongoClient(settings);
            db = mongoClient.GetDatabase(config.Value.MongoDatabase);
        }

        protected async Task Write(string collectionName, BsonDocument reservation)
        {
            var collection = db.GetCollection<BsonDocument>(collectionName);
            await collection.InsertOneAsync(reservation).ConfigureAwait(false);
        }

        protected async Task<IList<TDto>> Read(string collectionName, IFilter filter)
        {
            var result = new List<TDto>();
            var collection = db.GetCollection<TDto>(collectionName);

            using (var cursor = await collection.FindAsync(filter.ToBsonDocument()).ConfigureAwait(false))
            {
                int count = 0;
                while (await cursor.MoveNextAsync().ConfigureAwait(false) &&
                  count >= filter.RangeStart &&
                  count <= filter.Amount)
                {
                    var bson = cursor.Current;
                    result.AddRange(bson);
                    count++;
                }
            }

            return result;
        }

        protected async Task<bool> Delete(string collectionName, IFilter filter)
        {
            var collection = db.GetCollection<BsonDocument>(collectionName);
            var result = await collection.DeleteOneAsync(filter.ToBsonDocument()).ConfigureAwait(false);

            return result.DeletedCount == 1;
        }

        protected async Task<bool> Update(string collectionName, IFilter filter, TDto dto)
        {
            var collection = db.GetCollection<BsonDocument>(collectionName);
            var result = await collection.UpdateOneAsync(filter.ToBsonDocument(), dto.ToBsonDocument()).ConfigureAwait(false);

            return result.ModifiedCount > 0;
        }

        public abstract Task<IList<T>> ReadModel(IFilter filter);
        public abstract Task<bool> Update(IFilter filter);
        public abstract Task<bool> Delete(IFilter filter);
        public abstract Task WriteModel(T doc);

    }
}
