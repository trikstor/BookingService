using BookingService.Domains;
using BookingService.Domains.ClientModels;
using BookingService.Infrastucture.DbProviders;
using System.Threading.Tasks;

namespace Booking.Validation
{
    public class ReservationValidator
    {
        public static async Task<bool> IsRightTimePosition(Reservation reservation, ReservationDbProvider dbProvider)
        {
            var filter = new ReservationFilter
            {
                Day = reservation.Date,
                Amount = int.MaxValue
            };
            var allReservationOfDay = await dbProvider.ReadModel(filter).ConfigureAwait(false);
            var endRes = reservation.Date.Add(reservation.Duration);

            foreach (var currentRes in allReservationOfDay)
            {
                if (currentRes.TableId == reservation.TableId && currentRes.Date < reservation.Date)
                {
                    var endCurrRes = currentRes.Date.Add(currentRes.Duration);
                    if (endCurrRes < reservation.Date)
                        return false;
                }
                if (currentRes.TableId == reservation.TableId && currentRes.Date > reservation.Date)
                {
                    if (endRes < currentRes.Date)
                        return false;
                }
            }
            return true;
        }
    }
}
