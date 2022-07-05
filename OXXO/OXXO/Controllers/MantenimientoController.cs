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
    public class MantenimientoController : Controller
    {
        string dbConn = "";
        public IConfiguration Configuration { get; }
        public MantenimientoController(IConfiguration configuration)
        {
            Configuration = configuration;
            dbConn = Configuration["ConnectionStrings:ConexionString"];
        }
        public IActionResult Index(string? alert, string IdEmisor)
        {
            ViewBag.Alert = alert;
            string puestoUser = HttpContext.Session.GetString("IdPerfil");
            string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();
            string consulta = "";

            try
            {
                Permisos permisos = new Permisos();
                
                if (!String.IsNullOrEmpty(IdEmisor))
                {
                    consulta = "SELECT C.IdComercio ,IdEmisor, RFC, GC.GiroComercial, RazonSocial, NombreComercial, CuentaDeposito, B.Banco, C.Activo FROM Comercio as C INNER JOIN GiroComercio as GC ON C.IdGiroComercio = GC.IdGiroComercio INNER JOIN Banco as B ON C.IdBanco = B.IdBanco WHERE IdEmisor LIKE '%" + IdEmisor + "%'";
                }
                else
                {
                    consulta = "SELECT C.IdComercio ,IdEmisor, RFC, GC.GiroComercial, RazonSocial, NombreComercial, CuentaDeposito, B.Banco, C.Activo FROM Comercio as C INNER JOIN GiroComercio as GC ON C.IdGiroComercio = GC.IdGiroComercio INNER JOIN Banco as B ON C.IdBanco = B.IdBanco";
                }

                List<Mantenimiento> ListaMantenimiento = new List<Mantenimiento>();
                
                if (!String.IsNullOrEmpty(consulta))
                {
                    ListadoDeBancos();
                    using (SqlConnection connection = new SqlConnection(dbConn))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(consulta, connection);
                        using (SqlDataReader dr = command.ExecuteReader())
                        {
                            
                            while (dr.Read())
                            {
                                Mantenimiento clsMantenimiento = new Mantenimiento();
                                clsMantenimiento.IdComercio = Convert.ToInt32(dr["IdComercio"]);
                                clsMantenimiento.IdEmisor = dr.IsDBNull("IdEmisor") ? 0 : Convert.ToInt32(dr["IdEmisor"]);
                                clsMantenimiento.NombreComercial = Convert.ToString(dr["NombreComercial"]);
                                clsMantenimiento.RFC = Convert.ToString(dr["RFC"]);
                                clsMantenimiento.Giro = Convert.ToString(dr["GiroComercial"]);
                                clsMantenimiento.RazonSocial = Convert.ToString(dr["RazonSocial"]);
                                clsMantenimiento.Banco = Convert.ToString(dr["Banco"]);
                                clsMantenimiento.Cuenta = Convert.ToString(dr["CuentaDeposito"]);
                                clsMantenimiento.Activo = Convert.ToInt32(dr["Activo"]);

                                ListaMantenimiento.Add(clsMantenimiento);
                            }
                            if (ListaMantenimiento.Count == 0)
                            {
                                connection.Close();
                                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Info, "No se encontró información relacionada.");
                                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });

                            }

                        }
                        connection.Close();
                    }
                    var res = new PermisosController(Configuration).GetPermisosUsuario("Index", "Mantenimiento", puestoUser);
                    ViewBag.Editar = res.Editar;
                    ViewBag.User = currentUser;
                    return View(ListaMantenimiento);
                }
                else
                {
                    ViewBag.Crear = false;
                    ViewBag.Editar = false;

                    ViewBag.User = currentUser;
                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Info, "Favor de revisar la consulta esperada.");
                    return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
                }

            }
            catch (Exception ex)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }

        }

        [HttpGet]
        public IActionResult Editar( int Id) 
        {
            Mantenimiento clsMantenimiento = new Mantenimiento();
            try
            {
                ListadoDeBancos();
                ListadoDeGiros();
            
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    string consulta = $"SELECT * FROM Comercio WHERE IdComercio = '{Id}'";
                    SqlCommand command = new SqlCommand(consulta, connection);

                    connection.Open();
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            clsMantenimiento.IdComercio = Convert.ToInt32(dr["IdComercio"]);
                            clsMantenimiento.IdEmisor = dr.IsDBNull("IdEmisor") ? 0 : Convert.ToInt32(dr["IdEmisor"]);
                            clsMantenimiento.Activo = Convert.ToInt32(dr["Activo"]);
                            clsMantenimiento.RazonSocial = Convert.ToString(dr["RazonSocial"]);
                            clsMantenimiento.NombreComercial = Convert.ToString(dr["NombreComercial"]);
                            clsMantenimiento.Direccion = Convert.ToString(dr["Direccion"]);
                            clsMantenimiento.Giro = Convert.ToString(dr["IdGiroComercio"]);
                            clsMantenimiento.RFC = Convert.ToString(dr["RFC"]);
                            clsMantenimiento.Telefono = Convert.ToString(dr["Telefono"]);
                            clsMantenimiento.Portal = Convert.ToString(dr["Portal"]);
                            clsMantenimiento.Correo = Convert.ToString(dr["Correo"]);
                            clsMantenimiento.Cuenta = Convert.ToString(dr["CuentaDeposito"]);
                            clsMantenimiento.Banco = Convert.ToString(dr["IdBanco"]);
                        }
                    }
                    connection.Close();
                }
                return PartialView("Editar",clsMantenimiento);
            }
            catch (Exception)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Favor de verificar su conexión.");
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
         
        }

        [HttpPost]
        [ActionName("Editar")]
        public IActionResult Editar(Mantenimiento clsMantenimiento)
        {
            string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    //ListadoDeGiros();
                    using (SqlConnection connection = new SqlConnection(dbConn))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("SP_MantenimientoComercio", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@IdComercio", clsMantenimiento.IdComercio);
                            command.Parameters.AddWithValue("@Activo", clsMantenimiento.Activo);
                            command.Parameters.AddWithValue("@RazonSocial", clsMantenimiento.RazonSocial);
                            command.Parameters.AddWithValue("@NombreComercial", clsMantenimiento.NombreComercial);
                            command.Parameters.AddWithValue("@Direccion", clsMantenimiento.Direccion);
                            command.Parameters.AddWithValue("@IdGiro", Convert.ToInt32(clsMantenimiento.Giro));
                            command.Parameters.AddWithValue("@RFC", clsMantenimiento.RFC);
                            command.Parameters.AddWithValue("@Telefono", clsMantenimiento.Telefono);
                            command.Parameters.AddWithValue("@Portal", clsMantenimiento.Portal);
                            command.Parameters.AddWithValue("@Correo", clsMantenimiento.Correo);
                            command.Parameters.AddWithValue("@Cuenta", clsMantenimiento.Cuenta);
                            command.Parameters.AddWithValue("@IdBanco", Convert.ToInt32(clsMantenimiento.Banco));
                            command.Parameters.AddWithValue("@Usuario_FUM", Convert.ToInt32(currentUser));
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                        ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Información Actualizada Correctamente.");
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

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Favor de revisar su conexión al actualizar.");
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
        }

        public object ListadoDeGiros()
        {
            try
            {
                List<GiroComercio> ListaGiros = new List<GiroComercio>();
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    connection.Open();

                    string consulta = "SELECT * FROM GiroComercio WHERE Activo = 1";
                    SqlCommand command = new SqlCommand(consulta, connection);
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            GiroComercio clsGiro = new GiroComercio();
                            clsGiro.IdGiroComercio = Convert.ToInt32(dr["IdGiroComercio"]);
                            clsGiro.GiroComercial = Convert.ToString(dr["GiroComercial"]);

                            ListaGiros.Add(clsGiro);
                        }
                    }
                    ViewData["Giros"] = new SelectList(ListaGiros.ToList(), "IdGiroComercio", "GiroComercial");
                    connection.Close();

                    return ViewData["Giros"];
                }
            }
            catch (Exception)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "No se pudo extrar la información de los Giros Comerciales");
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
      
        }
        public object ListadoDeBancos()
        {
            try
            {
                List<Banco> ListaBancos = new List<Banco>();
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    connection.Open();

                    string consulta = "SELECT * FROM Banco WHERE Activo = 1";
                    SqlCommand command = new SqlCommand(consulta, connection);
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Banco clsBanco = new Banco();
                            clsBanco.IdBanco = Convert.ToInt32(dr["IdBanco"]);
                            clsBanco.Bancos = Convert.ToString(dr["Banco"]);

                            ListaBancos.Add(clsBanco);
                        }
                    }
                    ViewData["Bancos"] = new SelectList(ListaBancos.ToList(), "IdBanco", "Bancos");
                    connection.Close();

                    return ViewData["Bancos"];
                }
            }
            catch (Exception)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "No se pudo extrar la información de los Bancos");
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }

        }

    }
}
