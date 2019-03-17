using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Booking.Domains.ClientModels;

namespace Validators
{
    public interface ISemanticValidator<T>
    {
        Task<IList<ValidationResult>> Validate(T model);
    }
}