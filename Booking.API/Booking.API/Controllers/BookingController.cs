using Booking.Domains;
using Booking.Domains.DTO;
using Booking.Domains.Filters;
using Booking.Infrastucture;
using Booking.Infrastucture.DbProviders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Booking.Validators;

namespace Booking.API.Controllers
{
    [Route("api/[controller]")]
    public class BookingController : Controller
    {
        private ReservationDbProvider reservationsDbProvider { get; }
        private HisoryRecordDbProvider historyDbProvider { get; }

        public BookingController(
        IOptions<Configuration> config, 
        ReservationDbProvider reservationDbProvider, 
        HisoryRecordDbProvider historyRecordDbProvider)
        {
            reservationsDbProvider = reservationDbProvider;
            historyDbProvider = historyRecordDbProvider;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get(ReservationFilter filter)
        {
            var nlRes = await reservationsDbProvider.ReadModel(filter).ConfigureAwait(false);
            var res = nlRes.ToList();

            if (filter.Day.HasValue)
            {
                filter.Day = filter.Day.Value.Date.AddDays(-1);
                var preDayRes = await reservationsDbProvider.ReadModel(filter).ConfigureAwait(false);
                var withAndInNextDay = preDayRes.Where(r => r.Date + r.Duration > filter.Day);
                res.AddRange(withAndInNextDay);
            }

            return Ok(res);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Domains.ClientModels.Reservation reservation)
        {
            var checkReport = new List<ValidationResult>();
            Validator.TryValidateObject(reservation, new ValidationContext(reservation), checkReport, true);
            var rightTimePosition = new ReservationSemanticValidator(reservationsDbProvider);
            var semanticValidationReport = await rightTimePosition.Validate(reservation).ConfigureAwait(false);
            checkReport.AddRange(semanticValidationReport);
            
            if (checkReport.Count > 0)
                return BadRequest(checkReport);
            
            await reservationsDbProvider.WriteModel(reservation).ConfigureAwait(false);

            var historyRecord = new HistoryRecord(ActionTypes.AddReservation, reservation.Id, DateTime.Now);
            await historyDbProvider.WriteModel(historyRecord).ConfigureAwait(false);

            return Ok();
        }

        [Authorize]
        [HttpDelete("id/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var filter = new ReservationFilter
            {
                Id = id,
                Amount = 1
            };
            var result = await reservationsDbProvider.Delete(filter).ConfigureAwait(false);

            if (!result) return NotFound();
            var historyRecord = new HistoryRecord(ActionTypes.DeleteReservation, id, DateTime.Now);
            await historyDbProvider.WriteModel(historyRecord).ConfigureAwait(false);

            return Ok();
        }

        [Authorize]
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory(HistoryFilter filter)
        {
            var history = await historyDbProvider.ReadModel(filter).ConfigureAwait(false);
            var res = history.ToList();

            return Ok(res);
        }
    }
}
