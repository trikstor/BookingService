using Booking.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Booking.Domains.ClientModels
{
    public class Operator : IClientModel
    {
        [Required]
        public string OrgName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }

        public DTO.Operator GetDTO()
        {
            return new DTO.Operator
            {
                Id = Guid.NewGuid(),
                OrgName = OrgName,
                Email = Email,
                Phone = Phone,
                //TODO
                Password = Cryptography.SaltPassword(Password, "salt")
            };
        }

        public Operator()
        { }

        public Operator(DTO.Operator opDto)
        {
            OrgName = opDto.OrgName;
            Email = opDto.Email;
            Phone = opDto.Phone;
            Password = opDto.Password;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var checkReport = new List<ValidationResult>();
            Validator.TryValidateObject(this, validationContext, checkReport, true);

            return checkReport;
        }
    }
}
