using RestSharp;

namespace GraphQL.NET_API_AutomationTestFramework.Base
{
    public class RestBuilder : IRestBuilder
    {
        private readonly IRestLibrary _restLibrary;

        public RestBuilder(IRestLibrary restLibrary)
        {
            this._restLibrary = restLibrary;
        }

        public RestRequest _restRequest { get; set; } = null!;

        public IRestBuilder WithBody(Object objectData)
        {
            _restRequest.AddJsonBody(objectData);
            return this;
        }
        public IRestBuilder WithFile(string name, string path)
        {
            _restRequest.AddFile(name, path);
            return this;
        }
        public IRestBuilder WithHeader(string header, string value)
        {
            _restRequest.AddHeader(header, value);
            return this;
        }
        public IRestBuilder WithQueryParameter(string name, string value)
        {
            _restRequest.AddQueryParameter(name, value);
            return this;
        }
        public IRestBuilder WithRequest(string request, Method method)
        {
            _restRequest = new RestRequest(request, method);
            return this;
        }
        public IRestBuilder WithUrlSegment(string name, int value)
        {
            _restRequest.AddUrlSegment(name, value);
            return this;
        }
        public IRestBuilder WithUrlSegment(string name, string value)
        {
            _restRequest.AddUrlSegment(name, value);
            return this;
        }

        public async Task<T?> WithGet<T>()
        {
            return await _restLibrary._restClient.GetAsync<T>(_restRequest);
        }
        public async Task<RestResponse> WithGet() {
            return await _restLibrary._restClient.GetAsync(_restRequest);
        }
        public async Task<T?> WithPost<T>()
        {
            return await _restLibrary._restClient.PostAsync<T>(_restRequest);
        }
        public async Task<RestResponse> WithPost() {
            return await _restLibrary._restClient.PostAsync(_restRequest);
        }
        public async Task<T?> WithPut<T>()
        {
            return await _restLibrary._restClient.PutAsync<T>(_restRequest);
        }
        public async Task<T?> WithDelete<T>()
        {
            return await _restLibrary._restClient.DeleteAsync<T>(_restRequest);
        }


    }
}
