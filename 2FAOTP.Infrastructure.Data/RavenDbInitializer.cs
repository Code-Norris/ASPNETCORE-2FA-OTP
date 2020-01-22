using Raven.Client.Documents.Session;
using Raven.Embedded;

namespace TwoFAOTP.Infrastructure.Data
{
    public class RavenDbInitializer
    {
        public static IDocumentSession Init(string dataDirection, string serverUrl)
        {
             EmbeddedServer.Instance.StartServer();
            using (var store = EmbeddedServer.Instance.GetDocumentStore("otpcodegeninfo"))
            {
                return store.OpenSession();
            }
        }
    }
}