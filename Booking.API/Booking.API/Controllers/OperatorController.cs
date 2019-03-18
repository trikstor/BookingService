using Booking.Domains.ClientModels;
using Booking.Domains.Filters;
using Booking.Helpers;
using Booking.Infrastucture;
using Booking.Infrastucture.DbProviders;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Booking.API.Controllers
{
    [Route("api/[controller]")]
    public class OperatorController : Controller
    {
        private readonly Configuration config;
        private readonly OperatorDbProvider dbProvider;

        public OperatorController(IOptions<Configuration> config)
        {
            this.config = config.Value;
            dbProvider = new OperatorDbProvider(config);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Operator op)
        {
            var filter = new OperatorFilter
            {
                Email = op.Email,
                Amount = 1
            };
            var res = await dbProvider.ReadModel(filter).ConfigureAwait(false);
            if (res.Count > 0)
                return Conflict("Пользователь с такой электронной почтой уже существует");
            filter = new OperatorFilter
            {
                Phone = op.Phone,
                Amount = 1
            };
            res = await dbProvider.ReadModel(filter).ConfigureAwait(false);
            if (res.Count > 0)
                return Conflict("Пользователь с таким номером телефона уже существует");

            await dbProvider.WriteModel(op).ConfigureAwait(false);
            return Ok("Вы успешно зарегистрированы");
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            email = email.ToLower();

            var filter = new OperatorFilter
            {
                Email = email,
                Amount = 1
            };

            var res = await dbProvider.ReadModel(filter).ConfigureAwait(false);
            //TODO
            if (res.Count == 0 || res.First().Password != Cryptography.SaltPassword(password, "salt"))
                return NotFound("Пользователь с такими реквизитами не найден");

            var encodedJwt = JwtHelpers.IssueToken(config.AppName, config.AppUrl, email, config.AuthLifetimeInHours, config.JwtTokenSecret);
            var response = new
            {
                access_token = encodedJwt,
                username = email
            };

            return Json(response);
        }
    }
}
