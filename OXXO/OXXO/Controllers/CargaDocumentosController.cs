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
    public class CargaDocumentosController : Controller
    {


        public IConfiguration Configuration { get; }

        public CargaDocumentosController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private readonly ILogger<HomeController> _logger;

        public IActionResult Index(string? alert)
        {
            //ViewBag.Alert = alert;
            //ListadoBancos();
            //ListadoGiroComercial();
            return View();
        }

        //[HttpPost]
        //public IActionResult Index(string Rfc, string NombreCompleto, string Telefono, string Correo, string Direccion, string CuentaDeposito, int IdBanco, string RazonSocial, string NombreComercial, int IdGiroComercio, string Portal, int Persona, int Usuario_FAl, int Usuario_FUM, int IdTipoDeposito)
        //{
        //    int IdPerfil = Int32.Parse(HttpContext.Session.GetString("IdPerfil"));
        //    if (String.IsNullOrEmpty(Rfc)|| String.IsNullOrEmpty(Telefono) || String.IsNullOrEmpty(Correo) || String.IsNullOrEmpty(Direccion) || String.IsNullOrEmpty(CuentaDeposito) || String.IsNullOrEmpty(IdBanco.ToString()) || String.IsNullOrEmpty(IdTipoDeposito.ToString()))
        //    {
        //        ViewBag.Alert = CommonServices.ShowAlert(Alerts.Warning, "Tienes campos sin llenar.");
        //        return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
        //    }
        //    else
        //    {
        //        try
        //        {
        //            string connectionString = Configuration["ConnectionStrings:ConexionString"];
        //            using SqlConnection connection = new SqlConnection(connectionString);

        //            connection.Open();
        //            Guid RFC = Guid.NewGuid();

        //            using SqlCommand command = new SqlCommand("SP_ComercioAlta", connection);

        //            command.CommandType = CommandType.StoredProcedure;

        //            command.Parameters.AddWithValue("@RFC", Rfc);
        //            command.Parameters.AddWithValue("@NombreCompleto", NombreCompleto);
        //            command.Parameters.AddWithValue("@Telefono", Telefono);
        //            command.Parameters.AddWithValue("@Correo", Correo);
        //            command.Parameters.AddWithValue("@Direccion", Direccion);
        //            command.Parameters.AddWithValue("@CuentaDeposito", CuentaDeposito);
        //            command.Parameters.AddWithValue("@IdBanco", IdBanco);
        //            command.Parameters.AddWithValue("@RazonSocial", RazonSocial);
        //            command.Parameters.AddWithValue("@NombreComercial", NombreComercial);
        //            command.Parameters.AddWithValue("@IdGiroComercio", IdGiroComercio);
        //            command.Parameters.AddWithValue("@Portal", Portal);
        //            command.Parameters.AddWithValue("@Persona", Persona);
        //            command.Parameters.AddWithValue("@Usuario_FAl", IdPerfil);
        //            command.Parameters.AddWithValue("@Usuario_FUM", IdPerfil);
        //            command.Parameters.AddWithValue("@IdTipoDeposito", IdTipoDeposito);

        //            command.ExecuteNonQuery();
        //            connection.Close();

        //            ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Comercio dado de alta correctamente");

        //            return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });

        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
        //            return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
        //        }
        //    }
        //}

        //public object ListadoBancos()
        //{
        //    List<Banco> BancoList = new List<Banco>();

        //    string connectionString = Configuration["ConnectionStrings:ConexionString"];

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        using (SqlCommand command = new SqlCommand("SP_ConsultaBanco", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            SqlDataReader dataReader = command.ExecuteReader();
        //            while (dataReader.Read())
        //            {
        //                Banco banco = new Banco
        //                {
        //                    IdBanco = Convert.ToInt32(dataReader["IdBanco"]),
        //                    Bancos = Convert.ToString(dataReader["Bancos"])
        //                };

        //                BancoList.Add(banco);
        //            }
        //        }

        //        ViewData["Bancos"] = new SelectList(BancoList.ToList(), "IdBanco", "Bancos");
        //        connection.Close();

        //        return ViewData["Bancos"];
        //    }
        //}
        //public object ListadoGiroComercial()
        //{
        //    List<GiroComercio> GiroComercioList = new List<GiroComercio>();

        //    string connectionString = Configuration["ConnectionStrings:ConexionString"];

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        using (SqlCommand command = new SqlCommand("SP_ConsultaGiroComercio", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            SqlDataReader dataReader = command.ExecuteReader();
        //            while (dataReader.Read())
        //            {
        //                GiroComercio giroComercio = new GiroComercio
        //                {
        //                    IdGiroComercio = Convert.ToInt32(dataReader["IdGiroComercio"]),
        //                    GiroComercial = Convert.ToString(dataReader["GiroComercial"])
        //                };

        //                GiroComercioList.Add(giroComercio);
        //            }
        //        }

        //        ViewData["GiroComercio"] = new SelectList(GiroComercioList.ToList(), "IdGiroComercio", "GiroComercial");
        //        connection.Close();

        //        return ViewData["GiroComercio"];
        //    }
        //}

        //public object ListadoEstatus()
        //{
        //    List<Estatus> EstatusList = new List<Estatus>();

        //    string connectionString = Configuration["ConnectionStrings:ConexionString"];

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        using (SqlCommand command = new SqlCommand("SP_Estatus", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            SqlDataReader dataReader = command.ExecuteReader();
        //            while (dataReader.Read())
        //            {
        //                Estatus estatus = new Estatus
        //                {
        //                    IdEstatus = Convert.ToInt32(dataReader["IdEstatus"]),
        //                    Estatus1 = Convert.ToString(dataReader["Estatus"])
        //                };
        //                EstatusList.Add(estatus);
        //            }
        //        }

        //        ViewData["Estatus"] = new SelectList(EstatusList.ToList(), "IdEstatus", "Estatus");
        //        connection.Close();

        //        return ViewData["Estatus"];
        //    }
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


    