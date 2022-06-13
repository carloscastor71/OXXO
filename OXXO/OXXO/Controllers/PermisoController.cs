using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OXXO.Models;
using OXXO.Enums;
using OXXO.Services;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace OXXO.Controllers
{
    public class PermisoController : Controller
    {
        string dbConn = "";
        public IConfiguration Configuration { get; }

        public PermisoController(IConfiguration configuration) 
        {
            Configuration = configuration;
            dbConn = Configuration["ConnectionStrings:ConexionString"];
        }
        public IActionResult Index(string? alert)
        {
            ViewBag.Alert = alert;
            ListasControladores();
            ListasPerfiles();
            return View();
        }

        public object ListasPerfiles() 
        {
            List<Perfil> PerfilList = new List<Perfil>();
            try
            {
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT IdPerfil, Nombre FROM Perfil WHERE Activo = 1", connection))
                    {
                        SqlDataReader dr = command.ExecuteReader();
                        while (dr.Read())
                        {
                            Perfil clsPerfil = new Perfil();
                            clsPerfil.IdPerfil = Convert.ToString(dr["IdPerfil"]);
                            clsPerfil.Nombre = Convert.ToString(dr["Nombre"]);

                            PerfilList.Add(clsPerfil);
                        }
                    }

                    ViewData["Perfil"] = new SelectList(PerfilList.ToList(), "IdPerfil", "Nombre");
                    connection.Close();

                    return ViewData["Perfil"];
                }
            }
            catch (Exception ex)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }

        }

        public object ListasControladores()
        {
            List<Controlador> ControladoresList = new List<Controlador>();
            using (SqlConnection connection = new SqlConnection(dbConn))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT IdControlador, Texto FROM Controlador", connection))
                {
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        Controlador clsControlador = new Controlador();
                        clsControlador.IdControlador = Convert.ToInt32(dr["IdControlador"]);
                        clsControlador.NombreControlador = Convert.ToString(dr["Texto"]);

                        ControladoresList.Add(clsControlador);
                    }
                }

                ViewData["Controlador"] = new SelectList(ControladoresList.ToList(), "IdControlador","NombreControlador");
                connection.Close();
                return ViewData["Controlador"];
            }

        }

        public JsonResult ListadoAcciones(int IdControlador) 
        {
            List<SelectListItem> ActionList = new List<SelectListItem>();
            using (SqlConnection connection = new SqlConnection(dbConn))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT IdAccion, Encabezado FROM AccionControlador WHERE IdControlador ="+IdControlador, connection))
                {
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        ActionList.Add(new SelectListItem { Text = dr["Encabezado"].ToString(), Value = dr["IdAccion"].ToString()});
                    }
                }
                connection.Close();
                return Json(ActionList);
            }
        }

        [HttpPost]
        public IActionResult GuardarPermiso(int IdRol, int IdControlador, int IdAcciones, string leer, string crear, string editar) 
        {
            try
            {
                bool PuedeLeer = (leer ?? "").Equals("on", StringComparison.CurrentCultureIgnoreCase);
                bool PuedeCrear = (crear ?? "").Equals("on", StringComparison.CurrentCultureIgnoreCase);
                bool PuedeEditar = (editar ?? "").Equals("on", StringComparison.CurrentCultureIgnoreCase);

                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SP_CrearActualizarPermisos", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdRol", IdRol);
                        command.Parameters.AddWithValue("@IdControlador",IdControlador);
                        command.Parameters.AddWithValue("@IdAccion", IdAcciones);
                        command.Parameters.AddWithValue("@PuedeLeer", PuedeLeer);
                        command.Parameters.AddWithValue("@PuedeCrear", PuedeCrear);
                        command.Parameters.AddWithValue("@PuedeEditar", PuedeEditar);

                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Permiso creado con éxito.");
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert});
            }
            catch (Exception ex)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert});
            }
        }

        public JsonResult GetPermisos(string IdRol, string IdControlador, string IdAccion) 
        {
            Permisos permiso = new Permisos();
            using (SqlConnection connection = new SqlConnection(dbConn))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT Leer, Crear, Editar FROM RolControlador WHERE IdRol="+IdRol+" AND IdControlador="+IdControlador+" AND IdAccion="+IdAccion,connection))
                {
                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        permiso.Leer = Convert.ToBoolean(dr["Leer"]);
                        permiso.Crear = Convert.ToBoolean(dr["Crear"]);
                        permiso.Editar = Convert.ToBoolean(dr["Editar"]);
                    }
                }
            }
            return Json(permiso);
        }

        public PermisosUsuario GetPermisosUsuario(string NombreAccion, string NombreControlador, string IdRol) 
        {
            PermisosUsuario userPermiso = new PermisosUsuario();
            if (IdRol != null)
            {
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    connection.Open();
                    using (SqlCommand command= new SqlCommand("SP_BuscaPermisoUsuario", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdRol", IdRol);
                        command.Parameters.AddWithValue("@NombreControlador", NombreControlador);
                        command.Parameters.AddWithValue("@NombreAccion", NombreAccion);

                        SqlDataReader dr = command.ExecuteReader();
                        while (dr.Read())
                        {
                            userPermiso.Crear = Convert.ToBoolean(dr["Crear"]);
                            userPermiso.Editar = Convert.ToBoolean(dr["Editar"]);
                            userPermiso.Leer = Convert.ToBoolean(dr["Leer"]);
                        }
                    }
                    connection.Close();
                }
            }
            return userPermiso;
        }


    }
}
