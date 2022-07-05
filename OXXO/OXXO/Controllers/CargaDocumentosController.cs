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
using Dapper;

namespace OXXO.Controllers
{
    public class CargaDocumentosController : Controller
    {
        string dbConn = "";

        public IConfiguration Configuration { get; }

        public CargaDocumentosController(IConfiguration configuration)
        {
            Configuration = configuration;
            dbConn = Configuration["ConnectionStrings:ConexionString"];

        }

        //Metodo principal que carga la vista con los datos asociados al RFC.
        public ActionResult Index(string RFC, string? alert)
        {
            ViewBag.Alert = alert;
            Comercio clsComercio = new Comercio();
            try
            {
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    if (String.IsNullOrEmpty(RFC))
                    {
                        RFC = HttpContext.Session.GetString("RFC");

                    }
                    HttpContext.Session.SetString("RFC", RFC);

                    string consulta = $"SELECT * FROM Comercio WHERE RFC = '{RFC}'";
                    SqlCommand command = new SqlCommand(consulta, connection);

                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            clsComercio.IdComercio = Convert.ToInt32(dr["IdComercio"]);
                            clsComercio.NombreCompleto = Convert.ToString(dr["NombreCompleto"]);
                            clsComercio.NombreComercial = Convert.ToString(dr["NombreComercial"]);
                            clsComercio.RazonSocial = Convert.ToString(dr["RazonSocial"]);
                            clsComercio.RFC = Convert.ToString(dr["RFC"]);
                        }
                    }
                    connection.Close();
                }
               
                 
                return View(clsComercio);
            }
            catch (Exception ex)
            {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }

        }

        //Este metodo es el que se encarga de llenar y mostrar la tabla de los documentos asociados al Comercio(RFC).
        [HttpPost]
        public JsonResult Cargar(string RFC)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    if (String.IsNullOrEmpty(RFC))
                    {
                        RFC = HttpContext.Session.GetString("RFC");

                    }
                    HttpContext.Session.SetString("RFC", RFC);


                    List<Documento> ListaDocumento = new List<Documento>();

                    message res = new message();

                    string consulta = string.Format("exec SP_ConsultaDocumentos {0}", RFC);
                    //string consulta = "select * from Comercio";

                    SqlCommand command = new SqlCommand(consulta, connection);
                    connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Documento documento = new Documento();
                            documento.IdArchivo = Convert.ToInt32(dataReader["IdArchivo"]);
                            documento.NombreDocumento = Convert.ToString(dataReader["NombreDocumento"]);
                            documento.Descripcion = Convert.ToString(dataReader["Descripcion"]);
                            ListaDocumento.Add(documento);
                        }

                        res.status = true;
                        res.mensaje = "Success";
                        res.datad = ListaDocumento;
                        connection.Close();
                        return Json(res);

                    }

                }
            }
            catch (Exception ex)
            {
                List<message> error = new List<message>() {
                    new message {status = false, mensaje = ex.Message}
                };

                return Json(error);
            }
        }

        //Este metodo se encarga de mostrar el tipo de documento que vas a subir en un tag <select/>.
        public object ListadoTipoDocumento()
        {
            List<TipoDocumento> TipoDocumentoList = new List<TipoDocumento>();

            string connectionString = Configuration["ConnectionStrings:ConexionString"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SP_ConsultaTipoDocumento", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dataReader = command.ExecuteReader()
                        ;
                    while (dataReader.Read())
                    {
                        TipoDocumento tipoDocumento = new TipoDocumento
                        {
                            IdTipoDocumento = Convert.ToInt32(dataReader["IdTipoDocumento"]),
                            NombreDocumento = Convert.ToString(dataReader["NombreDocumento"])
                        };

                        TipoDocumentoList.Add(tipoDocumento);
                    }
                }

                ViewData["TipoDocumento"] = new SelectList(TipoDocumentoList.ToList(), "IdTipoDocumento", "NombreDocumento");
                connection.Close();

                return ViewData["TipoDocumento"];
            }
        }

        //Este metodo solo es para cargar la vista parcial donde se suben los documentos.
        [HttpGet]
        public ActionResult SubirDocumentos(string RFC, string? alert)
        {
            ViewBag.Alert = alert;

            ListadoTipoDocumento();

            return PartialView("_SubirDocumentos");
        }

        //Este metodo se encarga de subir los documentos 
        [HttpPost]
        public ActionResult CargarDocumentos(IFormFile documentos, string RFC, int IdTipoDocumento, string? alert)
        {
            ViewBag.Alert = alert;

            ListadoTipoDocumento();

            if (String.IsNullOrEmpty(RFC))
            {
                RFC = HttpContext.Session.GetString("RFC");
            }

            HttpContext.Session.SetString("RFC", RFC);


            string connectionString = Configuration["ConnectionStrings:ConexionString"];
            using SqlConnection connection = new SqlConnection(connectionString);
            ListadoTipoDocumento();

            if (documentos != null)
            {

                var fileName = Path.GetFileNameWithoutExtension(documentos.FileName);
                var fileExtension = Path.GetExtension(documentos.FileName);
                var newFileName = String.Concat(fileName, fileExtension);

                var objfiles = new Documento()
                {
                    nombre = newFileName,
                    extension = fileExtension,

                };

                using (var target = new MemoryStream())
                {
                    documentos.CopyTo(target);
                    objfiles.archivo = target.ToArray();
                }

                try
                {
                    connection.Open();
                    using SqlCommand command3 = new SqlCommand("SP_cargaDocumentos", connection);

                    command3.CommandType = CommandType.StoredProcedure;

                    command3.Parameters.AddWithValue("@Comercio", RFC);
                    command3.Parameters.AddWithValue("@Nombre", objfiles.nombre);
                    command3.Parameters.AddWithValue("@Archivo", objfiles.archivo);
                    command3.Parameters.AddWithValue("@Extension", objfiles.extension);
                    command3.Parameters.AddWithValue("@Itd", IdTipoDocumento);


                    command3.ExecuteNonQuery();
                    connection.Close();

                }
                catch (SqlException ex)
                {
                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                    return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
                }

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Documento subidos correctamente");
                return RedirectToAction("Index", "CargaDocumentos", new { alert = ViewBag.Alert });

            }

            return PartialView("_SubirDocumentos");

        }

        //Este metodo se encarga de descargar los documentos.
        [HttpGet]
        public FileResult DescargarDocumento(int IdArchivo)
        {


            List<Documento> ObjFiles = GetFileList();

            var FileById = (from FC in ObjFiles
                            where FC.IdArchivo.Equals(IdArchivo)
                            select new { FC.nombre, FC.archivo }).ToList().FirstOrDefault();

            return File(FileById.archivo, "application/pdf", FileById.nombre);

        }

        //Metodo que nos devuleve en forma de lista los documentos(Usado en conjunto con el metodo "DescargarDocumento").
        private List<Documento> GetFileList()
        {
            List<Documento> DetList = new List<Documento>();

            string connectionString = Configuration["ConnectionStrings:ConexionString"];
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            DetList = SqlMapper.Query<Documento>(connection, "GetFileDetails", commandType: CommandType.StoredProcedure).ToList();
            connection.Close();
            return DetList;
        }

        //  //Este metodo se encarga de eliminar los documentos.
        public ActionResult EliminarDocumento(string RFC, int IdArchivo, string? alert)
        {
            ViewBag.Alert = alert;

            ListadoTipoDocumento();

            if (String.IsNullOrEmpty(RFC))
            {
                RFC = HttpContext.Session.GetString("RFC");
            }

            HttpContext.Session.SetString("RFC", RFC);


            string connectionString = Configuration["ConnectionStrings:ConexionString"];
            using SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                using SqlCommand command3 = new SqlCommand("SP_EliminarDocumentos", connection);

                command3.CommandType = CommandType.StoredProcedure;

                command3.Parameters.AddWithValue("@RFC", RFC);
                command3.Parameters.AddWithValue("@IdArchivo", IdArchivo);



                command3.ExecuteNonQuery();
                connection.Close();

            }
            catch (SqlException ex)
            {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }

            ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Documento eliminado correctamente");
            return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
        }

    }
}
