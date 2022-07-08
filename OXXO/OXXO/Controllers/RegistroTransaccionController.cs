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
    public class RegistroTransaccionController : Controller
    {
        string dbConn = "";
        public IConfiguration Configuration { get; }
        public RegistroTransaccionController(IConfiguration configuration)
        {
            Configuration = configuration;
            dbConn = Configuration["ConnectionStrings:ConexionString"];
        }
        public IActionResult Index(string? alert)
        {
            ViewBag.Alert = alert;
            string puestoUser = HttpContext.Session.GetString("IdPerfil");
            string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();
            return View();
        }
        [HttpPost]
        public IActionResult Crear(RegistroTransaccion clsRegistro)
        {
            //if (String.IsNullOrEmpty(clsRegistro.Fecha) || String.IsNullOrEmpty(clsRegistro.Monto))
            //{
            //    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Warning, "Por favor ingrese correctamente el monto.");
            //    return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            //}
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
                            using (SqlCommand command = new SqlCommand("SP_RegistrarTransaccionManual", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;

                                command.Parameters.AddWithValue("@Referencia", clsRegistro.Referencia);
                                command.Parameters.AddWithValue("@Fecha", Convert.ToDateTime(clsRegistro.Fecha).ToString("yyyy-MM-dd"));
                                command.Parameters.AddWithValue("@Hora", clsRegistro.Hora);
                                command.Parameters.AddWithValue("@Monto", Convert.ToDecimal(string.Format("{0:.##}", clsRegistro.Monto)));
                                command.Parameters.AddWithValue("@Tienda", clsRegistro.Tienda);
                                command.Parameters.AddWithValue("@Estatus", clsRegistro.Estatus);

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
            //return View();
        }

    }
}
