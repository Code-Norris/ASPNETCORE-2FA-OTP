using System.Threading.Tasks;

namespace OTPSMS.Infrastructure.AuthFactor
{
    public class SMSAuthFactorService : IAuthFactor
    {
        public Task SendCode(string code)
        {
            throw new System.NotImplementedException();
        }
    }
}