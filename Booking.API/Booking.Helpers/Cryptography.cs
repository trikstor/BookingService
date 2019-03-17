using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Booking.Helpers
{
    public class Cryptography
    {
        public static string SaltPassword(string password)
        {
            var crypt = new SHA256Managed();
            var hash = string.Empty;
            var crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password + "abba"));
            return crypto.Aggregate(hash, (current, theByte) => current + theByte.ToString("x2"));
        }
    }
}
