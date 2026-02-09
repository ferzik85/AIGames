using ModelContextProtocol.Server;
using System.ComponentModel;

namespace FinanceMcpServer
{
    [McpServerToolType]
    public static class FinanceTools
    {
        [McpServerTool, Description("Get hardcoded exchange rates from Finance API")]
        public static async Task<string> GetExchangeRates(HttpClient client)
        {
            using var jsonDocument = await client.ReadJsonDocumentAsync($"/ExchangeRate");
            var jsonElement = jsonDocument.RootElement;
            var rates = jsonElement.EnumerateArray();

            if (!rates.Any())
            {
                return "No rates were found.";
            }

            return string.Join("\n--\n", rates.Select(rate =>
            {
                return $"""
                    Currency1: {rate.GetProperty("currency1").GetString()}
                    Currency2: {rate.GetProperty("currency2").GetString()}
                    Rate: {rate.GetProperty("rate").GetDouble()}
                    DateTime: {rate.GetProperty("dateTime").GetString()}
                    """;
            }));
        }  
    }
}