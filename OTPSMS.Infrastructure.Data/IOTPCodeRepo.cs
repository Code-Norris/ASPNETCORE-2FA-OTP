using System;
using System.Threading.Tasks;

namespace OTPSMS.Infrastructure.Data
{
    public interface IOTPCodeRepo
    {
        Task SaveSendCodeInfo(string otpCode, string phoneNumber, DateTime otpExpiry);
    }
}
