using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace OTPSMS.Infrastructure.AuthFactor
{
    public class SMSAuthFactorService : IAuthFactor
    {
        public SMSAuthFactorService(string twilioAccountId, string twilioAuthToken)
        {
            _twilioAccountId = twilioAccountId;
            _twilioAuthToken = twilioAuthToken;
        }

        public void SendCode(string smsMessage, string recipientPhoneNumber)
        {
            TwilioClient.Init(_twilioAccountId, _twilioAuthToken);

            var message = MessageResource.Create(
                body: smsMessage,
                from: new Twilio.Types.PhoneNumber("+15017122661"),
                to: new Twilio.Types.PhoneNumber(recipientPhoneNumber)
            );
        }

        private string _twilioAccountId;
        private string _twilioAuthToken;
    }
}