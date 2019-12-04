using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TwoFAOTP.Infrastructure.AuthFactor;
using TwoFAOTP.Infrastructure.Data;

namespace TwoFAOTP.Web.API
{
    [ApiController]
    [Route("api/otp/sms")]
    public class OTPController : ControllerBase
    {
        public OTPController(IAuthFactor authFactor, IOTPCodeRepository otpCodeRepo)
        {
            _authFactor = authFactor;
            _otpCodeRepo = otpCodeRepo;
        }

        //Message is a template, %optcode% will be replaced with OTPCode
        [HttpPost("send")]
        public void Send([FromBody]OTPInfo otpInfo)
        {
            string optCode = GenerateOTP();

            string msgToSMS = otpInfo.Message.Replace("%otpcode%", optCode);

            if(_authFactor.SendCode(msgToSMS, otpInfo.RecipientPhoneNumber, otpInfo.FromPhoneNumber))
            {
                DateTime timeOTPCodeGen = DateTime.Now;

                _otpCodeRepo.SaveSendCodeInfo
                    (otpInfo.UniqueUserName, optCode,
                     otpInfo.RecipientPhoneNumber, otpInfo.OTPExpiryInSeconds, timeOTPCodeGen);
            }
            else
            {
                //TODO: log error
            }
        }

        [HttpGet("verify")]
        public bool Verify(string uniqueUserName, string otpCode, string recipientPhoneNumber)
        {
            var optCodeInfo = _otpCodeRepo.GetOTPSentInfo(uniqueUserName, otpCode, recipientPhoneNumber);

            if(optCodeInfo == null)
            {
                //TODO: Log error
                return false;
            }
            else
            {
                if(!IsOTPCodeExpired(optCodeInfo))
                {
                    optCodeInfo.Verified = true;
                    optCodeInfo.CodeVerifiedAt = DateTime.Now;
                    _otpCodeRepo.UpdateSendCodeInfo(optCodeInfo);
                    return true;
                }
                else
                    return false;
            }

        }

        private bool IsOTPCodeExpired(OTPCodeInfo codeInfo)
        {
           DateTime codeGenTime = codeInfo.OTPCodeGenTime;
           int tokenExpiryInSec = codeInfo.OTPExpiryInSeconds;

           if((DateTime.Now - codeGenTime).TotalSeconds <= tokenExpiryInSec)
                return true;
            else
                return false;
        }

        private string GenerateOTP()
        {
            var generator = new Random();
            String code = generator.Next(0, 999999).ToString("D6");
            return code;
        }

        private IAuthFactor _authFactor;
        private IOTPCodeRepository _otpCodeRepo;
    }
}
