namespace Booking.Domains.Filters
{
    public interface IFilter
    {
        int RangeStart { get; set; }
        int Amount { get; set; }
    }
}
