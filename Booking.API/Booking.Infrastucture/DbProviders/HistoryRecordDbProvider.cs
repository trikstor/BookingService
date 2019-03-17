using Booking.Domains;
using Booking.Domains.DTO;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Infrastucture.DbProviders
{
    public class HisoryRecordDbProvider : DbProvider<HistoryRecord, HistoryRecord>
    {
        private string collectionName { get; } = "history";

        public HisoryRecordDbProvider(IOptions<Configuration> config) : base(config)
        { }

        public override Task<bool> Delete(IFilter filter)
        {
            throw new NotImplementedException();
        }

        public override async Task<IList<HistoryRecord>> ReadModel(IFilter filter)
        {
            return await Read(collectionName, filter).ConfigureAwait(false);
        }

        public override Task<bool> Update(IFilter filter)
        {
            throw new NotImplementedException();
        }

        public override async Task WriteModel(HistoryRecord doc)
        {
            await Write(collectionName, doc.ToBsonDocument()).ConfigureAwait(false);
        }
    }
}
