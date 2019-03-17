using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Booking.Domains.Filters
{
    public class HistoryFilter : IFilter
    {
        public int RangeStart { get; set; }
        public int Amount { get; set; }
        [BsonIgnoreIfNull]
        [BsonId]
        public Guid? Id;
    }
}
