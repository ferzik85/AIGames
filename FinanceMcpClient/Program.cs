using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Client;
using System.ClientModel;

namespace FinanceMcpClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Configuration
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>();

            var clientTransport = new StdioClientTransport(new()
            {
                Name = "Finance Mcp Server",
                Command = "dotnet",
                Arguments = ["C:\\Users\\Sergey_Sidyakin\\source\\repos\\AIGames\\FinanceMcpServer\\bin\\debug\\net10.0\\FinanceMcpServer.dll"],
            });

            await using var mcpClient = await McpClient.CreateAsync(clientTransport);

            var tools = await mcpClient.ListToolsAsync();
            foreach (var tool in tools)
            {
                Console.WriteLine($"Connected to server with tools: {tool.Name}");
            }

            var aiModel = "gpt-4.1-mini";
            var githubToken = builder.Configuration["GITHUB_TOKEN"]!;
            IChatClient aiClient = new OpenAI.Chat.ChatClient(
                aiModel, 
                new ApiKeyCredential(githubToken),
                new OpenAI.OpenAIClientOptions()
                {
                    Endpoint= new Uri("https://models.github.ai/inference"),  
                })
                .AsIChatClient()
                .AsBuilder()
                .UseFunctionInvocation()
                .Build();

            var chatHistory = new List<ChatMessage>
            {
                new(ChatRole.System, "You are a helpful assistant with access to finance tools. ALWAYS check if any available tools can help answer the user's question before providing a response. Prefer using tools over your general knowledge when relevant tools are available.")
            };

            var options = new ChatOptions
            {
                MaxOutputTokens = 1000,
                ModelId = aiModel,
                Tools = [.. tools],
                ToolMode = ChatToolMode.RequireAny
            };

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("MCP Client Started!");
            Console.ResetColor();

            PromptForInput();
            while (Console.ReadLine() is string query && !"exit".Equals(query, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    PromptForInput();
                    continue;
                }

                chatHistory.Add(new ChatMessage(ChatRole.User, query));

                try
                {
                    await foreach (var message in aiClient.GetStreamingResponseAsync(query, options))
                    {
                        Console.Write(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine();

                PromptForInput();
            }

            static void PromptForInput()
            {
                Console.WriteLine("Enter a command (or 'exit' to quit):");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("> ");
                Console.ResetColor();
            }
        }
    }
}
