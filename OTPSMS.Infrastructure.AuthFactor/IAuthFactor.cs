using System;
using System.Threading.Tasks;

namespace OTPSMS.Infrastructure.AuthFactor
{
    public interface IAuthFactor
    {
        Task SendCode(string code);
    }
}
