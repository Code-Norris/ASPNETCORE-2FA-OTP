using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace TwoFAOTP.Common.Secret
{
    public class SecretManager
    {
        //default gets from ASP.NET Core SecretManager, fallback to environment variables
        public static Secret GetSecret()
        {
            Secret secret;

            if(!GetSecretFromDotnetCoreUserSecret(out secret))
            {
                if(!GetSecretFromEnvironmentVariables(out secret))
                    throw new ArgumentException("Cannot find Twilio ACCOUNT SID and AUTH TOKEN in NetCore UserSecrets and Environment Variables");
            }

            return secret;
        }

        private static bool GetSecretFromDotnetCoreUserSecret(out Secret secret)
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Secret>();
            IConfigurationRoot Configuration = builder.Build();
            
            
            if(Configuration.AsEnumerable().Count() == 0)
            {
                secret = null;
                return false;
            }
            else
            {
                secret = new Secret()
                {
                    TwilioAccountId = Configuration["TwilioAccountId"],
                    TwilioAuthToken = Configuration["TwilioAuthToken"]
                };
                return true;
            }
        }

        private static bool GetSecretFromEnvironmentVariables(out Secret secret)
        {
            string twilioAcctId = Environment.GetEnvironmentVariable("TwilioAccountId");
            string twilioAuthToken = Environment.GetEnvironmentVariable("TwilioAuthToken");
            
            if(!string.IsNullOrEmpty(twilioAcctId) && !string.IsNullOrEmpty(twilioAuthToken))
            {
                secret = new Secret()
                {
                    TwilioAccountId = twilioAcctId,
                    TwilioAuthToken = twilioAuthToken
                };

                return true;
            }
            else
            {
                secret = null;
                return false;
            }
        }
    }
}