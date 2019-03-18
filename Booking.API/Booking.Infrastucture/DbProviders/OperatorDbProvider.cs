using Booking.Domains.ClientModels;
using Booking.Domains.Filters;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Infrastucture.DbProviders
{
    public class OperatorDbProvider : DbProvider<Operator, Domains.DTO.Operator>
    {
        private string collectionName { get; } = "operators";

        public OperatorDbProvider(IOptions<Configuration> config) : base(config)
        { }

        public override Task<bool> Delete(IFilter filter)
        {
            throw new NotImplementedException();
        }

        public override async Task<IList<Operator>> ReadModel(IFilter filter)
        {
            var result = new List<Operator>();
            var ops = await Read(collectionName, filter).ConfigureAwait(false);

            foreach (var op in ops)
            {
                result.Add(new Operator(op));
            }

            return result;
        }

        public override Task<bool> Update(IFilter filter)
        {
            throw new NotImplementedException();
        }

        public override async Task WriteModel(Operator op)
        {
            await Write(collectionName, op.GetDTO().ToBsonDocument()).ConfigureAwait(false);
        }
    }
}
