using System.Reflection;
using log4net;

namespace Booking.Infrastucture
{
    public class Logger
    {
        public static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    }
}
