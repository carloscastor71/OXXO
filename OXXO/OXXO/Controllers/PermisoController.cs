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
            //ListadoControladores();
            ListadoPerfiles();
            return View();
        }

        public object ListadoPerfiles() 
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

        


    }
}
