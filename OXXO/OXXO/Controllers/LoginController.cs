using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using OXXO.Models;
using OXXO.Services;
using OXXO.Enums;


namespace OXXO.Controllers
{
    public class LoginController : Controller
    {
        string dbConn = "";
        public IConfiguration Configuration { get; }
        
        public LoginController(IConfiguration configuration)
        {
            Configuration = configuration;
            dbConn = Configuration["ConnectionStrings:ConexionString"];
        }
        public IActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(string? alert)
        {
            ViewBag.Alert = alert;
            return View();
        }

       [HttpPost]
       public IActionResult Login(Login lg)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    if (existeEmpleado(lg.UserName) == true || existeContraseña(lg.Contrasena) == true)
                    {
                        ViewBag.Alert = CommonServices.ShowAlert(Alerts.Info, "Usuario y/o contraseña incorrectas, favor de ingresarlas correctamente.");
                        return View();
                    }
                    else
                    {
                        using (SqlConnection sqlCon = new SqlConnection(dbConn))
                        {
                            sqlCon.Open();
                            string query = "Select * from Usuario where UserName = @UserName AND Contrasena = @Contrasena";
                            SqlCommand comm = new SqlCommand(query, sqlCon);
                            comm.Parameters.AddWithValue("@UserName", lg.UserName);
                            // string pass = Usuario.GetMD5Hash(lg.Contrasena);
                            // comm.Parameters.AddWithValue("@Contrasena",pass);
                            comm.Parameters.AddWithValue("@Contrasena", lg.Contrasena);
                            SqlDataReader sdr = comm.ExecuteReader();
                            if (sdr.Read())
                            {
                                //si existe el usuario verifica si está
                                if (DateTime.Now > sdr.GetDateTime(8))
                                {
                                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Info, "Este usuario no esta vigente.");
                                    return View();
                                }
                                else
                                {
                                    HttpContext.Session.SetString("IdUsuario", sdr.GetInt32(0).ToString());
                                    HttpContext.Session.SetString("Nombre", sdr.GetString(1) + " " + sdr.GetString(2));
                                    HttpContext.Session.SetString("UserName", sdr.GetString(3));
                                    HttpContext.Session.SetString("Correo", sdr.GetString(5));
                                    HttpContext.Session.SetString("Puesto", sdr.GetString(6));
                                    HttpContext.Session.SetString("IdPerfil", sdr.GetInt32(9).ToString());
                                    sqlCon.Close();
                                    return RedirectToAction("Home", "Home");
                                }
                            }
                            return View();
                        }
                    }
                }
                catch (Exception ex)
                {

                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                    return View();
                }

            }
            else
            {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Warning, "Favor de ingresar el usuario y contraseña");
            }
            return View();
        }

        public bool existeEmpleado(string UserName)
        {
            string sql = @"SELECT COUNT(*) FROM Usuario WHERE UserName = @UserName";

            using (SqlConnection conn = new SqlConnection(dbConn))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserName", UserName);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count == 0;

            }
        }

        public bool existeContraseña(string password)
        {
            string sql = @"SELECT COUNT(*) FROM Usuario WHERE Contrasena = @Contrasena";

            using (SqlConnection conn = new SqlConnection(dbConn))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                //string pass = Usuario.GetMD5Hash(password);
                cmd.Parameters.AddWithValue("@Contrasena", password);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count == 0;

            }
        }



        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Sesión cerrada correctamente");
            return RedirectToAction(nameof(Login), new { alert = ViewBag.Alert });
        }

    }
}
