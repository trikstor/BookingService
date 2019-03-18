using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Booking.Validators
{
    public interface ISemanticValidator<in T>
    {
        Task<IList<ValidationResult>> Validate(T model);
    }
}