namespace Booking.Domains
{
    public interface IFilter
    {
        int RangeStart { get; set; }
        int Amount { get; set; }
    }
}
