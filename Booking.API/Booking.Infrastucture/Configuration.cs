namespace Booking.Infrastucture
{
    public class Configuration
    {
        public string AppName { get; set; }
        public string AppUrl { get; set; }
        public string MongoIp { get; set; }
        public string JwtTokenSecret { get; set; }
        public int AuthLifetimeInHours { get; set; }
        public int MongoPort { get; set; }
        public string MongoUsername { get; set; }
        public string MongoPassword { get; set; }
        public string MongoDatabase { get; set; }
    }
}
