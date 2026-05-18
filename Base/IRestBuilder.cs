using System.Threading.Tasks;
using RestSharp;

namespace GraphQL.NET_API_AutomationTestFramework.Base
{
    public interface IRestBuilder
    {
        RestRequest _restRequest { get; set; }
        IRestBuilder WithRequest(string request, Method method);
        IRestBuilder WithHeader(string header, string value);
        IRestBuilder WithUrlSegment(string name, int value);
        IRestBuilder WithUrlSegment(string name, string value);
        IRestBuilder WithQueryParameter(string name, string value);
        IRestBuilder WithBody(Object objectData);
        IRestBuilder WithFile(string name, string path);

        Task<T?> WithGet<T>();
        Task<RestResponse> WithGet();
        Task<T?> WithPost<T>();
        Task<RestResponse> WithPost();
        Task<T?> WithPut<T>();
        Task<T?> WithDelete<T>();
    }
}