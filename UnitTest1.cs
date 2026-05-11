using FluentAssertions;
using GraphQL.NET_API_AutomationTestFramework.Models;
using RestSharp;
using Xunit.Abstractions;

namespace GraphQL.NET_API_AutomationTestFramework
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _testOutput;

        public UnitTest1(ITestOutputHelper testOutput)
        {
            this._testOutput = testOutput;
        }
        [Fact]
        public async Task GetWithQueryURLSegmentTest()
        {
            //Arrange: Set up the necessary preconditions and inputs for the test

            // - Define the Rest Client options, such as base URL, authentication, and other settings
            var restClientOptions = new RestClientOptions()
            {
                BaseUrl = new Uri("https://localhost:5001/"),
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };

            // - Define the Rest Client with the base URL and other options 
            var client = new RestClient(restClientOptions);
            _testOutput.WriteLine("Setup: Resolving all the dependcies.");
            // - Define the Rest Request with the endpoint and HTTP method
            var request = new RestRequest("Product/GetProductById/{id}", Method.Get);
            request.AddUrlSegment("id", 1);

            //Act: Perform the action being tested, such as calling a method or executing a function
            var response = await client.GetAsync<Product>(request);
            _testOutput.WriteLine("Processing: Request Execution");

            //Assert: Verify that the expected outcome has occurred, such as checking the return value or state of the system
            // - Validate the request parameters and headers if any
            request.Should().NotBeNull();
            _testOutput.WriteLine($"Validate: Request Input Is Valid. " +
                $"\n\t Method: {request.Method} " +
                $"\n\t Endpoint: {request.Resource}" +
                $"\n\t {request.Parameters.First().Name}: {request.Parameters.First().Value}");

            // - Validate the response headers if any, status code and content
            _testOutput.WriteLine($"Validate: Response Output is Valid. " +
                $"\n\t Name: {response.Name}" +
                $"\n\t price: {response.Price}");
            response.Name?.Should().Be("Keyboard");
            response.Price.Should().Be(150);

            _testOutput.WriteLine("Complete: Execution Of Test Complete.");
        }
        [Fact]
        public async Task GetWithQueryParamterTest()
        {
            var restClientOptions = new RestClientOptions()
            {
                BaseUrl = new Uri("https://localhost:5001/"),
                RemoteCertificateValidationCallback = (_, _, _, _) => true
            };
            var restClient = new RestClient(restClientOptions);
            var request = new RestRequest("Product/GetProductByIdAndName", Method.Get);
            request.AddQueryParameter("id", 1);
            request.AddQueryParameter("name", "Keyboard");

            var response = await restClient.GetAsync<Product>(request);
            _testOutput.WriteLine($"Validate: Request Input Is Valid. " +
                $"\n\t Method: {request.Method} " +
                $"\n\t Endpoint: {request.Resource} " +
                $"\nQuery Paramter:");

            foreach (var product in request.Parameters)
            {
                _testOutput.WriteLine($"\t Field: {product.Name}, Value: {product.Value}");
            }
            // - Validate the response headers if any, status code and content
            _testOutput.WriteLine($"Validate: Response Output is Valid. " +
                $"\n\t Name: {response.Name} " +
                $"\n\t price: {response.Price}");
            response.Name?.Should().Be("Keyboard");
            response.Price.Should().Be(150);
        }

        [Fact]
        public async Task PostProductWithJsonBodyTest()
        {
            var restClientOptions = new RestClientOptions()
            {
                BaseUrl = new Uri("https://localhost:5001/"),
                RemoteCertificateValidationCallback = (_, _, _, _) => true
            };
            var restClient = new RestClient(restClientOptions);
            var request = new RestRequest("/Product/Create", Method.Post);
            var product = new Product()
            {
                Name = "Printer",
                Description = "Colour Printer",
                Price = 499,
                ProductType = ProductType.PERIPHARALS
            };
            request.AddJsonBody(product);

            var response = await restClient.PostAsync<Product>(request);

            response.Name?.Should().Be("Printer");
            response.Description?.Should().Be("Colour Printer");
            response.Price.Should().Be(499);


            var requestBody = request.Parameters
                .FirstOrDefault(p => p.Type == ParameterType.RequestBody)
                ?.Value as Product;

            _testOutput.WriteLine($"Validate: Request Input Is Valid. " +
                $"\n\t Request Method: {request.Method} " +
                $"\n\t Endpoint: {request.Resource} " +
                $"\n\t Body: " +
                $"\n\t\t  ID: {requestBody.ProductId}" +
                $"\n\t\t  Name: {requestBody.Name}" +
                $"\n\t\t  Description: {requestBody.Description}" +
                $"\n\t\t  Price: {requestBody.Price}");

            _testOutput.WriteLine($"Validate: Response Input Is Valid. " +
            $"\n\t Response Body: " +
            $"\n\t\t  ID: {response.ProductId}" +
            $"\n\t\t  Name: {response.Name}" +
            $"\n\t\t  Description: {response.Description}" +
            $"\n\t\t  Price: {response.Price}" +
            $"\n\t\t  Product Type: {response.ProductType}");
        }

    }
}