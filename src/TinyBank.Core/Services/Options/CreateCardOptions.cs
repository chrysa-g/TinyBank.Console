using System;

namespace TinyBank.Core.Services.Options
{
    public class CreateCardOptions
    {
        public string CardNumber { get; set; }
        public DateTime ExpirationDate{ get; set; }
    }
}
