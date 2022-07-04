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
    public class CategorizacionController : Controller
    {
        string dbConn = "";
        public IConfiguration Configuration { get; }
        public CategorizacionController(IConfiguration configuration)
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
                    consulta = "SELECT C.IdComercio ,IdEmisor, RFC, GC.GiroComercial, RazonSocial, NombreComercial, CuentaDeposito, IdBanco, E.Estatus, GC.Tasa FROM Comercio as C INNER JOIN GiroComercio as GC ON C.IdGiroComercio = GC.IdGiroComercio INNER JOIN Estatus as E ON C.Estatus = E.IdEstatus WHERE IdEmisor LIKE '%" + IdEmisor + "%'";
                }
                else
                {
                    consulta = "SELECT C.IdComercio ,IdEmisor, RFC, GC.GiroComercial, RazonSocial, NombreComercial, CuentaDeposito, IdBanco, E.Estatus, GC.Tasa FROM Comercio as C INNER JOIN GiroComercio as GC ON C.IdGiroComercio = GC.IdGiroComercio INNER JOIN Estatus as E ON C.Estatus = E.IdEstatus";
                }

                List<Categorizacion> ListaCategorizacion = new List<Categorizacion>();

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
                                Categorizacion clsCategorizacion = new Categorizacion();
                                clsCategorizacion.IdComercio = Convert.ToInt32(dr["IdComercio"]);
                                clsCategorizacion.IdEmisor = dr.IsDBNull("IdEmisor") ? 0 : Convert.ToInt32(dr["IdEmisor"]);
                                clsCategorizacion.NombreComercial = Convert.ToString(dr["NombreComercial"]);
                                clsCategorizacion.Comision = Convert.ToString(dr["Tasa"]);
                                clsCategorizacion.Estatus = Convert.ToString(dr["Estatus"]);

                                ListaCategorizacion.Add(clsCategorizacion);
                            }
                            if (ListaCategorizacion.Count == 0)
                            {
                                connection.Close();
                                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Info, "No se encontró información relacionada.");
                                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });

                            }

                        }
                        connection.Close();
                    }
                    var res = new PermisosController(Configuration).GetPermisosUsuario("Index", "Categorizacion", puestoUser);
                    ViewBag.Editar = res.Editar;
                    ViewBag.User = currentUser;
                    return View(ListaCategorizacion);
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
            Categorizacion clsCategorizacion = new Categorizacion();
            try
            {
                ListadoDeClusters();
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    string consulta = $"SELECT C.IdComercio ,IdEmisor, RFC, GC.GiroComercial, RazonSocial, NombreComercial, CuentaDeposito, B.Banco, E.Estatus, GC.Tasa, C.IdCluster FROM Comercio as C INNER JOIN GiroComercio as GC ON C.IdGiroComercio = GC.IdGiroComercio INNER JOIN Estatus as E ON C.Estatus = E.IdEstatus INNER JOIN Banco as B ON C.IdBanco = B.IdBanco WHERE C.IdComercio = '{Id}'";
                    SqlCommand command = new SqlCommand(consulta, connection);

                    connection.Open();
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            clsCategorizacion.IdComercio = Convert.ToInt32(dr["IdComercio"]);
                            clsCategorizacion.IdEmisor = dr.IsDBNull("IdEmisor") ? 0 : Convert.ToInt32(dr["IdEmisor"]);
                            clsCategorizacion.RFC = Convert.ToString(dr["RFC"]);
                            clsCategorizacion.Giro = Convert.ToString(dr["GiroComercial"]);
                            clsCategorizacion.RazonSocial = Convert.ToString(dr["RazonSocial"]);
                            clsCategorizacion.NombreComercial = Convert.ToString(dr["NombreComercial"]);
                            clsCategorizacion.Cuenta = Convert.ToString(dr["CuentaDeposito"]);
                            clsCategorizacion.Banco = Convert.ToString(dr["Banco"]);
                            clsCategorizacion.Estatus = Convert.ToString(dr["Estatus"]);
                            clsCategorizacion.Comision = Convert.ToString(dr["Tasa"]);
                            clsCategorizacion.Cluster = dr.IsDBNull("IdCluster") ? 0 : Convert.ToInt32(dr["IdCluster"]);

                        }
                    }
                    connection.Close();
                }
                return PartialView("Editar",clsCategorizacion);
            }
            catch (Exception ex)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Favor de verificar su conexión.");
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
         
        }

        [HttpPost]
        [ActionName("Editar")]
        public IActionResult Editar(Categorizacion clsCategorizacion)
        {
            string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    ListadoDeClusters();
                    using (SqlConnection connection = new SqlConnection(dbConn))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("SP_Categorizar", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@IdComercio", clsCategorizacion.IdComercio);
                            command.Parameters.AddWithValue("@Cluster", Convert.ToInt32(clsCategorizacion.Cluster));
                            command.Parameters.AddWithValue("@Usuario_FUM", Convert.ToInt32(currentUser));

                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                        ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Categorización Aprobada.");
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

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Favor de revisar su conexión al Categorizar.");
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
        }

        public object ListadoDeClusters()
        {
            try
            {
                List<Cluster> ListaClusters = new List<Cluster>();
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    connection.Open();

                    string consulta = "SELECT * FROM Cluster WHERE Activo = 1";
                    SqlCommand command = new SqlCommand(consulta, connection);
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Cluster clsCluster = new Cluster();
                            clsCluster.IdCluster = Convert.ToInt32(dr["IdCluster"]);
                            clsCluster.NombreCluster = Convert.ToString(dr["NombreCluster"]);

                            ListaClusters.Add(clsCluster);
                        }
                    }
                    ViewData["Clusters"] = new SelectList(ListaClusters.ToList(), "IdCluster", "NombreCluster");
                    connection.Close();

                    return ViewData["Clusters"];
                }
            }
            catch (Exception ex)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
            return View();
        }

    }
}
