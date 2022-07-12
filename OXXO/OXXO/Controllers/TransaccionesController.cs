using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using OXXO.Controllers;
using OXXO.Models;
using OXXO.Services;
using OXXO.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace OXXO.Controllers
{
    public class TransaccionesController : Controller
    {
        string dbConn = "";
        public IConfiguration Configuration { get; }
        public TransaccionesController(IConfiguration configuration)
        {
            Configuration = configuration;
            dbConn = Configuration["ConnectionStrings:ConexionString"];
        }
        public IActionResult Index(string? alert, Transacciones data)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    string consulta = "";
                    List<Transacciones> listatrans = new List<Transacciones>();

                    if (data.Fecha == null && data.Fecha == null && data.IdEmisor == null && data.TipoOperacion == null)
                    {
                        consulta = "SELECT Fecha, Hora, T.IdEmisor, Tienda,NombreCompleto, NombreComercial, Monto, Referencia, NombreOperacion FROM Transaccion AS T LEFT JOIN Comercio AS C ON T.IdEmisor = C.IdEmisor INNER JOIN TipoOperacion AS TP ON T.IdTipoOperacion = TP.IdTipoOperacion";
                    }
                    else
                    {
                        if (data.Fecha == null)
                        {
                            data.Fecha = "NULL";
                        }
                        if (data.Fecha2 == null)
                        {
                            data.Fecha2 = "NULL";
                        }
                        if (data.IdEmisor == null)
                        {
                            data.IdEmisor = "NULL";
                        }
                        if (data.TipoOperacion == null)
                        {
                            data.TipoOperacion = "NULL";
                        }
                        consulta = string.Format("exec SP_SelectTransacciones '{0}', '{1}', {2}, {3}", data.Fecha, data.Fecha2, data.IdEmisor, data.TipoOperacion);
                    }


                    SqlCommand command = new SqlCommand(consulta,connection);
                    connection.Open();
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Transacciones clsTransacciones = new Transacciones();

                            clsTransacciones.Fecha = Convert.ToString(dr["Fecha"]).Substring(0,10);
                            clsTransacciones.Hora = Convert.ToString(dr["Hora"]);
                            clsTransacciones.IdEmisor = dr.IsDBNull("IdEmisor") ? "N/A" : Convert.ToString(dr["IdEmisor"]);
                            if (dr.IsDBNull("NombreComercial") && dr.IsDBNull("NombreCompleto"))
                            {
                                clsTransacciones.Nombre = Convert.ToString(dr["Tienda"]);
                            }
                            else if (dr.IsDBNull("NombreComercial"))
                            {
                                clsTransacciones.Nombre = Convert.ToString(dr["NombreCompleto"]);
                            }
                            else
                            {
                                clsTransacciones.Nombre = Convert.ToString(dr["NombreComercial"]);
                            }
                            
                            clsTransacciones.Monto = Convert.ToString(dr["Monto"]);
                            clsTransacciones.Referencia = Convert.ToString(dr["Referencia"]);
                            clsTransacciones.TipoOperacion = Convert.ToString(dr["NombreOperacion"]);
                            listatrans.Add(clsTransacciones);
                        }
                        connection.Close();
                    }

                    return View(listatrans);
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
