using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;
using TinyBank.Web.Extensions;
using TinyBank.Web.Models;

namespace TinyBank.Web.Controllers
{
    [Route("card")]
    public class CardController : Controller
    {
        private readonly IPaymentService _payments;
        private readonly ILogger<HomeController> _logger;
        private readonly TinyBankDbContext _dbContext;

        // Path: '/card'
        public CardController(
            TinyBankDbContext dbContext,
            ILogger<HomeController> logger,
            IPaymentService payments)
        {
            _logger = logger;
            _payments = payments;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
             return View();
        }

        
        [HttpPost("checkout")]
        public IActionResult Pay([FromBody] PayWithCardOptions options)
        {
            var result = _payments.Pay(options);

            if (!result.IsSuccessful()) {
                return result.ToActionResult();
            }

            return Ok();
        }
    }
}
