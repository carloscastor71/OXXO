using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using OXXO.Controllers;
using OXXO.Models;
using OXXO.Services;
using OXXO.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace OXXO.Controllers
{
    public class TipoDocumentoController : Controller
    {
        string dbConn = "";
        public IConfiguration Configuration { get; }
        public TipoDocumentoController(IConfiguration configuration)
        {
            Configuration = configuration;
            dbConn = Configuration["ConnectionStrings:ConexionString"];
        }
        public IActionResult Index(string? alert, string NombreDocumento, string Descripcion, string TipoArchivo)
        {
            ViewBag.Alert = alert;
            string puestoUser = HttpContext.Session.GetString("IdPerfil");
            string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();
            string consulta = "";

            try
            {
                Permisos permisos = new Permisos();
                if (!String.IsNullOrEmpty(NombreDocumento) && !String.IsNullOrEmpty(Descripcion) && !String.IsNullOrEmpty(TipoArchivo))
                {
                    consulta = "Select * From TipoDocumento WHERE NombreDocumento LIKE '%" + NombreDocumento + "%' AND Descripcion LIKE '%" + Descripcion + "%' AND Descripcion LIKE '%" + TipoArchivo + "%'";
                }
                else if (!String.IsNullOrEmpty(NombreDocumento) && !String.IsNullOrEmpty(Descripcion))
                {
                    consulta = "Select * From TipoDocumento WHERE NombreDocumento LIKE '%" + NombreDocumento + "%' AND Descripcion LIKE '%" + Descripcion + "%'";
                }
                else if (!String.IsNullOrEmpty(NombreDocumento) && !String.IsNullOrEmpty(TipoArchivo))
                {
                    consulta = "Select * From TipoDocumento WHERE NombreDocumento LIKE '%" + NombreDocumento + "%' AND TipoArchivo LIKE '%" + TipoArchivo + "%'";
                }
                if (!String.IsNullOrEmpty(Descripcion) && !String.IsNullOrEmpty(TipoArchivo))
                {
                    consulta = "Select * From TipoDocumento WHERE Descripcion LIKE '%" + Descripcion + "%' AND TipoArchivo LIKE '%" + TipoArchivo + "%'";
                }
                else if (!String.IsNullOrEmpty(NombreDocumento))
                {
                    consulta = "Select * From TipoDocumento WHERE NombreDocumento LIKE '%" + NombreDocumento + "%'";
                }
                else if (!String.IsNullOrEmpty(Descripcion))
                {
                    consulta = "Select * From TipoDocumento WHERE Descripcion LIKE '%" + Descripcion + "%'";
                }
                else if (!String.IsNullOrEmpty(TipoArchivo))
                {
                    consulta = "Select * From TipoDocumento WHERE TipoArchivo LIKE '%" + TipoArchivo + "%'";
                }
                else
                {
                    consulta = "SELECT * FROM TipoDocumento";
                }

                List<TipoDocumento> TipoDocsLista = new List<TipoDocumento>();

                if (!String.IsNullOrEmpty(consulta))
                {
                    using (SqlConnection connection = new SqlConnection(dbConn))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(consulta, connection);
                        using (SqlDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                TipoDocumento clsTipoDoc = new TipoDocumento();

                                clsTipoDoc.IdTipoDocumento = Convert.ToInt32(dr["IdTipoDocumento"]);
                                clsTipoDoc.NombreDocumento = Convert.ToString(dr["NombreDocumento"]);
                                clsTipoDoc.Descripcion = Convert.ToString(dr["Descripcion"]);
                                clsTipoDoc.TipoArchivo = Convert.ToString(dr["TipoArchivo"]);
                                clsTipoDoc.PersonaFisica = Convert.ToBoolean(dr["PersonaFisica"]);
                                clsTipoDoc.PersonaMoral = Convert.ToBoolean(dr["PersonaMoral"]);
                                clsTipoDoc.Obligatorio = Convert.ToBoolean(dr["Obligatorio"]);

                                TipoDocsLista.Add(clsTipoDoc);
                            }
                        }
                        connection.Close();
                    }
                    var res = new PermisosController(Configuration).GetPermisosUsuario("Index", "TipoDocumento", puestoUser);
                    //ViewBag.Crear = res.Crear;
                    //ViewBag.Editar = res.Editar;
                    ViewBag.Crear = true;
                    ViewBag.Editar = true;

                    ViewBag.User = currentUser;
                    return View(TipoDocsLista);
                }
                else
                {
                    ViewBag.Crear = false;
                    ViewBag.Editar = false;

                    ViewBag.User = currentUser;
                    return View(TipoDocsLista);
                }

            }
            catch (Exception ex)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }

        }

        [HttpGet]
        public IActionResult Crear()
        {
            return PartialView("Crear");
        }

        [HttpPost]
        public IActionResult Crear(TipoDocumento clsTipoDoc)
        {
            string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection connection = new SqlConnection(dbConn))
                    {
                        connection.Open();
                        try
                        {
                            using (SqlCommand command = new SqlCommand("SP_CrearTipoDocumento", connection)) ////aún no está listo el SP
                            {
                                command.CommandType = CommandType.StoredProcedure;

                                //SqlParameter Mensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar,100);
                                //Mensaje.Direction = ParameterDirection.Output;

                                command.Parameters.AddWithValue("@NombreDocumento", clsTipoDoc.NombreDocumento);
                                command.Parameters.AddWithValue("@Descripcion", clsTipoDoc.Descripcion);
                                command.Parameters.AddWithValue("@TipoArchivo", clsTipoDoc.TipoArchivo);
                                command.Parameters.AddWithValue("@PersonaFisica", Convert.ToInt32(clsTipoDoc.PersonaFisica));
                                command.Parameters.AddWithValue("@PersonaMoral", Convert.ToInt32(clsTipoDoc.PersonaMoral));
                                command.Parameters.AddWithValue("@Obligatorio", Convert.ToInt32(clsTipoDoc.Obligatorio));
                                //command.Parameters.Add(Mensaje);
                                //command.Parameters["@Mensaje"].Direction = ParameterDirection.Output;

                                //
                                command.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 100);
                                command.Parameters["@Mensaje"].Direction = ParameterDirection.Output;

                                command.ExecuteNonQuery();
                                string Mensaje = Convert.ToString(command.Parameters["@Mensaje"].Value);
                                
                                command.Parameters.Clear();
                                
                                connection.Close();
                                
                                if (Mensaje.Length > 1)
                                {
                                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Warning, Mensaje);
                                    return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
                                }
                                else
                                {
                                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Registro completado con éxito.");
                                    return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
                                }

                            }
                            
                           
                        }
                        catch (Exception ex)
                        {

                            connection.Close();
                            ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                            return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
                        }
                       
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Favor de revisar la información de Creación.");
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
        }

        [HttpGet]
        public IActionResult Editar( int Id) 
        {
            TipoDocumento clsTipoDocumento = new TipoDocumento();
            try
            {
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    string consulta = $"SELECT * FROM TipoDocumento WHERE IdTipoDocumento = '{Id}'";
                    SqlCommand command = new SqlCommand(consulta, connection);

                    connection.Open();
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            clsTipoDocumento.IdTipoDocumento = Convert.ToInt32(dr["IdTipoDocumento"]);
                            clsTipoDocumento.NombreDocumento = Convert.ToString(dr["NombreDocumento"]);
                            clsTipoDocumento.Descripcion = Convert.ToString(dr["Descripcion"]);
                            clsTipoDocumento.TipoArchivo = Convert.ToString(dr["TipoArchivo"]);
                            clsTipoDocumento.PersonaFisica = dr.IsDBNull("PersonaFisica") ? false : Convert.ToBoolean(dr["PersonaFisica"]);
                            clsTipoDocumento.PersonaMoral = dr.IsDBNull("PersonaMoral") ? false : Convert.ToBoolean(dr["PersonaMoral"]);
                            clsTipoDocumento.Obligatorio = dr.IsDBNull("Obligatorio") ? false : Convert.ToBoolean(dr["Obligatorio"]);
                        }
                    }
                    connection.Close();
                }
                return PartialView("Editar",clsTipoDocumento);
            }
            catch (Exception)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Favor de verificar su conexión.");
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
         
        }

        [HttpPost]
        [ActionName("Editar")]
        public IActionResult Editar(TipoDocumento clsTipoDocumento)
        {
            string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection connection = new SqlConnection(dbConn))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("SP_EditarTipoDocumento", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@IdTipoDocumento", clsTipoDocumento.IdTipoDocumento);
                            command.Parameters.AddWithValue("@NombreDocumento", clsTipoDocumento.NombreDocumento);
                            command.Parameters.AddWithValue("@Descripcion", clsTipoDocumento.Descripcion);
                            command.Parameters.AddWithValue("@TipoArchivo", clsTipoDocumento.TipoArchivo);
                            command.Parameters.AddWithValue("@PersonaFisica", Convert.ToInt32(clsTipoDocumento.PersonaFisica));
                            command.Parameters.AddWithValue("@PersonaMoral", Convert.ToInt32(clsTipoDocumento.PersonaMoral));
                            command.Parameters.AddWithValue("@Obligatorio", Convert.ToInt32(clsTipoDocumento.Obligatorio));
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                        ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Registro actualizado con éxito.");
                        return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Favor de revisar su conexión al editar.");
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
        }

    }
}
