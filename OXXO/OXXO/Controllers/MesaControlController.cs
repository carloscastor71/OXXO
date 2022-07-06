using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OXXO.Enums;
using OXXO.Models;
using OXXO.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OXXO.Controllers
{
    public class MesaControlController : Controller
    {
        // GET: CategorizacionController

        string dbConn = "";

        public IConfiguration Configuration { get; }

        public MesaControlController(IConfiguration configuration)
        {
            Configuration = configuration;
            dbConn = Configuration["ConnectionStrings:ConexionString"];

        }

        //Metodo que devuelve la vista
        public ActionResult Index(string? alert)
        {
            ViewBag.Alert = alert;
            return View();
        }


        public ActionResult Editar(string RFC, string? alert)
        {
            ListadoBancos();
            ListadoGiroComercial();
            ViewBag.Alert = alert;
            Comercio clsComercio = new Comercio();
            try
            {
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    if (String.IsNullOrEmpty(RFC))
                    {
                        RFC = HttpContext.Session.GetString("RFC");

                    }
                    HttpContext.Session.SetString("RFC", RFC);

                    string consulta = string.Format("exec SP_SelectComercio {0}", RFC);

                    SqlCommand command = new SqlCommand(consulta, connection);

                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            clsComercio.IdComercio = Convert.ToInt32(dr["IdComercio"]);
                            clsComercio.IdEmisor = dr.IsDBNull("IdEmisor") ? 0 : Convert.ToInt32(dr["IdEmisor"]);
                            clsComercio.RFC = Convert.ToString(dr["RFC"]);
                            clsComercio.NombreCompleto = Convert.ToString(dr["NombreCompleto"]);
                            clsComercio.Telefono = Convert.ToString(dr["Telefono"]);
                            clsComercio.Correo = Convert.ToString(dr["Correo"]);
                            clsComercio.Direccion = Convert.ToString(dr["Direccion"]);
                            clsComercio.CuentaDeposito = Convert.ToString(dr["CuentaDeposito"]);
                            clsComercio.Banco = Convert.ToString(dr["Banco"]);
                            clsComercio.RazonSocial = Convert.ToString(dr["RazonSocial"]);
                            clsComercio.NombreComercial = Convert.ToString(dr["NombreComercial"]);
                            clsComercio.GiroComercio = Convert.ToString(dr["GiroComercial"]);
                            clsComercio.Portal = Convert.ToString(dr["Portal"]);
                            clsComercio.PersonaMoral = Convert.ToInt32(dr["PersonaMoral"]);
                            clsComercio.PersonaFisica = Convert.ToInt32(dr["PersonaFisica"]);
                            clsComercio.Estatus = Convert.ToString(dr["Estatus"]);
                            clsComercio.Activo = Convert.ToInt32(dr["Activo"]);
                            clsComercio.IdCompania = Convert.ToString(dr["Compania"]);
                            clsComercio.IdTipoDeposito = Convert.ToString(dr["TipoDeposito"]);

                        }
                    }
                    connection.Close();
                }

                return View(clsComercio);
            }
            catch (Exception ex)
            {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }

        }

        [HttpPost]
        public ActionResult Editar(string alert, string RFC, string NombreCompleto, string Telefono, string Correo, string Direccion, string CuentaDeposito, int IdBanco, string RazonSocial, string NombreComercial, int IdGiroComercio, string Portal, int Persona, int Usuario_FAl, int Usuario_FUM, int IdTipoDeposito)
        {
            int IdPerfil = Int32.Parse(HttpContext.Session.GetString("IdPerfil"));

            ViewBag.Alert = alert;
            Comercio clsComercio = new Comercio();
           
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    if (String.IsNullOrEmpty(RFC))
                    {
                        RFC = HttpContext.Session.GetString("RFC");

                    }
                    HttpContext.Session.SetString("RFC", RFC);

                    string connectionString = Configuration["ConnectionStrings:ConexionString"];

                    connection.Open();

                    using SqlCommand command = new SqlCommand("SP_EditarComercio", connection);

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@RFC", RFC);
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
                    command.Parameters.AddWithValue("@Persona", Persona);
                    command.Parameters.AddWithValue("@Usuario_FAl", IdPerfil);
                    command.Parameters.AddWithValue("@Usuario_FUM", IdPerfil);
                    command.Parameters.AddWithValue("@IdTipoDeposito", IdTipoDeposito);

                    command.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 100);
                    command.Parameters["@Mensaje"].Direction = ParameterDirection.Output;

                    int i = command.ExecuteNonQuery();

                    string mensaje = Convert.ToString(command.Parameters["@Mensaje"].Value);

                    connection.Close();

                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, mensaje);
                    return RedirectToAction("Index", "MesaControl", new { alert = ViewBag.Alert });
                }
            }
            

        //Listado de bancos
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
                        Banco banco = new Banco
                        {
                            IdBanco = Convert.ToInt32(dataReader["IdBanco"]),
                            Bancos = Convert.ToString(dataReader["Bancos"])
                        };

                        BancoList.Add(banco);
                    }
                }

                ViewData["Bancos"] = new SelectList(BancoList.ToList(), "IdBanco", "Bancos");
                connection.Close();

                return ViewData["Bancos"];
            }
        }

        //Listado de giros comerciales
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
                        GiroComercio giroComercio = new GiroComercio
                        {
                            IdGiroComercio = Convert.ToInt32(dataReader["IdGiroComercio"]),
                            GiroComercial = Convert.ToString(dataReader["GiroComercial"])
                        };

                        GiroComercioList.Add(giroComercio);
                    }
                }

                ViewData["GiroComercio"] = new SelectList(GiroComercioList.ToList(), "IdGiroComercio", "GiroComercial");
                connection.Close();

                return ViewData["GiroComercio"];
            }
        }

        // Metodo encargado de llenar la tabla de comercion y filtrar la informacion
        public JsonResult Buscar(searchConceptos data)
        {
            //string currentUser = HttpContext.Session.GetString("IdUsuario").ToString();
            try
            {
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    var persona = "NULL";

                    if (data.Persona == "Fisica") {
                        persona = "1";
                    }
                    else if (data.Persona == "Moral")
                    {
                        persona = "0";
                    }
                    else
                    {
                        persona = "NULL";
                    }
                    //if (data.rfc == null)
                    //{
                    //    data.rfc = "NULL";
                    //}
                    //if (data.NombreCompleto == null)
                    //{
                    //    data.NombreCompleto = "NULL";
                    //}
                    //if (data.RazonSocial == null)
                    //{
                    //    data.RazonSocial = "NULL";
                    //}

                    List<Comercio> listComercio = new List<Comercio>();

                    message res = new message();

                    string consulta = string.Format("exec SP_SelectComercios {0}, {1}, {2}, {3}, {4}, {5}", 1, persona, data.rfc, data.NombreCompleto, data.RazonSocial, "NULL");
                    //string consulta = "select * from Comercio";

                    SqlCommand command = new SqlCommand(consulta, connection);
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Comercio cmc = new Comercio();
                            cmc.IdComercio = Convert.ToInt32(dr["IdComercio"]);
                            cmc.IdEmisor = dr.IsDBNull("IdEmisor") ? 0 : Convert.ToInt32(dr["IdEmisor"]);
                            cmc.RFC = Convert.ToString(dr["RFC"]);
                            cmc.NombreCompleto = Convert.ToString(dr["NombreCompleto"]);
                            cmc.Telefono = Convert.ToString(dr["Telefono"]);
                            cmc.Correo = Convert.ToString(dr["Correo"]);
                            cmc.Direccion = Convert.ToString(dr["Direccion"]);
                            cmc.CuentaDeposito = Convert.ToString(dr["CuentaDeposito"]);
                            cmc.Banco = Convert.ToString(dr["Banco"]);
                            cmc.RazonSocial = Convert.ToString(dr["RazonSocial"]);
                            cmc.NombreComercial = Convert.ToString(dr["NombreComercial"]);
                            cmc.GiroComercio = Convert.ToString(dr["GiroComercial"]);
                            cmc.Portal = Convert.ToString(dr["Portal"]);
                            cmc.PersonaMoral = Convert.ToInt32(dr["PersonaMoral"]);
                            cmc.PersonaFisica = Convert.ToInt32(dr["PersonaFisica"]);
                            cmc.Estatus = Convert.ToString(dr["Estatus"]);
                            cmc.Activo = Convert.ToInt32(dr["Activo"]);
                            cmc.IdCompania = Convert.ToString(dr["Compania"]);
                            cmc.IdTipoDeposito = Convert.ToString(dr["TipoDeposito"]);
                            cmc.EmailConfirmado = dr.IsDBNull("EmailConfirmado") ? "1" : Convert.ToString(dr["EmailConfirmado"]);
                            listComercio.Add(cmc);
                        }

                        res.status = true;
                        res.mensaje = "Success";
                        res.data = listComercio;
                        connection.Close();
                        return Json(res);

                    }

                }
            }
            catch (Exception ex)
            {
                List<message> error = new List<message>() {
                    new message {status = false, mensaje = ex.Message}
                };

                return Json(error);
            }
        }

        // Metodo encargado de devolver la información asociada al RFC
        [HttpGet]
        public ActionResult Verificacion(string RFC, string? alert)
        {
            ViewBag.Alert = alert;
            Comercio clsComercio = new Comercio();
            try
            {
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    if (String.IsNullOrEmpty(RFC))
                    {
                        RFC = HttpContext.Session.GetString("RFC");

                    }
                    HttpContext.Session.SetString("RFC", RFC);

                    string consulta = string.Format("exec SP_SelectComercio {0}", RFC);

                    SqlCommand command = new SqlCommand(consulta, connection);

                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            clsComercio.IdComercio = Convert.ToInt32(dr["IdComercio"]);
                            clsComercio.IdEmisor = dr.IsDBNull("IdEmisor") ? 0 : Convert.ToInt32(dr["IdEmisor"]);
                            clsComercio.RFC = Convert.ToString(dr["RFC"]);
                            clsComercio.NombreCompleto = Convert.ToString(dr["NombreCompleto"]);
                            clsComercio.Telefono = Convert.ToString(dr["Telefono"]);
                            clsComercio.Correo = Convert.ToString(dr["Correo"]);
                            clsComercio.Direccion = Convert.ToString(dr["Direccion"]);
                            clsComercio.CuentaDeposito = Convert.ToString(dr["CuentaDeposito"]);
                            clsComercio.Banco = Convert.ToString(dr["Banco"]);
                            clsComercio.RazonSocial = Convert.ToString(dr["RazonSocial"]);
                            clsComercio.NombreComercial = Convert.ToString(dr["NombreComercial"]);
                            clsComercio.GiroComercio = Convert.ToString(dr["GiroComercial"]);
                            clsComercio.Portal = Convert.ToString(dr["Portal"]);
                            clsComercio.PersonaMoral = Convert.ToInt32(dr["PersonaMoral"]);
                            clsComercio.PersonaFisica = Convert.ToInt32(dr["PersonaFisica"]);
                            clsComercio.Estatus = Convert.ToString(dr["Estatus"]);
                            clsComercio.Activo = Convert.ToInt32(dr["Activo"]);
                            clsComercio.IdCompania = Convert.ToString(dr["Compania"]);
                            clsComercio.IdTipoDeposito = Convert.ToString(dr["TipoDeposito"]);

                        }
                    }
                    connection.Close();
                }

                return View(clsComercio);
            }
            catch (Exception ex)
            {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }

        }

        // Metodo encargado de cambiar el estatus de un comercio a "Aprobado"
        public ActionResult Aprobar(string RFC, int IdArchivo, string? alert)
        {
            ViewBag.Alert = alert;


            if (String.IsNullOrEmpty(RFC))
            {
                RFC = HttpContext.Session.GetString("RFC");
            }

            HttpContext.Session.SetString("RFC", RFC);


            string connectionString = Configuration["ConnectionStrings:ConexionString"];
            using SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                using SqlCommand command3 = new SqlCommand("SP_AprobarComercio", connection);

                command3.CommandType = CommandType.StoredProcedure;

                command3.Parameters.AddWithValue("@RFC", RFC);

                command3.ExecuteNonQuery();
                connection.Close();

            }
            catch (SqlException ex)
            {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }

            ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Comercio aprobado correctamente.");
            return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
        }

        // Metodo encargado de cambiar el estatus de un comercio a "Rechazado"
        public ActionResult Rechazar(string RFC, int IdArchivo, string? alert)
        {
            ViewBag.Alert = alert;

            if (String.IsNullOrEmpty(RFC))
            {
                RFC = HttpContext.Session.GetString("RFC");
            }

            HttpContext.Session.SetString("RFC", RFC);


            string connectionString = Configuration["ConnectionStrings:ConexionString"];
            using SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                using SqlCommand command3 = new SqlCommand("SP_RechazarComercio", connection);

                command3.CommandType = CommandType.StoredProcedure;

                command3.Parameters.AddWithValue("@RFC", RFC);



                command3.ExecuteNonQuery();
                connection.Close();

            }
            catch (SqlException ex)
            {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }

            ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "El estatus del registro cambio a rechazado correctamente.");
            return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
        }
    }
}


