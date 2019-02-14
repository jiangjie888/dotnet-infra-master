using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Abp.Runtime.Security;
using WebApp.Application;

namespace WebApp.Web.Host.Host.Startup
{
    public static class AuthConfigurer
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            if (bool.Parse(configuration["Authentication:JwtBearer:IsEnabled"]))
            {
                services.AddAuthentication()
                    .AddJwtBearer(options =>
                    {
                        options.Audience = configuration["Authentication:JwtBearer:Audience"];
                        //不使用https
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {

                            // The signing key must match!
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"])),

                            // Validate the JWT Issuer (iss) claim
                            ValidateIssuer = true,
                            ValidIssuer = configuration["Authentication:JwtBearer:Issuer"],

                            // Validate the JWT Audience (aud) claim
                            ValidateAudience = true,
                            ValidAudience = configuration["Authentication:JwtBearer:Audience"],

                            // Validate the token expiry
                            ValidateLifetime = true,

                            // If you want to allow a certain amount of clock drift, set that here
                            ClockSkew = TimeSpan.Zero
                        };

                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = QueryStringTokenResolver
                        };
                    });
            }
        }

        /* This method is needed to authorize SignalR javascript client.t from query string as an encrypted text. 
         * SignalR can not send authorization header. So, we are getting i*/
        private static Task QueryStringTokenResolver(MessageReceivedContext context)
        {
            //var token = context.Request.Headers["mytoken"];
            //context.Token = token.FirstOrDefault();
            //return Task.CompletedTask;


            if (!context.HttpContext.Request.Path.HasValue ||
                !context.HttpContext.Request.Path.Value.StartsWith("/signalr"))
            {
                // We are just looking for signalr clients
                return Task.CompletedTask;
            }

            var qsAuthToken = context.HttpContext.Request.Query["enc_auth_token"].FirstOrDefault();
            if (qsAuthToken == null)
            {
                // Cookie value does not matches to querystring value
                return Task.CompletedTask;
            }

            // Set auth token from cookie
            context.Token = SimpleStringCipher.Instance.Decrypt(qsAuthToken, AppConsts.DefaultPassPhrase);
            return Task.CompletedTask;
        }
    }
}
