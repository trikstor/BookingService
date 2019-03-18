using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Booking.Domains.ClientModels
{
    public class Reservation : IClientModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Surname { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
            ErrorMessage = "Некорректный форма адреса электронной почты")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^\+[2-9]\d{11}$",
            ErrorMessage = "Номер телефона должен иметь формат +xxxxxxxxxxx")]
        public string Phone { get; set; }

        [Required]
        public int DeviceId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        public TimeSpan Duration { get; set; }
        public int GuestsNumber { get; set; }
        public int TableId { get; set; }
        public double Discount { get; set; }
        public string Comment { get; set; }

        public Reservation()
        {
        }

        public Reservation(DTO.Reservation reservationDTO, DTO.Client clientDTO)
        {
            Id = reservationDTO.Id;
            Name = clientDTO.Name;
            Surname = clientDTO.Surname;
            Email = clientDTO.Email;
            Phone = clientDTO.Phone;
            DeviceId = reservationDTO.DeviceId;
            Date = reservationDTO.Date;
            Duration = reservationDTO.Duration;
            EndDate = Date + Duration;
            GuestsNumber = reservationDTO.GuestsNumber;
            TableId = reservationDTO.TableId;
            Discount = reservationDTO.Discount;
            Comment = reservationDTO.Comment;
        }

        public (DTO.Client, DTO.Reservation) GetDTO()
        {
            var clientId = Guid.NewGuid();

            return (new DTO.Client
            {
                Name = Name,
                Surname = Surname,
                Email = Email,
                Phone = Phone,
                Id = clientId
            },
            new DTO.Reservation
            {
                IsDeleted = false,
                DeviceId = DeviceId,
                Date = Date,
                Duration = Duration,
                GuestsNumber = GuestsNumber,
                CreatingDate = DateTime.UtcNow,
                TableId = TableId,
                Discount = Discount,
                Comment = Comment,
                ClientId = clientId,
                Id = Guid.NewGuid(),
                Day = Date.Date
            });
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var checkReport = new List<ValidationResult>();
            Validator.TryValidateObject(this, validationContext, checkReport, true);

            return checkReport;
        }
    }
}
