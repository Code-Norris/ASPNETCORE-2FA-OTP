using System;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;

namespace TwoFAOTP.Infrastructure.Data
{
    public class OTPCodeRepository : IOTPCodeRepository
    {
        public OTPCodeRepository(IDocumentSession ravendb)
        {
            _ravendb = ravendb;
        }

        public void SaveSendCodeInfo
            (string uniqueUserName, string otpCode, string phoneNumber,
             int otpExpiryInSeconds, DateTime otpCodeGenTime)
        {
           _ravendb.Store(new OTPCodeInfo(){
               uniqueUserName = uniqueUserName,
               OTPCode = otpCode,
               phoneNumber = phoneNumber,
               OTPExpiryInSeconds = otpExpiryInSeconds,
               OTPCodeGenTime = otpCodeGenTime
           });
           _ravendb.SaveChanges();
        }

        public OTPCodeInfo GetOTPSentInfo
            (string uniqueUserName, string otpCode)
        {
             var otpInfo = _ravendb
                .Query<OTPCodeInfo>()
                .SingleOrDefault(o => o.uniqueUserName == uniqueUserName &&
                        o.OTPCode == otpCode && o.Verified == false);
        
            return otpInfo;
        }

        public void UpdateSendCodeInfo(OTPCodeInfo otpCodeInfo)
        {
            var otpCodeInfoToUpdate =
                _ravendb.Load<OTPCodeInfo>(
                GetId(otpCodeInfo.uniqueUserName, otpCodeInfo.OTPCode, otpCodeInfo.phoneNumber));
            
            otpCodeInfoToUpdate.Verified = true;
            otpCodeInfoToUpdate.CodeVerifiedAt = otpCodeInfo.CodeVerifiedAt;
        }

        private string GetId(string uniqueUserName, string otpCode, string recipientPhoneNumber)
        {
            return uniqueUserName + ":" + otpCode + ":" + recipientPhoneNumber ;
        }

        private IDocumentSession _ravendb;
    }
}