using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;

using TinyBank.Core.Model;

using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;
using TinyBank.Core.Implementation.Data;

using Xunit;

namespace TinyBank.Core.Tests
{
    public class PaymentTests : IClassFixture<TinyBankFixture>
    {
        private readonly TinyBankDbContext _dbContext;
        private readonly IPaymentService _payment;
        private readonly CardTests _cardTests;

        public PaymentTests(TinyBankFixture fixture)
        {
            _dbContext = fixture.DbContext;
            _cardTests = new CardTests(fixture);
        }


        [Fact]
        public void PayWithCardSuccess()
        {
            var options = new PayWithCardOptions {
                CardNumber = "527779090000",
                Amount = 100,
                ExpirationMonth = 4,
                ExpirationYear = 2027
            };
            var card = _payment.Search(options)
                .FirstOrDefault();

            var availBalance = card.Accounts?.First().Balance;

            _payment.Pay(options);
            var newBalance = _payment.Search(options)
                .FirstOrDefault().Accounts?.First().Balance;

            Assert.Equal(availBalance, newBalance + options.Amount);
        }

    }
}
