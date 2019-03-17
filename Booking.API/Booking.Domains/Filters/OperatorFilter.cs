using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Booking.Domains.Filters
{
    public class OperatorFilter : IFilter
    {
        private string email;
        public int RangeStart { get; set; }
        public int Amount { get; set; }
        [BsonIgnoreIfNull]
        public string Email
        {
            get { return email; }
            set { email = value.ToLower(); }
        }
        [BsonIgnoreIfNull]
        public string Phone { get; set; }
        [BsonIgnoreIfNull]
        [BsonId]
        public Guid? Id { get; set; }
    }
}
