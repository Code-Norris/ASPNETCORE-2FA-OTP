namespace TwoFAOTP.Web.API
{
    public class OTPInfo
    {
        public string UniqueUserName { get; set; }
        public string Message { get; set; }
        public string RecipientPhoneNumber { get; set; }
        public string FromPhoneNumber { get; set; }
        public int OTPExpiryInSeconds { get; set; }

    }
}