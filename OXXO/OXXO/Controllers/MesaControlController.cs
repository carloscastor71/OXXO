using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public ActionResult Index()
        {
            return View();
        }

        // GET: CategorizacionController/Details/5
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
                    else if(data.Persona == "Moral")
                    {
                        persona = "0";
                    }
                    else
                    {
                        persona = "NULL";
                    }

                    List<Comercio> listComercio = new List<Comercio>();
                    
                    message res = new message();

                    string consulta = string.Format("exec SP_SelectComercios {0}, {1}, {2}, {3}, {4}, {5}", 1, persona, data.rfc, data.NombreCompleto,data.RazonSocial, "NULL");
                    //string consulta = "select * from Comercio";

                    SqlCommand command = new SqlCommand(consulta, connection);
                    connection.Open();

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Comercio cmc = new Comercio();
                            cmc.IdComercio = Convert.ToInt32(dr["IdComercio"]);
                            cmc.IdEmisor =  dr.IsDBNull("IdEmisor")? 0 : Convert.ToInt32(dr["IdEmisor"]);
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
                            cmc.PersonaMoral =  Convert.ToInt32(dr["PersonaMoral"]);
                            cmc.PersonaFisica = Convert.ToInt32(dr["PersonaFisica"]);
                            cmc.Estatus = Convert.ToString(dr["Estatus"]);
                            cmc.Activo = Convert.ToInt32(dr["Activo"]);
                            cmc.IdCompania =  Convert.ToString(dr["Compania"]);
                            cmc.IdTipoDeposito = Convert.ToString(dr["TipoDeposito"]);
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

        // GET: CategorizacionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategorizacionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategorizacionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategorizacionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategorizacionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategorizacionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
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



    }
}
