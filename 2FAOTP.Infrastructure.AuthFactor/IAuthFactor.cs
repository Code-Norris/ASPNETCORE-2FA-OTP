using System;
using System.Threading.Tasks;

namespace OTPSMS.Infrastructure.AuthFactor
{
    public interface IAuthFactor
    {
        void SendCode(string smsMessage, string recipientPhoneNumber);
    }
}
