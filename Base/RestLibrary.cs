using RestSharp;

namespace GraphQL.NET_API_AutomationTestFramework.Base
{
    public class RestLibrary : IRestLibrary
    {
        private readonly RestClientOptions restClientOptions;

        public RestLibrary()
        {
            // - Define the Rest Client options, such as base URL, authentication, and other settings
            this.restClientOptions = new RestClientOptions()
            {
                BaseUrl = new Uri("https://localhost:5001/"),
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };

            // - Create a Rest Client instance using the defined options
            this._restClient = new RestClient(restClientOptions);
        }

        public RestClient _restClient { get; }
    }
}
