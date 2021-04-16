using AutoMapper;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;

using TinyBank.Core.Constants;
using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;

namespace TinyBank.Core.Implementation.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly TinyBankDbContext _dbContext;
        private readonly IMapper _mapper;

        public PaymentService(TinyBankDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = new MapperConfiguration(
                    cfg => cfg.CreateMap<RegisterCustomerOptions, Customer>())
                .CreateMapper();
        }

       
        public IQueryable<Card> Search(PayWithCardOptions options)
        {
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            // SELECT FROM CARD
            var q = _dbContext.Set<Card>()
                .AsQueryable();

            // SELECT FROM Card WHERE cardnumber = options.cardnumber
            if (options.CardNumber != null) {
                q = q.Where(c => c.CardNumber == options.CardNumber);
            }
                        
            
            q = q.Take(500);

            return q;
        }

        private bool Exists(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber)) {
                throw new ArgumentNullException(cardNumber);
            }

            return Search(
                new PayWithCardOptions() {
                    CardNumber = cardNumber
                }).Any();
        }

        public ApiResult<Card> Pay(PayWithCardOptions options)
        {
            if (options == null) {
                return new TinyBank.Core.ApiResult<Card>() {
                    Code = ApiResultCode.NotFound,
                    ErrorText = $"Bad request"
                };
            }

            if (options.CardNumber == null) {
                return new TinyBank.Core.ApiResult<Card>() {
                    Code = ApiResultCode.NotFound,
                    ErrorText = $"Cardnumber needed"
                };
            }


            Card card = Search(new PayWithCardOptions() {
                CardNumber = options.CardNumber
                }).Include(c => c.Accounts)
                .SingleOrDefault();

            if (card == null) {
                return new ApiResult<Card>() {
                    Code = Constants.ApiResultCode.BadRequest,
                    ErrorText = $"Card was not found"
                };
            };

            if (!card.Active) {
                    return new ApiResult<Card>() {
                        Code = Constants.ApiResultCode.BadRequest,
                        ErrorText = $"Card is not active"
                    };
                };

            if(card.Expiration.Month!=options.ExpirationMonth || card.Expiration.Year!=options.ExpirationYear) {
                return new ApiResult<Card>() {
                    Code = Constants.ApiResultCode.BadRequest,
                    ErrorText = $"Card expiration not valid"
                };
            };

            if(card.Accounts == null || card.Accounts.Count==0 || card.Accounts?[0].Balance<options.Amount) {
                return new ApiResult<Card>() {
                    Code = Constants.ApiResultCode.BadRequest,
                    ErrorText = $"Insufficient account balance"
                };
            };

            Account account = card.Accounts?[0];
            account.Balance = account.Balance - options.Amount;
            _dbContext.SaveChanges();

            return new ApiResult<Card>() {
                Data = card
            };
        }

        
    }
}
