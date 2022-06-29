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
    public class GiroComercioController : Controller
    {
        string dbConn = "";
        public IConfiguration Configuration { get; }
        public GiroComercioController(IConfiguration configuration)
        {
            Configuration = configuration;
            dbConn = Configuration["ConnectionStrings:ConexionString"];
        }
        public IActionResult Index(string? alert, string NombreGiro, string Activo)
        {
            ViewBag.Alert = alert;
            string puestoUser = HttpContext.Session.GetString("IdPerfil");
            string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();
            string consulta = "";

            try
            {
                Permisos permisos = new Permisos();
                if (!String.IsNullOrEmpty(NombreGiro) && !String.IsNullOrEmpty(Activo))
                {
                    consulta = "SELECT * FROM GiroComercio WHERE GiroComercial LIKE '%" + NombreGiro + "%' AND Activo =" + Activo;
                }
                
                else if (!String.IsNullOrEmpty(NombreGiro))
                {
                    consulta = "SELECT * FROM GiroComercio WHERE GiroComercial LIKE '%" + NombreGiro + "%'";
                }
                else if (!String.IsNullOrEmpty(Activo))
                {
                    consulta = "SELECT * FROM GiroComercio WHERE Activo =" + Activo;
                }
                else
                {
                    consulta = "SELECT * FROM GiroComercio";
                }

                List<GiroComercio> GiroLista = new List<GiroComercio>();

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
                                GiroComercio clsGiro = new GiroComercio();

                                clsGiro.IdGiroComercio = Convert.ToInt32(dr["IdGiroComercio"]);
                                clsGiro.GiroComercial = Convert.ToString(dr["GiroComercial"]);
                                clsGiro.Activo = Convert.ToInt32(dr["Activo"]);


                                GiroLista.Add(clsGiro);
                            }
                        }
                        connection.Close();
                    }
                    var res = new PermisosController(Configuration).GetPermisosUsuario("Index", "GiroComercio", puestoUser);
                    ViewBag.Crear = res.Crear;
                    ViewBag.Editar = res.Editar;


                    ViewBag.User = currentUser;
                    return View(GiroLista);
                }
                else
                {
                    ViewBag.Crear = false;
                    ViewBag.Editar = false;

                    ViewBag.User = currentUser;
                    return View(GiroLista);
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
        public IActionResult Crear(GiroComercio clsGiro)
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
                            using (SqlCommand command = new SqlCommand("SP_CrearGiroComercio", connection)) 
                            {
                                command.CommandType = CommandType.StoredProcedure;

                                command.Parameters.AddWithValue("@GiroComercial", clsGiro.GiroComercial);
                                //command.Parameters.AddWithValue("@Activo", clsGiro.Activo);
                                command.Parameters.AddWithValue("@Usuario_FAI", currentUser);

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
            GiroComercio clsGiro = new GiroComercio();
            try
            {
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    string consulta = $"SELECT * FROM GiroComercio WHERE IdGiroComercio = '{Id}'";
                    SqlCommand command = new SqlCommand(consulta, connection);

                    connection.Open();
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            clsGiro.IdGiroComercio = Convert.ToInt32(dr["IdGiroComercio"]);
                            clsGiro.GiroComercial = Convert.ToString(dr["GiroComercial"]);
                            clsGiro.Activo = Convert.ToInt32(dr["Activo"]);
                        }
                    }
                    connection.Close();
                }
                return PartialView("Editar",clsGiro);
            }
            catch (Exception)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Favor de verificar su conexión.");
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
         
        }

        [HttpPost]
        [ActionName("Editar")]
        public IActionResult Editar(GiroComercio clsGiro)
        {
            string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection connection = new SqlConnection(dbConn))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("SP_EditarGiroComercio", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@IdGiroComercio", clsGiro.IdGiroComercio);
                            command.Parameters.AddWithValue("@GiroComercial", clsGiro.GiroComercial);
                            command.Parameters.AddWithValue("@Activo", clsGiro.Activo);
                            command.Parameters.AddWithValue("@Usuario_FUM", Convert.ToInt32(currentUser));
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
