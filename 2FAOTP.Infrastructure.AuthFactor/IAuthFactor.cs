using System;
using System.Threading.Tasks;

namespace TwoFAOTP.Infrastructure.AuthFactor
{
    public interface IAuthFactor
    {
        bool SendCode(string smsMessage, string recipientPhoneNumber, string fromPhoneNumber);
    }
}
