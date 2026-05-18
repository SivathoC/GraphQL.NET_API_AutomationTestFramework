using FluentAssertions;
using GraphQL.NET_API_AutomationTestFramework.Base;
using GraphQL.NET_API_AutomationTestFramework.Models;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace GraphQL.NET_API_AutomationTestFramework
{
    public class AuthenticationTests
    {
        private readonly ITestOutputHelper testOutputHelper;
        private readonly IRestFactory _restFactory;

        public AuthenticationTests(
            ITestOutputHelper testOutputHelper,
            IRestFactory restFactory)
        {
            this.testOutputHelper = testOutputHelper;
            this._restFactory = restFactory;
        }
        [Fact]
        public async Task PostLogin()
        {
            var response = await _restFactory.Create()
                .WithRequest("/api/Authenticate/Login", Method.Post)
                .WithBody(new
                {
                    Username = "kk",
                    Password = "123456"
                })
                .WithPost();
            var responseContent = response.Content;
            var token = JObject.Parse(responseContent!)["token"]?.ToString();
            var request = new RestRequest("/api/Authenticate/Login", Method.Post);
        }
    }
}
