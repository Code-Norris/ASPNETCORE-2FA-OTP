using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Raven.Client.Documents.Session;
using Serilog;
using Serilog.Core;
using TwoFAOTP.Common.Secret;
using TwoFAOTP.Infrastructure.AuthFactor;
using TwoFAOTP.Infrastructure.Data;

namespace TwoFAOTP.Web.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Secret secret = SecretManager.GetSecret();

            _logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("2falog.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            IDocumentSession ravendb =
                RavenDbInitializer.Init(secret.RavenDbFilePath, secret.RavenDbServerUrl);

            services.AddTransient<IAuthFactor>(sp => {
                return new SMSAuthFactorService(secret.TwilioAccountId, secret.TwilioAuthToken);
            });

            services.AddSingleton(typeof(Logger), _logger);

            services.AddSingleton(typeof(IDocumentSession), sp => {return ravendb; });

            services.AddSingleton<IOTPCodeRepository>(sp => {
                return new OTPCodeRepository(ravendb);
            });
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
 
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    { 
                        _logger.Error(contextFeature.Error.ToString());

                        await context.Response.WriteAsync(new JObject()
                        {
                            {"StatusCode", context.Response.StatusCode},
                            {"Message", "Internal Server Error."}
                        }.ToString());
                    }
                });
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private Logger _logger;
    }
}
