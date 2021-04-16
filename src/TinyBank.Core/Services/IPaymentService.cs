using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBank.Core.Model;

namespace TinyBank.Core.Services
{
    public interface IPaymentService
    {
        public ApiResult<Card> Pay(Options.PayWithCardOptions options);
       
        public IQueryable<Card> Search(
            Options.PayWithCardOptions options);
    }
}
