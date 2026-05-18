using System.Net;
using FluentAssertions;
using GraphQL.NET_API_AutomationTestFramework.Base;
using GraphQL.NET_API_AutomationTestFramework.Models;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace GraphQL.NET_API_AutomationTestFramework
{
    public class ProductTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IRestFactory _restFactory;

        public ProductTests(
            ITestOutputHelper testOutputHelper,
            IRestFactory restFactory)
        {
            //Arrange: Set up the necessary preconditions and inputs for the test
            this._testOutputHelper = testOutputHelper;
            this._restFactory = restFactory;
        }

        [Fact]
        public async Task GetProductByIdTest()
        {
            var token = GetToken();
            var response = await _restFactory
                .Create()
                .WithRequest("Product/GetProductById/{id}", Method.Get)
                .WithHeader("Authorization", $"Bearer {token}")
                .WithUrlSegment("id", 1)
                .WithGet<Product>();
            
            _testOutputHelper.WriteLine($"Validate: Response Output is Valid. " +
                $"\n\t Name: {response.Name}" +
                $"\n\t price: {response.Price}");
            
            response.Name?.Should().Be("Keyboard");
            response.Price.Should().Be(150);

            _testOutputHelper.WriteLine("Complete: Execution Of Test Complete.");
        }
        [Fact]
        public async Task GetProductByIdAndNameTest()
        {
            var token = GetToken();
            var response = await _restFactory.Create()
                   .WithRequest("Product/GetProductByIdAndName", Method.Get)
                   .WithHeader("Authorization", $"Bearer {token}")
                   .WithQueryParameter("id", "1")
                   .WithQueryParameter("name", "Keyboard")
                   .WithGet<Product>();

            // - Validate the response headers if any, status code and content
            _testOutputHelper.WriteLine($"Validate: Response Output is Valid. " +
                $"\n\t Name: {response.Name} " +
                $"\n\t price: {response.Price}");
            response.Name?.Should().Be("Keyboard");
            response.Price.Should().Be(150);
        }
        [Fact]
        public async Task GetProductByNameTest()
        {
            var token = GetToken();
            var response = await _restFactory
                .Create()
                .WithRequest("/Product/GetProductByName/{name}", Method.Get)
                .WithHeader("Authorization", $"Bearer {token}")
                .WithUrlSegment("name", "Keyboard")
                .WithGet<Product>();

            _testOutputHelper.WriteLine($"Validate: Response Output is Valid. " +
                $"\n\t Name: {response.Name} " +
                $"\n\t price: {response.Price}" +
                $"\n\t Description: {response.Description}");
            
            response.Name?.Should().Be("Keyboard");
        }
        [Fact]
        public async Task GetAllProductsTest()
        {
            var token = GetToken();
            var response = await _restFactory
                .Create()
                .WithRequest("/Product/GetProducts", Method.Get)
                .WithHeader("Authorization", $"Bearer {token}")
                .WithGet<List<Product>>();

            _testOutputHelper.WriteLine($"Validate: Response Output is Valid. " +
                $"\n\t Total Products: {response!.Count}");
            foreach (var product in response)
            {
                _testOutputHelper.WriteLine($"\n\t Product ID: {product.ProductId}" +
                    $"\n\t Name: {product.Name}" +
                    $"\n\t Price: {product.Price}" +
                    $"\n\t Components: {string.Join(", ", product.Components.Select(c => c.Name))}" +
                    $"\n\t Product Type: {product.ProductType}");
            }
            
            response.Count.Should().BeGreaterThan(0);
            foreach (var product in response)
            {
                product.Name.Should().NotBeNullOrEmpty();
                product.Price.Should().BeGreaterThan(0);
            }
        }
        [Fact]
        public async Task GetProductFileNameTest()
        {
            var token = GetToken();
            var response = await _restFactory.Create()
                   .WithRequest("/Product/{filename}", Method.Get)
                   .WithHeader("Authorization", $"Bearer {token}")
                   .WithUrlSegment("filename", "Automation Testing Process Diagram.png")
                   .WithGet();
            _testOutputHelper.WriteLine($"Validate: Response Output is Valid. " +
               $"\n\t Content Length: {response!.Content!.Length} bytes");
        }
        [Fact]
        public async Task PostProductCreateTest()
        {
            var token = GetToken();
            var response = await _restFactory.Create()
                   .WithRequest("/Product/Create", Method.Post)
                   .WithHeader("Authorization", $"Bearer {token}")
                   .WithBody(new Product()
                   {
                       Name = "Printer",
                       Description = "Colour Printer",
                       Price = 499,
                       ProductType = ProductType.PERIPHARALS
                   })
                   .WithPost<Product>();
            response!.Name?.Should().Be("Printer");
            response.Description?.Should().Be("Colour Printer");
            response.Price.Should().Be(499);
            _testOutputHelper.WriteLine($"Validate: Response Input Is Valid. " +
            $"\n\t Response Body: " +
            $"\n\t\t  ID: {response.ProductId}" +
            $"\n\t\t  Name: {response.Name}" +
            $"\n\t\t  Description: {response.Description}" +
            $"\n\t\t  Price: {response.Price}" +
            $"\n\t\t  Product Type: {response.ProductType}");
        }
        [Fact]
        public async Task PostProductFileUploadTest()
        {
            var token = GetToken();
            var response = await _restFactory.Create()
                   .WithRequest("/Product", Method.Post)
                   .WithHeader("Authorization", $"Bearer {token}")
                   .WithFile("myFile", @"C:/Users/SConjwa/OneDrive - Clientele Limited/Pictures/Automation Testing Process Diagram.png")
                   .WithPost();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        private string GetToken()
        {
            var authResponse = _restFactory
                .Create()
                .WithRequest("/api/Authenticate/Login", Method.Post)
                .WithBody(new
                   {
                       UserName = "kk",
                       Password = "123456"
                   })
                .WithPost().Result.Content;
            return JObject.Parse(authResponse!)["token"]?.ToString() ?? string.Empty;
        }
    }
}