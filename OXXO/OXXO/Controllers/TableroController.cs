using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OXXO.Models;
using OXXO.Enums;
using OXXO.Services;
using System.Net;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;

namespace OXXO.Controllers
{
    public class TableroController : Controller
    {
        string dbConn = "";
        public IConfiguration Configuration { get; }

        public TableroController(IConfiguration configuration)
        {
            Configuration = configuration;
            dbConn = Configuration["ConnectionStrings:ConexionString"];
        }
        //public IActionResult Index()
        //{

        //    return View();
        //}

        #region "Consumo de una API"

        public async Task<IActionResult> Index(string? alert)
        {
            ViewBag.Alert = alert;
            List<Tablero> tableroList= new List<Tablero>();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://retoolapi.dev/j7UoBz/data"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        tableroList = JsonConvert.DeserializeObject<List<Tablero>>(apiResponse);
                    }
                }

                return View(tableroList);
            }
            catch (Exception)
            {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "No se pudo consultar la información.");
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
                
            }
        
            
        }
        #endregion
        //método para obtener los datos de SQL
        protected string obtenerDatos(string consulta)
        {
            consulta = "SELECT * FROM Usuario";
            DataTable datos = new DataTable();
            using (SqlConnection connection = new SqlConnection(dbConn))
            {
               
                connection.Open();
                SqlCommand command = new SqlCommand(consulta, connection);
                using (SqlDataReader dr = command.ExecuteReader())
                {
                    
                    datos.Load(command.ExecuteReader());
                }
                connection.Close();
            }

            string strDatos;
            strDatos = "[['number','Dias'],['number','Fondos'],";

            foreach (DataRow dr in datos.Rows)
            {
                strDatos = strDatos + "[";
                strDatos = strDatos + "'" + dr[0] + "'" + "," + dr[1];
                strDatos = strDatos + "],";
            }
            strDatos = strDatos + "]";

            return strDatos;
        }
    }
}
