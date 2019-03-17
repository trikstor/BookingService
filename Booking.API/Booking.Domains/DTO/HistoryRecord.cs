using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Booking.Domains.DTO
{
    public class HistoryRecord
    {
        [BsonId]
        public Guid Id;
        public ActionTypes ActionType;
        public DateTime ActionTime;
        public Guid ReservationId;

        public HistoryRecord(ActionTypes actionType, Guid reservationId, DateTime actionTime)
        {
            Id = Guid.NewGuid();
            ActionType = actionType;
            ReservationId = reservationId;
            ActionTime = actionTime;
        }

        public HistoryRecord()
        { }
    }
}
