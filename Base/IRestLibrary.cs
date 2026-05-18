using RestSharp;

namespace GraphQL.NET_API_AutomationTestFramework.Base
{
    public interface IRestLibrary
    {
        RestClient _restClient { get; }
    }
}