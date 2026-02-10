using FinanceMcpServerWebApi.Extensions;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace FinanceMcpServerWebApi.Tools;

[McpServerToolType]
public sealed class FinanceTools
{
    private readonly IHttpClientFactory _httpClientFactory;

    public FinanceTools(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [McpServerTool, Description("Get hardcoded exchange rates from Finance API")]
    [McpMeta("category", "sersid")]
    public async Task<string> GetExchangeRates()
    {
        var client = _httpClientFactory.CreateClient("FinanceApi");
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
