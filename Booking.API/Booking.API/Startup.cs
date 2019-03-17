using Booking.Infrastucture;
using Booking.Infrastucture.DbProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Booking.API
{
    public class Startup
    {
        public IConfiguration Config { get; }

        public Startup(IConfiguration configuration)
        {
            Config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var config = Config.GetSection("AppConfig").Get<Configuration>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAllOrigin"));
            });

            services.AddOptions();
            services.Configure<Configuration>(x => Config.GetSection("AppConfig").Bind(x));

            services.AddTransient<IDbProvider, HisoryRecordDbProvider>();
            services.AddTransient<IDbProvider, ReservationDbProvider>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                                .AddJwtBearer(options =>
                                {
                                    options.RequireHttpsMetadata = false;
                                    options.TokenValidationParameters = new TokenValidationParameters
                                    {
                                        ValidateIssuer = true,
                                        ValidIssuer = config.AppName,
                                        ValidateAudience = true,
                                        ValidAudience = config.AppUrl,
                                        ValidateLifetime = true,
                                        IssuerSigningKey =
                                        new SymmetricSecurityKey(
                                            Encoding.ASCII.GetBytes(config.JwtTokenSecret)),
                                        ValidateIssuerSigningKey = true,
                                    };
                                });
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net("Claslog4net.config");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMvc();
        }
    }
}
