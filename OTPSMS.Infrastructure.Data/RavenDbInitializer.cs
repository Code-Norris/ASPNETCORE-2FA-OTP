using Raven.Embedded;

namespace OTPSMS.Infrastructure.Data
{
    public class RavenDbInitializer
    {
        public static void Init()
        {
            EmbeddedServer.Instance.StartServer();
            
            using (var store = EmbeddedServer.Instance.GetDocumentStore("otpcode"))
            {
                using (var session = store.OpenSession())
                {
                    // Your code here
                }
            }
        }
    }
}