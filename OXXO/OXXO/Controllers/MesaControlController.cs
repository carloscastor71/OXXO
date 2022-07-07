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
using System.Net;
using System.Net.Mail;
using System.IO;

namespace OXXO.Controllers
{
    public class MesaControlController : Controller
    {
        // GET: CategorizacionController

        string dbConn = "", host = "", port = "", fromAddress = "", passwordmail = "";

        public IConfiguration Configuration { get; }

        public MesaControlController(IConfiguration configuration)
        {
            Configuration = configuration;
            dbConn = Configuration["ConnectionStrings:ConexionString"];
            host = Configuration["Smtp-Server"];
            port = Configuration["Smtp-Port"];
            fromAddress = Configuration["Smtp-FromAddress"];
            passwordmail = Configuration["Smtp-Password"];
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
            string mensaje = "";
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

                 mensaje = Convert.ToString(command.Parameters["@Mensaje"].Value);

                connection.Close();

            }

            try
            {

                SendMail(RFC);
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, mensaje);
                return RedirectToAction("Index", "MesaControl", new { alert = ViewBag.Alert });
            }
            catch (Exception ex)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction("Index", "MesaControl", new { alert = ViewBag.Alert });
            }

        }

        [Route("[Controller]/[Action]/{id}/{name}")]
        public ActionResult Activacion()
        {
            if (RouteData.Values["id"] != null)
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

        //Metodo para enviar documentos por Email
        [HttpPost]
        public async Task<IActionResult> SendDocMail(string RFC,string Correo, IList<IFormFile> Files)
        {
            try
            {
                string consulta = "Select RFC, NombreCompleto, NombreComercial, Correo from Comercio WHERE RFC = '" + RFC + "'";
                string NombreCompleto = "", NombreComercial = "";
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
                            }
                        }
                        connection.Close();
                    }

                    using (MailMessage mm = new MailMessage(fromAddress, Correo))
                    {
                        mm.Subject = "Grupo AGM: Documento(s) Generado(s) para el comercio";
                        string body = "<h1 id='titulo'>Este correo fue enviado por parte de Grupo Empresarial AGM.</h1>";
                        body += "<br/>";
                        body += "<p style = 'vertical - align: middle; color: #FF8B00' > El documento adjunto está dirigido para el siguiente comercio:</ p >";
                        body += "<br/> ";


                        if (!String.IsNullOrEmpty(NombreCompleto))
                        {
                            body += "<p> Nombre completo:      <b>" + NombreCompleto + "</b></p>";
                        }
                        else if (!String.IsNullOrEmpty(NombreComercial))
                        {

                            body += "<p> Nombre comercial:      <b>" + NombreComercial + "</b></p>";

                        }

                        body += "<p> RFC:      <b>" + RFC + "</b></p>";


                        body += "<p> Correo:      <b>" + Correo + "</b></p>";



                        body += "<br /><br />Puedes descargar el siguiente documento o documentos adjunto:";

                        body += "<br/>";
                        body += "<br/>";
                        body += "</div>";
                        foreach (var file in Files)
                        {
                            if (file.Length > 0)
                            {
                                using (var ms = new MemoryStream())
                                {
                                    file.CopyTo(ms);
                                    var fileBytes = ms.ToArray();
                                    Attachment att = new Attachment(new MemoryStream(fileBytes), file.FileName);
                                    mm.Attachments.Add(att);
                                }
                            }
                        }



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

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Documento enviado Correctamente");
                return RedirectToAction("Index", "MesaControl", new { alert = ViewBag.Alert });
            }
            catch (Exception ex)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction("Index", "MesaControl", new { alert = ViewBag.Alert });
            }
            
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
                    mm.Subject = "Grupo AGM: Información Actualizada";
                    string body = "<h1 id='titulo'>Reconfirmación de Información</h1>";
                    body += "<br/>";
                    body += "<p style = 'vertical - align: middle; color: #FF8B00' > Se actualizó la información que rechazaste, favor de verificar esta nueva información </ p >";
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
                    body += "<br /><a href = '" + string.Format("{0}://{1}/Alta/Activacion/{2}/{3}", HttpContext.Request.Scheme, HttpContext.Request.Host, RFC, "2") + "'>Clic aquí para CONFIRMAR tus datos.</a>";


                    body += "<br /><br />O clic en el siguiente enlace para RECHAZAR esta información:";
                    body += "<br /><a href = '" + string.Format("{0}://{1}/Alta/Activacion/{2}/{3}", HttpContext.Request.Scheme, HttpContext.Request.Host, RFC, "3") + "'>Clic aquí para RECHAZAR estos datos.</a>";


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


