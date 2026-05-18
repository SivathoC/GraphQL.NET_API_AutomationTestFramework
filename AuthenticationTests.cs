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
        private RestClient restClient;

        public AuthenticationTests(ITestOutputHelper testOutputHelper, IRestLibrary restLibrary)
        {
            this.testOutputHelper = testOutputHelper;
            this.restClient = restLibrary._restClient;
        }
        [Fact]
        public async Task PostLogin()
        {
            var request = new RestRequest("/api/Authenticate/Login", Method.Post);
            // Assuming the API expects an anonymous object as JSON body
            var requestBody = new
            {
                Username = "kk",
                Password = "123456"
            };
            request.AddJsonBody(requestBody);
            var responseObject = restClient.PostAsync(request).Result.Content;
            var token = JObject.Parse(responseObject!)["token"]?.ToString();
            //response.IsSuccessful.Should().BeTrue();
            //var responseContent = response.Content;

            testOutputHelper.WriteLine($"PostLogin Response " +
                $"\n\tToken: {token}");

            request = new RestRequest("/Product/GetProducts", Method.Get);
            request.AddHeader("Authorization", $"Bearer {GetToken()}");

            var response = await restClient.GetAsync<List<Product>>(request);
            testOutputHelper.WriteLine($"Validate: Request Input Is Valid. " +
                                    $"\n\t Method: {request.Method} " +
                                    $"\n\t Endpoint: {request.Resource}" +
                                    $"\n\t Header :  {request.Parameters.FirstOrDefault()!.Value}");
            testOutputHelper.WriteLine($"Validate: Response Output is Valid. " +
                                    $"\n\t Total Products: {response!.Count}");
            foreach (var product in response)
            {
                testOutputHelper.WriteLine($"\n\t Product ID: {product.ProductId}" +
                                    $"\n\t Name: {product.Name}" +
                                    $"\n\t Price: {product.Price}" +
                                    $"\n\t Components: {string.Join(", ", 
                                                        product.Components.Select(c => c.Name))}" +
                                    $"\n\t Product Type: {product.ProductType}");
            }
            response.Count.Should().BeGreaterThan(0);
            foreach (var product in response)
            {
                product.Name.Should().NotBeNullOrEmpty();
                product.Price.Should().BeGreaterThan(0);
            }
        }

        private string GetToken()
        {
            var authRequest = new RestRequest("/api/Authenticate/Login", Method.Post);
            var requestBody = new {
                UserName = "kk",
                Password = "123456"
            };
            
            var authResponse = restClient.PostAsync(authRequest.AddJsonBody(requestBody)).Result.Content;

            return JObject.Parse(authResponse!)["token"]?.ToString() ?? string.Empty;
        }
    }
}
