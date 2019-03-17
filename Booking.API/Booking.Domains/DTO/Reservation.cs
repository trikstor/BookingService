using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Booking.Domains.DTO
{
    public class Reservation
    {
        [BsonId]
        public Guid Id;
        public bool IsDeleted;
        public DateTime CreatingDate;
        public Guid ClientId;
        public int DeviceId;
        public DateTime Date;
        public DateTime Day;
        public TimeSpan Duration;
        public int GuestsNumber;
        public int TableId;
        public double Discount;
        public string Comment;
    }
}
