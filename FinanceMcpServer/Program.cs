using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;

namespace FinanceMcpServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = Host.CreateEmptyApplicationBuilder(settings: null);

            builder.Services.AddMcpServer()
                .WithStdioServerTransport()
                .WithToolsFromAssembly();

            builder.Services.AddSingleton(_ =>
            {
                var client = new HttpClient() { BaseAddress = new Uri("http://localhost:5000") };
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("finance-tool", "1.0"));
                return client;
            });

            var app = builder.Build();

            await app.RunAsync();
        }
    }
}
