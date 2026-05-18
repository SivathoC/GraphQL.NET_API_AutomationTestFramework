using GraphQL.NET_API_AutomationTestFramework.Base;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.NET_API_AutomationTestFramework
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<IRestLibrary, RestLibrary>()
                .AddScoped<IRestBuilder, RestBuilder>()
                .AddScoped<IRestFactory, RestFactory>();
        }
    }
}
