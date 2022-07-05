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
    public class ClusterController : Controller
    {
        string dbConn = "";
        public IConfiguration Configuration { get; }
        public ClusterController(IConfiguration configuration)
        {
            Configuration = configuration;
            dbConn = Configuration["ConnectionStrings:ConexionString"];
        }
        public IActionResult Index(string? alert, string NombreCluster, string Activo)
        {
            ViewBag.Alert = alert;
            string puestoUser = HttpContext.Session.GetString("IdPerfil");
            string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();
            string consulta = "";

            try
            {
                Permisos permisos = new Permisos();
                if (!String.IsNullOrEmpty(NombreCluster) && !String.IsNullOrEmpty(Activo))
                {
                    consulta = "SELECT * FROM Cluster WHERE NombreCluster LIKE '%" + NombreCluster + "%' AND Activo =" + Activo;
                }
                
                else if (!String.IsNullOrEmpty(NombreCluster))
                {
                    consulta = "SELECT * FROM Cluster WHERE NombreCluster LIKE '%" + NombreCluster + "%'";
                }
                else if (!String.IsNullOrEmpty(Activo))
                {
                    consulta = "SELECT * FROM Cluster WHERE Activo =" + Activo;
                }
                else
                {
                    consulta = "SELECT * FROM Cluster";
                }

                List<Cluster> ClusterLista = new List<Cluster>();

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
                                Cluster clsCluster = new Cluster();

                                clsCluster.IdCluster = Convert.ToInt32(dr["IdCluster"]);
                                clsCluster.NombreCluster = Convert.ToString(dr["NombreCluster"]);
                                clsCluster.Activo = Convert.ToInt32(dr["Activo"]);


                                ClusterLista.Add(clsCluster);
                            }
                        }
                        connection.Close();
                    }
                    var res = new PermisosController(Configuration).GetPermisosUsuario("Index", "Cluster", puestoUser);
                    ViewBag.Crear = res.Crear;
                    ViewBag.Editar = res.Editar;


                    ViewBag.User = currentUser;
                    return View(ClusterLista);
                }
                else
                {
                    ViewBag.Crear = false;
                    ViewBag.Editar = false;

                    ViewBag.User = currentUser;
                    return View(ClusterLista);
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
        public IActionResult Crear(Cluster clsCluster)
        {
            string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();
            try
            {
                
                    using (SqlConnection connection = new SqlConnection(dbConn))
                    {
                        connection.Open();
                        try
                        {
                            using (SqlCommand command = new SqlCommand("SP_CrearCluster", connection)) 
                            {
                                command.CommandType = CommandType.StoredProcedure;

                                command.Parameters.AddWithValue("@NombreCluster", clsCluster.NombreCluster);
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
            catch (Exception)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Favor de revisar la información de Creación.");
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
        }
        [HttpGet]
        public IActionResult Editar( int Id) 
        {
            Cluster clsCluster = new Cluster();
            try
            {
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    string consulta = $"SELECT * FROM Cluster WHERE IdCluster = '{Id}'";
                    SqlCommand command = new SqlCommand(consulta, connection);

                    connection.Open();
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            clsCluster.IdCluster = Convert.ToInt32(dr["IdCluster"]);
                            clsCluster.NombreCluster = Convert.ToString(dr["NombreCluster"]);
                            clsCluster.Activo = Convert.ToInt32(dr["Activo"]);
                        }
                    }
                    connection.Close();
                }
                return PartialView("Editar",clsCluster);
            }
            catch (Exception)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Favor de verificar su conexión.");
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
         
        }

        [HttpPost]
        [ActionName("Editar")]
        public IActionResult Editar(Cluster clsCluster)
        {
            string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();
            try
            {
                
                    using (SqlConnection connection = new SqlConnection(dbConn))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("SP_EditarCluster", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@IdCluster", clsCluster.IdCluster);
                            command.Parameters.AddWithValue("@NombreCluster", clsCluster.NombreCluster);
                            command.Parameters.AddWithValue("@Activo", clsCluster.Activo);
                            command.Parameters.AddWithValue("@Usuario_FUM", Convert.ToInt32(currentUser));
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                        ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Registro actualizado con éxito.");
                        return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
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
