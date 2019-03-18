using Booking.Domains.ClientModels;
using Booking.Domains.Filters;
using Booking.Infrastucture.DbProviders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Booking.Validators
{
    public class ReservationSemanticValidator : ISemanticValidator<Reservation>
    {
        private ReservationDbProvider reservationDbProvider { get; }

        public ReservationSemanticValidator(IDbProvider reservationDbProvider)
        {
            this.reservationDbProvider = reservationDbProvider as ReservationDbProvider;
        }

        private async Task<bool> IsRightTimePosition(Reservation reservation)
        {
            var filter = new ReservationFilter
            {
                Day = reservation.Date.Date,
                TableId = reservation.TableId
            };
            var allReservationOfDay = await reservationDbProvider.ReadModel(filter).ConfigureAwait(false);
            if (allReservationOfDay.Count == 0)
            {
                filter.Day = filter.Day.Value.AddDays(-1);
                allReservationOfDay = await reservationDbProvider.ReadModel(filter).ConfigureAwait(false);
            }

            var endRes = reservation.EndDate;
            foreach (var currentRes in allReservationOfDay)
            {
                if (currentRes.TableId == reservation.TableId &&
                    !((currentRes.Date < reservation.Date && currentRes.EndDate < reservation.Date)
                      || (currentRes.Date > reservation.Date && currentRes.EndDate > reservation.Date)))
                    return false;
            }

            return true;
        }

        public async Task<IList<ValidationResult>> Validate(Reservation model)
        {
            var checkReport = new List<ValidationResult>();

            var isRightTimePosition = await IsRightTimePosition(model).ConfigureAwait(false);
            if (!isRightTimePosition)
                checkReport.Add(new ValidationResult("Время совпадает с другой бронью. Выберите другое время."));

            return checkReport;
        }
    }
}