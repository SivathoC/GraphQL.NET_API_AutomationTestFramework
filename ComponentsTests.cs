using FluentAssertions;
using GraphQL.NET_API_AutomationTestFramework.Base;
using GraphQL.NET_API_AutomationTestFramework.Models;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace GraphQL.NET_API_AutomationTestFramework
{
    public class ComponentsTests
    {
        private readonly ITestOutputHelper testOutputHelper;
        private RestClient _restClient;

        public ComponentsTests(ITestOutputHelper testOutputHelper,
                               IRestLibrary restLibrary)
        {
            this.testOutputHelper = testOutputHelper;
            this._restClient = restLibrary._restClient;
        }

        [Fact]
        public async Task GetComponentByProductId() {
            var productId = 1;
            var request = new RestRequest("/Components/GetComponentByProductId/{id}", Method.Get);
            request.AddUrlSegment("id", productId);

            var response = await _restClient.GetAsync<List<Components>>(request);
            response?.Count.Should().BeGreaterThan(0);
            foreach (var component in response!)
            {
                testOutputHelper.WriteLine($"GetComponentByProductId: {productId}" +
                    $"\nComponent ID: {component.Id}," +
                    $"\n\tName: {component.Name}, " +
                    $"\n\tDescription: {component.Description}, " +
                    $"\n\tProduct ID: {component.ProductId}");
            }
        }

        [Fact]
        public async Task GetComponentsByProductId() {
            var productId = 1;
            var request = new RestRequest("/Components/GetComponentsByProductId/{id}", Method.Get);
            request.AddUrlSegment("id", productId);

            var response = await _restClient.GetAsync<List<Components>>(request);
            response?.Count.Should().BeGreaterThan(0);
            foreach (var component in response!)
            {
                testOutputHelper.WriteLine($"GetComponentsByProductId: {productId}" +
                    $"\nComponent ID: {component.Id}, " +
                    $"\n\tName: {component.Name}, " +
                    $"\n\tDescription: {component.Description}, " +
                    $"\n\tProduct ID: {component.ProductId}");
            }
        }

        [Fact]
        public async Task GetAllComponents() {
            var request = new RestRequest("/Components/GetAllComponents", Method.Get);
            var response = await _restClient.GetAsync<List<Components>>(request);
            response?.Count.Should().BeGreaterThan(0);
            foreach (var component in response!)
            {
                testOutputHelper.WriteLine($"GetAllComponents: " +
                    $"\nComponent ID: {component.Id}, " +
                    $"\n\tName: {component.Name}, " +
                    $"\n\tDescription: {component.Description}, " +
                    $"\n\tProduct ID: {component.ProductId}");
            }
        }

        [Fact]
        public async Task PostCreateComponent() {
            var requestQueryParam = new Components() {
                Name = "Test Component",
                Description = "This is a test component.",
                ProductId = 1,
            };
            var request = new RestRequest("/Components/CreateComponent", Method.Post);
            request.AddJsonBody(requestQueryParam);
            var response = await _restClient.PostAsync(request);
            response.Should().NotBeNull();

            var responseObject =  JObject.Parse(response.Content);
            var responseComponent = new Components() {
                    Id = (int)responseObject["id"]!,
                    Name = (string)responseObject["name"]!,
                    Description = (string)responseObject["description"]!,
                    ProductId = (int)responseObject["productId"]!,
                };
            testOutputHelper.WriteLine($"PostCreateComponent: " +
                        $"\nComponent ID: {responseComponent.Id}, " +
                        $"\n\tName: {responseComponent.Name}, " +
                        $"\n\tDescription: {responseComponent.Description}, " +
                        $"\n\tProduct ID: {responseComponent.ProductId}");
        }
    }
}
