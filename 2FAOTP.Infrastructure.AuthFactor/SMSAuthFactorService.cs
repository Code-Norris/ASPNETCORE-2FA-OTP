using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace TwoFAOTP.Infrastructure.AuthFactor
{
    public class SMSAuthFactorService : IAuthFactor
    {
        public SMSAuthFactorService(string twilioAccountId, string twilioAuthToken)
        {
            _twilioAccountId = twilioAccountId;
            _twilioAuthToken = twilioAuthToken;
        }

        public bool SendCode
            (string smsMessage, string recipientPhoneNumber, string fromPhoneNumber)
        {
            try
            {
                return true;

                // TwilioClient.Init(_twilioAccountId, _twilioAuthToken);

                // var message = MessageResource.Create(
                //     body: smsMessage,
                //     from: new Twilio.Types.PhoneNumber(fromPhoneNumber),
                //     to: new Twilio.Types.PhoneNumber(recipientPhoneNumber)
                // );

                // return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private string _twilioAccountId;
        private string _twilioAuthToken;
    }
}