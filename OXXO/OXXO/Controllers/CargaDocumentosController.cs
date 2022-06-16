using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OXXO.Enums;
using OXXO.Models;
using OXXO.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace OXXO.Controllers
{
    public class CargaDocumentosController : Controller
    {
        static List<Documento> documentosList = new List<Documento>();

        public IConfiguration Configuration { get; }

        public CargaDocumentosController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private readonly ILogger<HomeController> _logger;

        public IActionResult Index(string? alert)
        {
             documentosList = new List<Documento>();

            return View();
        }
        [HttpPost("UploadFiles")]
        public ActionResult SubirDocumentos(string nombre, List<IFormFile> Documento) {
            return RedirectToAction();



        }


        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


    