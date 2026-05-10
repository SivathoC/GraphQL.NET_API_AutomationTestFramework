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
            // * Arrange: Set up the necessary preconditions and inputs for the test

            // Define the Rest Client options, such as base URL, authentication, and other settings
            var restClientOptions = new RestClientOptions()
            {
                BaseUrl = new Uri("https://localhost:5001/"),
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };

            //Define the Rest Client with the base URL and other options 
            var client = new RestClient(restClientOptions);
            _testOutput.WriteLine("Setup: Resolving all the dependcies.");
            //Define the Rest Request with the endpoint and HTTP method
            var request = new RestRequest("Product/GetProductById/{id}", Method.Get);
            request.AddUrlSegment("id", 1);

            //* Act: Perform the action being tested, such as calling a method or executing a function
            var response = await client.GetAsync<Product>(request);
            _testOutput.WriteLine("Processing: Request Execution");

            // * Assert: Verify that the expected outcome has occurred, such as checking the return value or state of the system
            // Validate the request parameters and headers if any
            request.Should().NotBeNull();
            _testOutput.WriteLine("Validate: Request Input Is Valid.");

            //Validate the response headers if any, status code and content
            _testOutput.WriteLine("Validate: Response Output is Valid");
            response.Name?.Should().Be("Keyboard");
            response.Price.Should().Be(150);

            _testOutput.WriteLine("Complete: Execution Of Test Complete.");
        }
    }
}