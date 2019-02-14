using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.Extensions;
using WebApp.WebCore.Authentication.JwtBearer;
using WebApp.WebCore.Configuration;
using WebApp.WebCore.Authentication.BaseManager;
using WebApp.Web.Host.Host.Startup;
using WebApp.Web.Host.Startup;
using System.IO;

//#if FEATURE_SIGNALR
//using Microsoft.AspNet.SignalR;
//using Microsoft.Owin.Cors;
//using Owin;
//using Abp.Owin;
//using zyGIS.Owin;
//#elif FEATURE_SIGNALR_ASPNETCORE
//using Abp.AspNetCore.SignalR.Hubs;
//#endif

namespace WebApp.Web.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC
            services.AddMvc(
                options => options.Filters.Add(new CorsAuthorizationFilterFactory(_defaultCorsPolicyName))
            );

            //IdentityRegistrar.Register(services);

            #region
            //start add by jjie   LogInManager  IUserClaimsPrincipalFactory
            ////services.AddLogging();
            ////services.AddScoped<LogInManager>();

            ////AbpIdentityBuilder identityBuilder = new AbpIdentityBuilder(services.AddIdentity<UserInfo, RoleInfo>(null), typeof(Tenant));
            ////var type = typeof(LogInManager);
            ////identityBuilder.Services.AddScoped<LogInManager>();
            ////identityBuilder.Services.AddScoped(typeof(LogInManager));

            //var identityBuilder = IdentityRegistrar.AddIdentity<UserInfo, RoleInfo>(services, options =>
            //{
            //    //options.Cookies.ApplicationCookie.AuthenticationScheme = "ApplicationCookie";
            //    //options.Cookies.ApplicationCookie.CookieName = "Interop";
            //});
            //identityBuilder.Services.AddScoped<LogInManager>();
            //identityBuilder.AddDefaultTokenProviders();

            services.AddScoped<LogInManager>();
            //end
            #endregion

            AuthConfigurer.Configure(services, _appConfiguration);

            //#if FEATURE_SIGNALR_ASPNETCORE
            //            services.AddSignalR();
            //#endif

            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                             //App: CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                )
            );

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "zyGIS API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                // Assign scope requirements to operations based on AuthorizeAttribute
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            // Configure Abp and Dependency Injection
            return services.AddAbp<WebAppWebModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(options => { options.UseAbpRequestLocalization = false; }); // Initializes ABP framework.

            //app.UseCors(); // Enable CORS!
            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            app.UseStaticFiles();

            app.UseAuthentication();//声明支持

            app.UseJwtTokenMiddleware(); //中间件

            app.UseAbpRequestLocalization();

            //app.UseContentRoot(Directory.GetCurrentDirectory())



#if FEATURE_SIGNALR
            // Integrate with OWIN
            app.UseAppBuilder(ConfigureOwinServices);
#elif FEATURE_SIGNALR_ASPNETCORE
            app.UseSignalR(routes =>
            {
                routes.MapHub<AbpCommonHub>("/signalr");
            });
#endif

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            //Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.InjectOnCompleteJavaScript("/swagger/ui/abp.js");
                options.InjectOnCompleteJavaScript("/swagger/ui/on-complete.js");
                options.SwaggerEndpoint("/webapp/swagger/v1/swagger.json", "zyGIS API V1");
            }); // URL: /swagger
        }

//#if FEATURE_SIGNALR
//        private static void ConfigureOwinServices(IAppBuilder app)
//        {
//            app.Properties["host.AppName"] = "zyGIS";

//            app.UseAbp();
            
//            app.Map("/signalr", map =>
//            {
//                map.UseCors(CorsOptions.AllowAll);
//                var hubConfiguration = new HubConfiguration
//                {
//                    EnableJSONP = true
//                };
//                map.RunSignalR(hubConfiguration);
//            });
//        }
//#endif
    }
}