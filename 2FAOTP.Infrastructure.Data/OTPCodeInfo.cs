using System;

namespace TwoFAOTP.Infrastructure.Data
{
    public class OTPCodeInfo
    {
        public string Id { get {return uniqueUserName + ":" + OTPCode + ":" + phoneNumber ;}}
        public string uniqueUserName { get; set; }
        public string OTPCode { get; set; }
        public string phoneNumber { get; set; }
        public int OTPExpiryInSeconds { get; set; }
        public DateTime OTPCodeGenTime { get; set; }
        public bool Verified { get; set; }
        public DateTime CodeVerifiedAt { get; set; }
        public string CodeVerificationFailedReason { get; set; }
    }
}