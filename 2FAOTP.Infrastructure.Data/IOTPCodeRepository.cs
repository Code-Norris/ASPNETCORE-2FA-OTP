using System;
using System.Threading.Tasks;

namespace TwoFAOTP.Infrastructure.Data
{
    public interface IOTPCodeRepository
    {
        void SaveSendCodeInfo
            (string uniqueUserName, string otpCode, string phoneNumber,
             int otpExpiryInSeconds, DateTime otpCodeGenTime);

        OTPCodeInfo GetOTPSentInfo
            (string uniqueUserName, string otpCode);

        void UpdateSendCodeInfo(OTPCodeInfo otpCodeInfo);
    }
}
