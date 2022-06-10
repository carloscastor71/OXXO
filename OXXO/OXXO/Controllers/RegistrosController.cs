using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OXXO.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OXXO.Controllers
{
    public class RegistrosController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public RegistrosController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Altas()
        {
            return View();
        }

        public IActionResult Categorizacion()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
