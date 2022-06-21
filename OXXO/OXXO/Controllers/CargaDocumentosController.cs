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

namespace OXXO.Controllers
{
    public class CargaDocumentosController : Controller
    {
        //static List<Documento> documentosList = new List<Documento>();

        //public IConfiguration Configuration { get; }

        //public CargaDocumentosController(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        //private readonly ILogger<HomeController> _logger;

        //private readonly AgmContext _context;
        //public CargaDocumentosController(AgmContext context)
        //{
        //    _context = context;
        //}
        //public IConfiguration Configuration { get; }

        //public CargaDocumentosController(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}
        //public IActionResult Index()
        //{
        //    //documentosList = new List<Documento>();

        //    return View();
        //}




        //[HttpPost]
        //public async Task <IActionResult> Index(List<IFormFile> documentos)
        //{

        //    long size = documentos.Sum(f => f.Length);


        //    foreach (var formFile in documentos)
        //    {
        //        if (documentos != null)
        //        {
        //            if (formFile.Length > 0)
        //            {
        //                //Getting FileName

        //                var fileName = Path.GetFileName(formFile.FileName);
        //                //Getting file Extension
        //                var fileExtension = Path.GetExtension(fileName);
        //                // concatenating  FileName + FileExtension
        //                var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);
        //                var objfiles = new Documento()
        //                {
        //                    Nombre = newFileName,
        //                    Extension = fileExtension

        //                };

        //                using (var target = new MemoryStream())
        //                {
        //                    await formFile.CopyToAsync(target);
        //                     objfiles.Archivo = target.ToArray();
        //                }

        //                try
        //                {
        //                    string connectionString = Configuration["ConnectionStrings:ConexionString"];
        //                    using SqlConnection connection = new SqlConnection(connectionString);

        //                    connection.Open();
        //                    Guid RFC = Guid.NewGuid();

        //                    using SqlCommand command = new SqlCommand("SP_cargaDocumentos", connection);

        //                    command.CommandType = CommandType.StoredProcedure;

        //                    command.Parameters.AddWithValue("@IdComercio", 1);
        //                    command.Parameters.AddWithValue("@nombre", objfiles.Nombre);
        //                    command.Parameters.AddWithValue("@archivo", objfiles.Archivo);
        //                    command.Parameters.AddWithValue("@extension", objfiles.Extension);


        //                    command.ExecuteNonQuery();
        //                    connection.Close();



        //                }
        //                catch (Exception ex)
        //                {
        //                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
        //                    return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
        //                }
        //            }

        //        }

        //    }
        //    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Documentos guardados correctamente.");

        //    return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
        //}

        //    //private async Task<FileUploadViewModel> LoadAllFiles()
        //    //{
        //    //    var viewModel = new FileUploadViewModel();
        //    //    viewModel.FilesOnDatabase = await context.

        //    //        FilesOnDatabase.ToListAsync();

        //    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //    //public IActionResult Error()
        //    //{
        //    //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //    //}
        //    //    return viewModel;
        //    //}
        //    //[HttpPost]
        //    //public async Task<IActionResult> UploadToDatabase(List<IFormFile> files, string description)
        //    //{
        //    //    foreach (var file in files)
        //    //    {
        //    //        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
        //    //        var extension = Path.GetExtension(file.FileName);
        //    //        var fileModel = new FileOnDatabaseModel
        //    //        {
        //    //            CreatedOn = DateTime.UtcNow,
        //    //            FileType = file.ContentType,
        //    //            Extension = extension,
        //    //            Name = fileName,
        //    //            Description = description
        //    //        };
        //    //        using (var dataStream = new MemoryStream())
        //    //        {
        //    //            await file.CopyToAsync(dataStream);
        //    //            fileModel.Data = dataStream.ToArray();
        //    //        }
        //    //        context.FilesOnDatabase.Add(fileModel);
        //    //        context.SaveChanges();
        //    //    }
        //    //    TempData["Message"] = "File successfully uploaded to Database";
        //    //    return RedirectToAction("Index");
        //    //}
        //    //[HttpPost]
        //    //public async Task<IActionResult> SubirDocumento(List<IFormFile> files, string description)
        //    //{
        //    //    foreach (var file in files)
        //    //    {
        //    //        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
        //    //        var extension = Path.GetExtension(file.FileName);
        //    //        var fileModel = new FileOnDatabaseModel
        //    //        {
        //    //            CreatedOn = DateTime.UtcNow,
        //    //            FileType = file.ContentType,
        //    //            Extension = extension,
        //    //            Name = fileName,
        //    //            Description = description
        //    //        };
        //    //        using (var dataStream = new MemoryStream())
        //    //        {
        //    //            await file.CopyToAsync(dataStream);
        //    //            fileModel.Data = dataStream.ToArray();
        //    //        }
        //    //        context.FilesOnDatabase.Add(fileModel);
        //    //        context.SaveChanges();
        //    //    }
        //    //    TempData["Message"] = "File successfully uploaded to Database";
        //    //    return RedirectToAction("Index");
        //    //}



    }
}



