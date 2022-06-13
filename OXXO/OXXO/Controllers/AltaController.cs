using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OXXO.Enums;
using OXXO.Models;
using OXXO.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OXXO.Controllers
{
    public class AltaController : Controller
    {


        public IConfiguration Configuration { get; }

        public AltaController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private readonly ILogger<HomeController> _logger;

        //public ComercioController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            ListadoBancos();
            //ListadoEstatus();
            ListadoGiroComercial();
            return View();
        }


        //public IActionResult Create()
        //{
        //    ListadoBancos();
        //    //ListadoEstatus();
        //    ListadoGiroComercial();
        //    return View();
        //}

        [HttpPost]
        public IActionResult Index( int IdEmisor,string Rfc, string NombreCompleto, string Telefono, string Correo, string Direccion, string CuentaDeposito, int IdBanco , string RazonSocial, string NombreComercial, int IdGiroComercio, string Portal, int Persona, int Usuario_FAl, int Usuario_FUM, int IdTipoDeposito)
        {
            string IdPerfil = HttpContext.Session.GetString("IdPerfil");
            try
            {
               
                    string connectionString = Configuration["ConnectionStrings:ConexionString"];
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        Guid RFC = Guid.NewGuid();
                        string prOutput = "";
                        using (SqlCommand command = new SqlCommand("SP_ComercioAlta", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@IdEmisor", IdEmisor);
                            command.Parameters.AddWithValue("@RFC", Rfc);
                            command.Parameters.AddWithValue("@NombreCompleto", NombreCompleto);
                            command.Parameters.AddWithValue("@Telefono", Telefono);
                            command.Parameters.AddWithValue("@Correo", Correo);
                            command.Parameters.AddWithValue("@Direccion", Direccion);
                            command.Parameters.AddWithValue("@CuentaDeposito", CuentaDeposito);
                            command.Parameters.AddWithValue("@IdBanco", IdBanco);
                            command.Parameters.AddWithValue("@RazonSocial", RazonSocial);
                            command.Parameters.AddWithValue("@NombreComercial", NombreComercial);
                            command.Parameters.AddWithValue("@IdGiroComercio", IdGiroComercio);
                            command.Parameters.AddWithValue("@Portal", Portal);
                            command.Parameters.AddWithValue("@Persona",Persona);
                            command.Parameters.AddWithValue("@Usuario_FAl", IdPerfil);
                            command.Parameters.AddWithValue("@Usuario_FUM", IdPerfil);
                            command.Parameters.AddWithValue("@IdTipoDeposito", IdTipoDeposito);
                            command.ExecuteNonQuery();
                            connection.Close();
                            ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "El comercio se ha dado de alta correctamente.");
                            return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
                        }
                    }
           
            }
            catch (Exception ex)
            {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
           
        }

        public object ListadoBancos()
        {
            List<Banco> BancoList = new List<Banco>();
            string connectionString = Configuration["ConnectionStrings:ConexionString"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SP_ConsultaBanco", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Banco banco = new Banco();
                        banco.IdBanco = Convert.ToInt32(dataReader["IdBanco"]);
                        banco.Bancos = Convert.ToString(dataReader["Bancos"]);

                        BancoList.Add(banco);
                    }
                }
                ViewData["Bancos"] = new SelectList(BancoList.ToList(), "IdBanco", "Bancos");
                connection.Close();

                return ViewData["Bancos"];
            }

        }
        public object ListadoGiroComercial()
        {
            List<GiroComercio> GiroComercioList = new List<GiroComercio>();
            string connectionString = Configuration["ConnectionStrings:ConexionString"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SP_ConsultaGiroComercio", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        GiroComercio giroComercio = new GiroComercio();
                        giroComercio.IdGiroComercio = Convert.ToInt32(dataReader["IdGiroComercio"]);
                        giroComercio.GiroComercial = Convert.ToString(dataReader["GiroComercial"]);

                        GiroComercioList.Add(giroComercio);
                    }
                }
                ViewData["GiroComercio"] = new SelectList(GiroComercioList.ToList(), "IdGiroComercio", "GiroComercial");
                connection.Close();

                return ViewData["GiroComercio"];
            }

        }

        public object ListadoEstatus()
        {
            List<Estatus> EstatusList = new List<Estatus>();
            string connectionString = Configuration["ConnectionStrings:ConexionString"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SP_Estatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Estatus estatus = new Estatus();
                        estatus.IdEstatus = Convert.ToInt32(dataReader["IdEstatus"]);
                        estatus.Estatus1 = Convert.ToString(dataReader["Estatus"]);

                        EstatusList.Add(estatus);
                    }
                }
                ViewData["Estatus"] = new SelectList(EstatusList.ToList(), "IdEstatus", "Estatus");
                connection.Close();

                return ViewData["Estatus"];
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


    