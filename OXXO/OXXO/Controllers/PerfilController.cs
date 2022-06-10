using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OXXO.Enums;
using OXXO.Models;
using OXXO.Services;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace OXXO.Controllers
{
    public class PerfilController : Controller
    {
        string dbConn = "";
        public IConfiguration Configuration { get; }

        public PerfilController(IConfiguration configuration)
        {
            Configuration = configuration;
            dbConn = Configuration["ConnectionStrings:ConexionString"];
        }
        public IActionResult Index(string? alert, string NombrePerfil)
        {
            string consultaP = "";
            if (!string.IsNullOrEmpty(NombrePerfil))
            {
                consultaP = "SELECT IdPerfil,Nombre,Descripcion,Activo,FechjaAlta,FechaUltimaMod,IdUsuarioFA,IdUsuarioFUM FROM Perfil where Nombre LIKE '%" + NombrePerfil + "%'";

            }
            else
            {
                consultaP = "SELECT IdPerfil,Nombre,Descripcion,Activo,FechjaAlta,FechaUltimaMod,IdUsuarioFA,IdUsuarioFUM FROM Perfil";
            }

            ViewBag.Alert = alert;
            List<Perfil> PerfilList = new List<Perfil>();
            using (SqlConnection connection = new SqlConnection(dbConn))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(consultaP, connection);
                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Perfil clsPerfil = new Perfil();
                        clsPerfil.IdPerfil = Convert.ToString(dr["IdPerfil"]);
                        clsPerfil.Nombre = Convert.ToString(dr["Nombre"]);
                        clsPerfil.Descripcion = Convert.ToString(dr["Descripcion"]);
                        clsPerfil.Activo = Convert.ToBoolean(dr["Activo"]);
                        PerfilList.Add(clsPerfil);
                    }

                }
                connection.Close();
            }

            string PuestoUsuario = HttpContext.Session.GetString("IdPerfil");
           // var result = new PermisoController(Configuration).GetPermisoUsuario("Index","Perfil", PuestoUsuario);
            //ViewBag.Crear = result.Crear;
            //ViewBag.Editar = result.Editar;
            

            return View(PerfilList);
        }

        public IActionResult Crear() { return View(); }

        [HttpPost]
        public IActionResult Crear(Perfil clsPerfil)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string currentUser = HttpContext.Session.GetInt32("IdUsuario").ToString();

                    using (SqlConnection connection = new SqlConnection(dbConn))
                    {
                        connection.Open();
                        using (SqlCommand command= new SqlCommand("SP_CrearPerfil", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Nombre",clsPerfil.Nombre);
                            command.Parameters.AddWithValue("Descripcion",clsPerfil.Descripcion);
                            command.Parameters.AddWithValue("IdUsuarioFA", currentUser);
                            command.ExecuteNonQuery();
                            connection.Close();
                        }

                        ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Registro Completado con éxito.");
                        return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex )
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert});
            }
           
        }

        ///PENDIENTE EDIT

        public IActionResult Editar(int IdPerfil) 
        {
            Perfil clsPerfil = new Perfil();
            try
            {
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    String consulta = $"SELECT * FROM Perfil WHERE IdPerfil = '{IdPerfil}'";
                    SqlCommand command = new SqlCommand(consulta,connection);
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            clsPerfil.IdPerfil = Convert.ToString(dr["IdPerfil"]);
                            clsPerfil.Nombre = Convert.ToString(dr["Nombre"]);
                            clsPerfil.Descripcion = Convert.ToString(dr["Descripcion"]);
                            clsPerfil.Activo = Convert.ToBoolean(dr["Activo"]);
                        }
                    }
                    connection.Close();
            }
                return View(clsPerfil);
            }
            catch (Exception ex)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }

            
        }

        [HttpPost]
        [ActionName("Editar")]
        public IActionResult Editar(Perfil clsPefil) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string currentUser = HttpContext.Session.GetInt32("IdUsuario").ToString();
                    using (SqlConnection connection = new SqlConnection())
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("SP_EditarPerfil", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("IdPerfil",clsPefil.IdPerfil);
                            command.Parameters.AddWithValue("Nombre",clsPefil.Nombre);
                            command.Parameters.AddWithValue("Descripcion",clsPefil.Descripcion);
                            command.Parameters.AddWithValue("Activo", Convert.ToInt32(clsPefil.Activo));
                            command.Parameters.AddWithValue("IdUsuarioFUM", currentUser);
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                        ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Registro actualizado con éxito.");
                        return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert});
                    }

                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
        }


    }
}
