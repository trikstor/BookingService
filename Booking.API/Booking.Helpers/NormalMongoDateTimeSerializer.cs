using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace Booking.Helpers
{
    public class NormalMongoDateTimeSerializer : DateTimeSerializer
    {
        public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var obj = base.Deserialize(context, args);
            return new DateTime(obj.Ticks, DateTimeKind.Unspecified);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
        {
            var utcValue = new DateTime(value.Ticks, DateTimeKind.Utc);
            base.Serialize(context, args, utcValue);
        }
    }


}
