using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Session;
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

            IDocumentSession ravendb =
                RavenDbInitializer.Init(secret.RavenDbFilePath, secret.RavenDbServerUrl);

            services.AddTransient<IAuthFactor>(sp => {
                return new SMSAuthFactorService(secret.TwilioAccountId, secret.TwilioAuthToken);
            });

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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
