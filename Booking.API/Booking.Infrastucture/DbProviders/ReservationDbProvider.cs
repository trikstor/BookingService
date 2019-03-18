using Booking.Domains.ClientModels;
using Booking.Domains.Filters;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Infrastucture.DbProviders
{
    public class ReservationDbProvider : DbProvider<Reservation, BsonDocument>
    {
        private string collectionName { get; } = "reservations";

        public ReservationDbProvider(IOptions<Configuration> config) : base(config)
        { }

        public override async Task<bool> Delete(IFilter filter)
        {
            var reservations = await Read(collectionName, filter).ConfigureAwait(false);
            if (reservations.Count == 0)
                return false;

            var bson = reservations.First();
            var reservationDTO = BsonSerializer.Deserialize<Domains.DTO.Reservation>(bson);
            reservationDTO.IsDeleted = true;

            return await Update(collectionName, filter, reservationDTO.ToBsonDocument()).ConfigureAwait(false);
        }

        public override async Task WriteModel(Reservation doc)
        {
            var dto = doc.GetDTO();

            await Write("clients", dto.Item1.ToBsonDocument()).ConfigureAwait(false);
            await Write(collectionName, dto.Item2.ToBsonDocument()).ConfigureAwait(false);
        }

        public override async Task<IList<Reservation>> ReadModel(IFilter filter)
        {
            var result = new List<Reservation>();
            var bsons = await Read(collectionName, filter).ConfigureAwait(false);
            foreach (var bson in bsons)
            {
                var reservationDTO = BsonSerializer.Deserialize<Domains.DTO.Reservation>(bson);
                Console.WriteLine(reservationDTO.Day);
                var clientFilter = new ClientFilter
                {
                    Id = reservationDTO.ClientId,
                    Amount = 1
                };

                var clientBsons = await Read("clients", clientFilter).ConfigureAwait(false);
                var clientBson = clientBsons.First();
                var clientDTO = BsonSerializer.Deserialize<Domains.DTO.Client>(clientBson);
                result.Add(new Reservation(reservationDTO, clientDTO));
            }

            return result;
        }

        public override Task<bool> Update(IFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
