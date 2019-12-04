namespace TwoFAOTP.Common.Secret
{
    public class Secret
    {
        public string RavenDbFilePath { get; set; } = @"D:\home\site\wwwroot"; //AzAppSvc path

        public string RavenDbServerUrl { get; set; } = @"http://127.0.0.1:8080";

        public string TwilioAccountId { get; set; }

        public string TwilioAuthToken { get; set; }
    }
}