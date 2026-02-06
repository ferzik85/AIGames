using System;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        // fake rates for demonstration purposes
        private readonly ExchangeRate PlnRubRate = new ExchangeRate("PLN", "RUB", 21.69, DateTime.UtcNow);
        private readonly ExchangeRate UsdRubRate = new ExchangeRate("USD", "RUB", 93.12, DateTime.UtcNow);
        private readonly ExchangeRate EurRubRate = new ExchangeRate("EUR", "RUB", 100.45, DateTime.UtcNow);

        [HttpGet(Name = "GetExchangeRates")]
        public IEnumerable<ExchangeRate> GetExchangeRates()
        {
            return [PlnRubRate, UsdRubRate, EurRubRate];
        }   
    }
}
