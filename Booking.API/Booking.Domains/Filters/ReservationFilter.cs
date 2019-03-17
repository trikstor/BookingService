using Booking.Domains.ClientModels;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Booking.Domains
{
    public class ReservationFilter : IFilter
    {
        [BsonDefaultValue(0)]
        public int RangeStart { get; set; }
        [BsonDefaultValue(int.MaxValue)]
        public int Amount { get; set; }
        [BsonIgnoreIfNull]
        [BsonId]
        public Guid? Id { get; set; }
        [BsonIgnoreIfNull]
        public int? TableId { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? Day { get; set; }
        [BsonDefaultValue(false)]
        public bool IsDeleted { get; set; }

        [BsonConstructor]
        public ReservationFilter(int rangeStart, int amount, Guid? id, int? tableId, DateTime? day)
        {
            RangeStart = rangeStart;
            Amount = amount;
            Id = id;
            TableId = tableId;
            if (Day.HasValue)
            {
                Day = DateTime.SpecifyKind(Day.Value, DateTimeKind.Utc);
            }
            Day = day;
        }

        public ReservationFilter()
        { }
    }
}
