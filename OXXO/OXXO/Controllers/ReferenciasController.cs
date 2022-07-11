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
    public class ReferenciasController : Controller
    {
        string dbConn = "";
        public IConfiguration Configuration { get; }
        public ReferenciasController(IConfiguration configuration)
        {
            Configuration = configuration;
            dbConn = Configuration["ConnectionStrings:ConexionString"];
        }
        public IActionResult Index(string? alert, Referencias data)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    string consulta = "";
                    List<Referencias> listarefer = new List<Referencias>();
                   
                    

                    if (data.Fecha == null && data.Fecha == null && data.IdEmisor == null && data.Estatus == null)
                    {
                        consulta = "SELECT Fecha, Hora, T.IdEmisor, Tienda,NombreCompleto, NombreComercial, Monto, Referencia, NombreEstatus FROM Transaccion AS T LEFT JOIN Comercio AS C ON T.IdEmisor = C.IdEmisor INNER JOIN EstatusPago AS EP ON T.Estatus = EP.IdEstatusPagos";
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
                        if (data.Estatus == null)
                        {
                            data.Estatus = "NULL";
                        }
                        consulta = string.Format("exec SP_SelectReferencias '{0}', '{1}', {2}, {3}", data.Fecha, data.Fecha2, data.IdEmisor, data.Estatus);
                    }

                    var dateAndTime = DateTime.Now;
                    var date = dateAndTime.Date;

                    SqlCommand command = new SqlCommand(consulta,connection);
                    connection.Open();
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Referencias clsReferencias = new Referencias();

                            clsReferencias.Fecha = Convert.ToString(dr["Fecha"]).Substring(0,10);
                            clsReferencias.Hora = Convert.ToString(dr["Hora"]);
                            clsReferencias.IdEmisor = dr.IsDBNull("IdEmisor") ? "N/A" : Convert.ToString(dr["IdEmisor"]);
                            if (dr.IsDBNull("NombreComercial") && dr.IsDBNull("NombreCompleto"))
                            {
                                clsReferencias.Nombre = Convert.ToString(dr["Tienda"]);
                            }
                            else if (dr.IsDBNull("NombreComercial"))
                            {
                                clsReferencias.Nombre = Convert.ToString(dr["NombreCompleto"]);
                            }
                            else
                            {
                                clsReferencias.Nombre = Convert.ToString(dr["NombreComercial"]);
                            }
                            
                            clsReferencias.Monto = Convert.ToString(dr["Monto"]);
                            clsReferencias.Referencia = Convert.ToString(dr["Referencia"]);
                            clsReferencias.Estatus = Convert.ToString(dr["NombreEstatus"]);
                            listarefer.Add(clsReferencias);
                        }
                        connection.Close();
                    }

                    return View(listarefer);
                }
            }
            catch (Exception ex)
            {

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, ex.Message);
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });
            }
            
            
        }
        

        public object ListadoDeClusters()
        {
            try
            {
                List<Cluster> ListaClusters = new List<Cluster>();
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    connection.Open();

                    string consulta = "SELECT * FROM Cluster WHERE Activo = 1";
                    SqlCommand command = new SqlCommand(consulta, connection);
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Cluster clsCluster = new Cluster();
                            clsCluster.IdCluster = Convert.ToInt32(dr["IdCluster"]);
                            clsCluster.NombreCluster = Convert.ToString(dr["NombreCluster"]);

                            ListaClusters.Add(clsCluster);
                        }
                    }
                    ViewData["Clusters"] = new SelectList(ListaClusters.ToList(), "IdCluster", "NombreCluster");
                    connection.Close();

                    return ViewData["Clusters"];
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
