using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using OXXO.Models;
using OXXO.Enums;
using OXXO.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace OXXO.Controllers
{
    public class UsuariosController : Controller
    {

        string dbConn = "";
        public IConfiguration Configuration { get; }
        public UsuariosController(IConfiguration configuration) 
        {
            Configuration = configuration;
            dbConn = Configuration["ConnectionStrings:ConexionString"];
            
        }
        public IActionResult Index(string? alert, string Nombre, string UserName)
        {
            ViewBag.Alert = alert;

            string  puestoUser = HttpContext.Session.GetString("IdPerfil");
            string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();

            Permisos permisos = new Permisos();
            try
            {
                //ListadoDePerfiles();
                string consulta = "";
                if (!String.IsNullOrEmpty(Nombre))
                {
                    consulta = "SELECT * FROM Usuario WHERE Nombre LIKE '%" + Nombre + "%'";

                }
                else if (!String.IsNullOrEmpty(UserName))
                {
                    consulta = "SELECT * FROM Usuario WHERE UserName LIKE '%" + UserName + "%'";
                }
                else
                {
                    consulta = "SELECT * FROM usuario";
                }
                List<Usuario> ListaUsuarios = new List<Usuario>();
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
                                Usuario clsUsuario = new Usuario();
                                clsUsuario.IdUsuario = Convert.ToString(dr["IdUsuario"]);
                                clsUsuario.Nombre = Convert.ToString(dr["Nombre"]);
                                clsUsuario.Apellido = Convert.ToString(dr["Apellido"]);
                                clsUsuario.UserName = Convert.ToString(dr["UserName"]);
                                clsUsuario.Correo = Convert.ToString(dr["Correo"]);
                                clsUsuario.Activo = dr.IsDBNull("Activo") ? false : Convert.ToBoolean(dr["Activo"]);
                                clsUsuario.Vigencia = Convert.ToDateTime(dr["Vigencia"]);
                                clsUsuario.Puesto = Convert.ToString(dr["Puesto"]);
                                ListaUsuarios.Add(clsUsuario);

                            }
                        }
                        connection.Close();
                    }
                    var res = new PermisosController(Configuration).GetPermisosUsuario("Index", "Usuarios", puestoUser);
                    ViewBag.Crear = res.Crear;
                    ViewBag.Editar = res.Editar;

                    if (puestoUser == "1")
                    {
                        ViewBag.CambiarContrasena = true;
                    }
                    else
                    {
                        ViewBag.CambiarContrasena = false;
                    }
                    ViewBag.User = currentUser;

                    return View(ListaUsuarios);
                }
                else
                {
                    var res = new PermisosController(Configuration).GetPermisosUsuario("Index", "Usuarios", puestoUser);
                    ViewBag.Crear = res.Crear;
                    ViewBag.Editar = res.Editar;
                    return View(ListaUsuarios);
                }
               
            }
            catch (Exception ex)
            {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });

            }

        }

       public object ListadoDePerfiles() 
       {
            List<Perfil> ListaPerfiles = new List<Perfil>();
            using (SqlConnection connection = new SqlConnection(dbConn))
            {
                connection.Open();

                string consulta = "SELECT * FROM Perfil WHERE Activo = 1";
                SqlCommand command = new SqlCommand(consulta,connection);
                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Perfil clsPerfil = new Perfil();
                        clsPerfil.IdPerfil = Convert.ToString(dr["IdPerfil"]);
                        clsPerfil.Nombre = Convert.ToString(dr["Nombre"]);

                        ListaPerfiles.Add(clsPerfil);
                    }
                }
                ViewData["Perfiles"] = new SelectList(ListaPerfiles.ToList(), "IdPerfil", "Nombre");
                connection.Close();

                return ViewData["Perfiles"];
            }
       }

        public bool ValidarNumero(string username, string? iduser) 
        {
            try
            {
                using (SqlConnection connection= new SqlConnection(dbConn))
                {
                    string consulta = $"SELECT * FROM Usuario WHERE UserName = '{username}'";
                    if (!String.IsNullOrEmpty(iduser))
                    {
                        consulta += "AND IdUsuario <> " + iduser;
                    }

                    SqlCommand command = new SqlCommand(consulta,connection);
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr.HasRows)
                            {
                                connection.Close();
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        connection.Close();
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return false;
            }
        }

        [HttpGet]
        public IActionResult Crear(string? alert) { ViewBag.Alert = alert; ListadoDePerfiles(); return PartialView("Crear"); }

        [HttpPost]
        public IActionResult Crear(Usuario clsUsuario) 
        {
            string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    if (ValidarNumero(clsUsuario.UserName,null)==true)
                    {
                        ModelState.AddModelError("UserName","Este usuario ya existe.");
                        ListadoDePerfiles();
                        ViewBag.Alert = CommonServices.ShowAlert(Alerts.Warning, "Este usuario ya existe, por favor intenta con otro.");
                        return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
                    }

                    using (SqlConnection connection = new SqlConnection(dbConn))
                    {
                        string contrasenaEncriptada = Usuario.GetMD5Hash(clsUsuario.Contrasena);
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("SP_CrearUsuario",connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Nombre", clsUsuario.Nombre);
                            command.Parameters.AddWithValue("@Apellido", clsUsuario.Apellido);
                            command.Parameters.AddWithValue("@Contrasena", contrasenaEncriptada);
                            command.Parameters.AddWithValue("@UserName", clsUsuario.UserName);
                            command.Parameters.AddWithValue("@Correo",clsUsuario.Correo);
                            command.Parameters.AddWithValue("@Puesto", clsUsuario.Puesto);
                            command.Parameters.AddWithValue("@Vigencia", Convert.ToDateTime(clsUsuario.Vigencia).ToString("yyyy-MM-dd HH:mm:ss"));
                            command.Parameters.AddWithValue("@IdUsuarioFA", currentUser);
                            command.Parameters.AddWithValue("IdPerfil", clsUsuario.IdPerfil);
                            command.ExecuteNonQuery();
                            
                        }
                        connection.Close();
                        ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Registro completado con éxito.");
                        return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
                    }
                }
                else
                {
                    ListadoDePerfiles();
                    return View();
                }
            }
            catch (Exception ex)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
        }


        [HttpGet]
        public IActionResult Editar(int Id)
        {
            Usuario clsUsuario = new Usuario();
            try
            {
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    string consulta = $"SELECT * FROM Usuario WHERE IdUsuario='{Id}'";
                    SqlCommand command = new SqlCommand(consulta, connection);

                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            clsUsuario.IdUsuario = Convert.ToString(dr["IdUsuario"]);
                            clsUsuario.Nombre = Convert.ToString(dr["Nombre"]);
                            clsUsuario.Apellido = Convert.ToString(dr["Apellido"]);
                            clsUsuario.UserName = Convert.ToString(dr["UserName"]);
                            clsUsuario.Contrasena = Convert.ToString(dr["Contrasena"]);
                            clsUsuario.ConfirmarContrasena = Convert.ToString(dr["Contrasena"]);
                            clsUsuario.Correo = Convert.ToString(dr["Correo"]);
                            clsUsuario.Activo = dr.IsDBNull("Activo") ? false : Convert.ToBoolean(dr["Activo"]);
                            clsUsuario.Vigencia = Convert.ToDateTime(dr["Vigencia"]);
                            clsUsuario.Puesto = Convert.ToString(dr["Puesto"]);
                            clsUsuario.IdPerfil = Convert.ToString(dr["IdPerfil"]);
                        }
                    }
                    connection.Close();
                }
                ListadoDePerfiles();
                return PartialView("Editar", clsUsuario);
            }
            catch (Exception ex)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
    
        }

        [HttpPost]
        [ActionName("Editar")]
        public IActionResult Editar(Usuario clsUsuario)
        {
            string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    if (ValidarNumero(clsUsuario.UserName, clsUsuario.IdUsuario)==true)
                    {
                        ModelState.AddModelError("UserName","Este usuario ya existe.");
                        ListadoDePerfiles();
                        return View();
                    }

                    using (SqlConnection connection = new SqlConnection(dbConn))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("SP_EditarUsuarios",connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@IdUsuario",clsUsuario.IdUsuario);
                            command.Parameters.AddWithValue("@Nombre", clsUsuario.Nombre);
                            command.Parameters.AddWithValue("@Apellido", clsUsuario.Apellido);
                            command.Parameters.AddWithValue("@UserName", clsUsuario.UserName);
                            command.Parameters.AddWithValue("@Correo", clsUsuario.Correo);
                            command.Parameters.AddWithValue("@Puesto", clsUsuario.Puesto);
                            command.Parameters.AddWithValue("@Vigencia", Convert.ToDateTime(clsUsuario.Vigencia).ToString("yyyy-MM-dd HH:mm:ss"));
                            command.Parameters.AddWithValue("@IdUsuarioFUM", currentUser);
                            command.Parameters.AddWithValue("@Activo", Convert.ToInt32(clsUsuario.Activo));
                            command.Parameters.AddWithValue("IdPerfil", clsUsuario.IdPerfil);
                            command.ExecuteNonQuery();
                            connection.Close();

                        }
                        
                        ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Registro actualizado con éxito.");
                        return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert});
                    }
                }
                else
                {
                    ListadoDePerfiles();
                    return View();
                }
            }
            catch (Exception ex)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
        }

        [HttpGet]
        public IActionResult CambiarContrasena(int Id)
        {
            Usuario clsUsuario = new Usuario();
            if (ModelState.IsValid)
            {
              
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    string consulta = $"SELECT * FROM Usuario WHERE IdUsuario='{Id}'";
                    SqlCommand command = new SqlCommand(consulta, connection);
                    connection.Open();
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            clsUsuario.IdUsuario = Convert.ToString(dr["IdUsuario"]);
                            clsUsuario.Nombre = Convert.ToString(dr["Nombre"]);
                            clsUsuario.Apellido = Convert.ToString(dr["Apellido"]);
                            clsUsuario.UserName = Convert.ToString(dr["UserName"]);
                            clsUsuario.Correo = Convert.ToString(dr["Correo"]);
                            clsUsuario.Activo = dr.IsDBNull("Activo") ? false : Convert.ToBoolean(dr["Activo"]);
                            clsUsuario.Vigencia = Convert.ToDateTime(dr["Vigencia"]);
                            clsUsuario.Puesto = Convert.ToString(dr["Puesto"]);
                            clsUsuario.IdPerfil = Convert.ToString(dr["IdPerfil"]);
                        }
                    }
                    connection.Close();
                }

                ModelState.Clear();
               
            }

            
            return PartialView("_CambiarContrasena", clsUsuario);

        }

        [HttpPost]
        [ActionName("CambiarContrasena")]
        public IActionResult CambiarContrasena(Usuario clsUsuario) 
        {
            try
            {
                if (ModelState["Contrasena"].Errors.Count == 0 && ModelState["ConfirmarContrasena"].Errors.Count == 0)
                {
                    string contrasenaEncriptada = Usuario.GetMD5Hash(clsUsuario.Contrasena);
                    using (SqlConnection connection = new SqlConnection())
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("SP_CambiarContrasena", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@IdUsuario", clsUsuario.IdUsuario);
                            command.Parameters.AddWithValue("@Contrasena", contrasenaEncriptada);
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                        ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Contraseña actualizada con éxito.");
                        return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });


                    }
                }
                else
                {
                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Las contraseñas no coinciden.");
                    return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });

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
