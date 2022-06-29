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
    public class BancoController : Controller
    {
        string dbConn = "";
        public IConfiguration Configuration { get; }
        public BancoController(IConfiguration configuration)
        {
            Configuration = configuration;
            dbConn = Configuration["ConnectionStrings:ConexionString"];
        }
        public IActionResult Index(string? alert, string NombreBanco, string Activo)
        {
            ViewBag.Alert = alert;
            string puestoUser = HttpContext.Session.GetString("IdPerfil");
            string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();
            string consulta = "";

            try
            {
                Permisos permisos = new Permisos();
                if (!String.IsNullOrEmpty(NombreBanco) && !String.IsNullOrEmpty(Activo))
                {
                    consulta = "SELECT * FROM Banco WHERE Banco LIKE '%" + NombreBanco + "%' AND Activo =" + Activo;
                }
                
                else if (!String.IsNullOrEmpty(NombreBanco))
                {
                    consulta = "SELECT * FROM Banco WHERE Banco LIKE '%" + NombreBanco + "%'";
                }
                else if (!String.IsNullOrEmpty(Activo))
                {
                    consulta = "SELECT * FROM Banco WHERE Activo =" + Activo;
                }
                else
                {
                    consulta = "SELECT * FROM Banco";
                }

                List<Banco> BancoLista = new List<Banco>();

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
                                Banco clsBanco = new Banco();

                                clsBanco.IdBanco = Convert.ToInt32(dr["IdBanco"]);
                                clsBanco.Bancos = Convert.ToString(dr["Banco"]);
                                clsBanco.Activo = Convert.ToInt32(dr["Activo"]);


                                BancoLista.Add(clsBanco);
                            }
                        }
                        connection.Close();
                    }
                    var res = new PermisosController(Configuration).GetPermisosUsuario("Index", "Banco", puestoUser);
                    ViewBag.Crear = res.Crear;
                    ViewBag.Editar = res.Editar;


                    ViewBag.User = currentUser;
                    return View(BancoLista);
                }
                else
                {
                    ViewBag.Crear = false;
                    ViewBag.Editar = false;

                    ViewBag.User = currentUser;
                    return View(BancoLista);
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
        public IActionResult Crear(Banco clsBanco)
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
                            using (SqlCommand command = new SqlCommand("SP_CrearBanco", connection)) 
                            {
                                command.CommandType = CommandType.StoredProcedure;

                                command.Parameters.AddWithValue("@Banco", clsBanco.Bancos);
                                //command.Parameters.AddWithValue("@Activo", clsBanco.Activo);
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
            Banco clsBanco = new Banco();
            try
            {
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    string consulta = $"SELECT * FROM Banco WHERE IdBanco = '{Id}'";
                    SqlCommand command = new SqlCommand(consulta, connection);

                    connection.Open();
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            clsBanco.IdBanco = Convert.ToInt32(dr["IdBanco"]);
                            clsBanco.Bancos = Convert.ToString(dr["Banco"]);
                            clsBanco.Activo = Convert.ToInt32(dr["Activo"]);
                        }
                    }
                    connection.Close();
                }
                return PartialView("Editar",clsBanco);
            }
            catch (Exception)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Favor de verificar su conexión.");
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
         
        }

        [HttpPost]
        [ActionName("Editar")]
        public IActionResult Editar(Banco clsBanco)
        {
            string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection connection = new SqlConnection(dbConn))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("SP_EditarBanco", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@IdBanco", clsBanco.IdBanco);
                            command.Parameters.AddWithValue("@Banco", clsBanco.Bancos);
                            command.Parameters.AddWithValue("@Activo", clsBanco.Activo);
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
