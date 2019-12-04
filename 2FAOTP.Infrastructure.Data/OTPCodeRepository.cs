using System;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;

namespace TwoFAOTP.Infrastructure.Data
{
    public class OTPCodeRepository : IOTPCodeRepository
    {
        public OTPCodeRepository(string dbDirectory, string dbServerUrl)
        {
            _dbDirectory = dbDirectory;
            _dbServerUrl = dbServerUrl;
        }

        public void SaveSendCodeInfo
            (string uniqueUserName, string otpCode, string phoneNumber,
             int otpExpiryInSeconds, DateTime otpCodeGenTime)
        {
           IDocumentSession ravendb = RavenDbInitializer.Init(_dbDirectory, _dbServerUrl);

           ravendb.Store(new OTPCodeInfo(){
               uniqueUserName = uniqueUserName,
               OTPCode = otpCode,
               phoneNumber = phoneNumber,
               OTPExpiryInSeconds = otpExpiryInSeconds,
               OTPCodeGenTime = otpCodeGenTime
           });
           ravendb.SaveChanges();
        }

        private string _dbDirectory;
        private string _dbServerUrl;
    }
}