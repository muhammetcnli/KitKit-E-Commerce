using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Eticaret.Views.Home
{
    public class Purchase : PageModel
    {
        private readonly ILogger<Purchase> _logger;

        public Purchase(ILogger<Purchase> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}