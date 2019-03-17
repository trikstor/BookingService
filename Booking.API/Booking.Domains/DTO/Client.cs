using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Booking.Domains.DTO
{
    public class Client
    {
        [BsonId]
        public Guid Id;
        public string Name;
        public string Surname;
        public string Email;
        public string Phone;
    }
}
