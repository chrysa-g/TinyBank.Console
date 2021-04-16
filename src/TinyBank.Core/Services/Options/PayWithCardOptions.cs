using System;

namespace TinyBank.Core.Services.Options
{
    public class PayWithCardOptions
    {
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
    }
}
