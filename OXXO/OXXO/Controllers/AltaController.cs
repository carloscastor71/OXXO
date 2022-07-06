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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace OXXO.Controllers
{
    public class AltaController : Controller
    {

        
        string dbConn = "", host="",port = "",fromAddress ="",passwordmail="";


        public IConfiguration Configuration { get; }

        //
        public AltaController(IConfiguration configuration)
        {
            Configuration = configuration;
            dbConn = Configuration["ConnectionStrings:ConexionString"];

            host = Configuration["Smtp-Server"];
            port = Configuration["Smtp-Port"];
            fromAddress = Configuration["Smtp-FromAddress"];
            passwordmail = Configuration["Smtp-Password"];
        }

        //Metodo que devuelve la vista principal de las altas para los comercios
        public IActionResult Index(string? alert)
        {
            ViewBag.Alert = alert;
            ListadoBancos();
            ListadoGiroComercial();
            return View();
        }

        //Este metodo es el encargado de dar de alta los registros de los comercios
        [HttpPost]
        public IActionResult Index(string Rfc, string NombreCompleto, string Telefono, string Correo, string Direccion, string CuentaDeposito, int IdBanco, string RazonSocial, string NombreComercial, int IdGiroComercio, string Portal, int Persona, int Usuario_FAl, int Usuario_FUM, int IdTipoDeposito)
        {
            //Id perfil asociado a la sesion abierta.
            int IdPerfil = Int32.Parse(HttpContext.Session.GetString("IdPerfil"));

            if (!String.IsNullOrEmpty(Rfc))
            {
                HttpContext.Session.SetString("RFC", Rfc);
            }
           
            string RFC = HttpContext.Session.GetString("RFC");

            string connectionString = Configuration["ConnectionStrings:ConexionString"];
            using SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            using SqlCommand command = new SqlCommand("SP_ComercioAlta", connection);

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

            if (i == -1) {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Warning, mensaje);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }

            try
            {
                SendMail(RFC);
            }
            catch (Exception ex)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Warning, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
            
            ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, mensaje);
            return RedirectToAction("Index","CargaDocumentos", new { alert = ViewBag.Alert });
        }

       
        //Metodo que genera una lista de los bancos disponibles en la base de datos
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

        [Route("[Controller]/[Action]/{id}/{name}")]
        public ActionResult Activacion()
        {
            if (RouteData.Values["id"]!= null)
            {
                
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SP_ConfirmarByEmail", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@RFC", RouteData.Values["id"].ToString());
                        command.Parameters.AddWithValue("@EmailConfirmado", Convert.ToInt32(RouteData.Values["name"]));
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    return PartialView("Activacion");
                }
            }
            return View();
        }

        private void SendMail(string RFC)
        {
            


            string consulta = "Select RFC, NombreCompleto, NombreComercial,RazonSocial, GiroComercial, Telefono, PersonaFisica, PersonaMoral, Portal, Correo, Direccion,CuentaDeposito, Banco  from Comercio as C LEFT JOIN GiroComercio as GC ON C.IdGiroComercio = GC.IdGiroComercio INNER JOIN Banco as B ON C.IdBanco = B.IdBanco WHERE RFC = '" + RFC + "'";
            string Correo = "", NombreCompleto = "", NombreComercial = "", RazonSocial = "", GiroComercial = "", Telefono = "", Portal = "", Direccion = "", CuentaDeposito = "", Banco = "", PersonaFisica = "", PersonaMoral = "";
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
                            NombreComercial = dr.IsDBNull("NombreComercial") ? "" : Convert.ToString(dr["NombreComercial"]);
                            Correo = dr.IsDBNull("Correo") ? "" : Convert.ToString(dr["Correo"]);
                            NombreCompleto = dr.IsDBNull("NombreCompleto") ? "" : Convert.ToString(dr["NombreCompleto"]);
                            RazonSocial = dr.IsDBNull("RazonSocial") ? "" : Convert.ToString(dr["RazonSocial"]);
                            GiroComercial = dr.IsDBNull("GiroComercial") ? "" : Convert.ToString(dr["GiroComercial"]);
                            Telefono = dr.IsDBNull("Telefono") ? "" : Convert.ToString(dr["Telefono"]);
                            Portal = dr.IsDBNull("Portal") ? "" : Convert.ToString(dr["Portal"]);
                            Direccion = dr.IsDBNull("Direccion") ? "" : Convert.ToString(dr["Direccion"]);
                            CuentaDeposito = dr.IsDBNull("CuentaDeposito") ? "" : Convert.ToString(dr["CuentaDeposito"]);
                            Banco = dr.IsDBNull("Banco") ? "" : Convert.ToString(dr["Banco"]);
                            PersonaFisica = dr.IsDBNull("PersonaFisica") ? "" : Convert.ToString(dr["PersonaFisica"]);
                            PersonaMoral = dr.IsDBNull("PersonaMoral") ? "" : Convert.ToString(dr["PersonaMoral"]);
                        }
                    }
                    connection.Close();
                }

                
                

                using (MailMessage mm = new MailMessage(fromAddress, Correo))
                {
                    mm.Subject = "Grupo AGM: Confirmación de Información Registrada";
                    string body = "<h1 id='titulo'>Confirmación de Información</h1>";
                    body += "<br/>";
                    body += "<p style = 'vertical - align: middle; color: #FF8B00' > Por favor confirme si la siguiente información de su comercio está correcta </ p >";
                    body += "<p></p> ";
                    if (PersonaFisica == "1" && PersonaMoral == "1")
                    {
                        body += "<p> Usted está registrada cómo: <b>Persona Física y Persona Moral</b></p>";
                    }
                    else if (PersonaFisica == "1")
                    {
                        body += "<p> Usted está registrada cómo: <b>Persona Física</b></p>";
                    }

                    else if (PersonaMoral == "1")
                    {
                        body += "<p> Usted está registrada cómo: <b>Persona Moral</b></p>";
                    }

                    if (!String.IsNullOrEmpty(NombreCompleto))
                    {
                        body += "<p> Nombre Completo registrado:      <b>" + NombreCompleto + "</b></p>";
                    }
                    else if (!String.IsNullOrEmpty(RazonSocial) || !String.IsNullOrEmpty(NombreComercial))
                    {
                        body += "<p> Razón Social registrada:      <b>" + RazonSocial + "</b></p>";
                        body += "<p> Nombre Comercial registrado:      <b>" + NombreComercial + "</b></p>";
                        body += "<p> Giro Comercial registrado:      <b>" + GiroComercial + "</b></p>";
                    }

                    body += "<p> RFC registrado:      <b>" + RFC + "</b></p>";

                    if (!String.IsNullOrEmpty(Portal))
                    {
                        body += "<p> Portal registrado:      <b>" + Portal + "</b></p>";
                    }

                    body += "<p> Teléfono registrado:      <b>" + Telefono + "</b></p>";
                    body += "<p> Correo registrado:      <b>" + Correo + "</b></p>";
                    body += "<p> Dirección registrada:     <b> " + Direccion + "</b></p>";
                    body += "<p> Cuenta de deposito registrada:      <b>" + CuentaDeposito + "</b></p>";
                    body += "<p> Banco registrado:      <b>" + Banco + "</b></p>";


                    body += "<br /><br />Haga clic en el siguiente enlace para CONFIRMAR que su información está correcta:";
                    body += "<br /><a href = '" + string.Format("{0}://{1}/Alta/Activacion/{2}/{3}", HttpContext.Request.Scheme, HttpContext.Request.Host, RFC,"2") + "'>Clic aquí para CONFIRMAR tus datos.</a>";


                    body += "<br /><br />O clic en el siguiente enlace para RECHAZAR esta información:";
                    body += "<br /><a href = '" + string.Format("{0}://{1}/Alta/Activacion/{2}/{3}", HttpContext.Request.Scheme, HttpContext.Request.Host, RFC,"3") + "'>Clic aquí para RECHAZAR estos datos.</a>";
                    

                    body += "<br/>";
                    body += "<br/>";
                    body += "</div>";



                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = host;
                    smtp.EnableSsl = true;
                    NetworkCredential networkCred = new NetworkCredential(fromAddress, passwordmail);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCred;
                    smtp.Port = Convert.ToInt32(port);

                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(mm);
                }
            }
        }
            //Metodo que genera una lista de los giros comerciales existentes en el base de datos
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void enviarMail(Comercio comercio)
        {
            Guid ActivationCode = Guid.NewGuid();
           
        }
    }
}
