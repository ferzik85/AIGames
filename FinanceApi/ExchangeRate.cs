namespace FinanceApi
{
    public class ExchangeRate
    {
        public string Currency1 { get; }
        public string Currency2 { get; }
        public double Rate { get; }
        public DateTime DateTime { get; }

        public ExchangeRate(string currency1, string currency2, double rate, DateTime dateTime)
        {
            Currency1 = currency1;
            Currency2 = currency2;
            Rate = rate;
            DateTime = dateTime;
        }
    }
}
