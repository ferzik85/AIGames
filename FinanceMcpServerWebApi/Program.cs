using FinanceMcpServerWebApi.Resources;
using FinanceMcpServerWebApi.Tools;

namespace FinanceMcpServerWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddOpenApi();

            builder.Services.AddMcpServer()
                .WithHttpTransport()
                .WithTools<EchoTool>()
                .WithTools<FinanceTools>()
                .WithResources<SimpleResourceType>();

            builder.Services.AddHttpClient("FinanceApi", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseAuthorization();

            app.MapMcp();

            app.MapControllers();

            app.Run();
        }
    }
}
