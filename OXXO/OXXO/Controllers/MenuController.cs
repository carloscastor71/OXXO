using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OXXO.Models;
using Microsoft.AspNetCore.Http;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using OXXO.Services;
using OXXO.Enums;

namespace OXXO.Controllers
{
    public class MenuController : Controller
    {
        string dbConn = "";
        public IConfiguration Configuration { get; }
        List<Menu> MenuList = new List<Menu>();
        List<Permisos> permisos = new List<Permisos>();
        
        public MenuController(IConfiguration configuration) { Configuration = configuration; dbConn = Configuration["ConnectionStrings:ConexionString"]; }
        public PartialViewResult MostrarMenu()
        
        {
            try
            {
                MenuList = GetMenu();

                string IdPerfil = HttpContext.Session.GetString("IdPerfil");
                MenuUser menuuser = new MenuUser();
                MenuUserItem menuItem;
                foreach (var menuMain in MenuList)
                {
                    menuItem = new MenuUserItem()
                    {
                        MenuMain = menuMain
                    };

                    using (SqlConnection connection = new SqlConnection(dbConn))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand("SP_BuscarControlador_PorRol", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("IdPerfil", IdPerfil);
                            command.Parameters.AddWithValue("IdMenu", menuMain.IdMenu);
                            SqlDataReader dr = command.ExecuteReader();
                            while (dr.Read())
                            {
                                Permisos clsPermisos = new Permisos();

                                clsPermisos.IdRol = Convert.ToInt32(dr["IdRol"]);
                                clsPermisos.IdPerfil = Convert.ToInt32(dr["IdPerfil"]);
                                clsPermisos.Encabezado = Convert.ToString(dr["Texto"]);
                                clsPermisos.ControllerName = Convert.ToString(dr["NombreControlador"]);
                                clsPermisos.ActionName = Convert.ToString(dr["NombreAccion"]);
                                clsPermisos.Leer = Convert.ToBoolean(dr["Leer"]);
                                clsPermisos.Crear = Convert.ToBoolean(dr["Crear"]);
                                clsPermisos.Editar = Convert.ToBoolean(dr["Editar"]);
                                clsPermisos.IdMenuPadre = Convert.ToInt32(dr["IdMenuPadre"]);
                                permisos.Add(clsPermisos);

                            }
                        }
                        connection.Close();

                        if (permisos.Count() > 0)
                        {
                            menuItem.SubMenus.AddRange(permisos);
                            menuuser.Menus.Add(menuItem);
                        }
                        permisos.Clear();
                    }

                }
                return PartialView("_MenuPartial", menuuser);
            }
            catch (Exception)
            {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Hubo un problema al cargar el menú. (MostrarMenu)");
         
                return PartialView("_MenuPartial", new { alert = ViewBag.Alert });
            }
            
        }

        public List<Menu> GetMenu()
        {
           
                using (SqlConnection connection = new SqlConnection(dbConn))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM Menu", connection);
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Menu clsMenu = new Menu();
                            clsMenu.IdMenu = Convert.ToInt32(dr["IdMenu"]);
                            clsMenu.NombreMenu = Convert.ToString(dr["NombreMenu"]);
                            clsMenu.Orden = Convert.ToInt32(dr["Orden"]);
                            clsMenu.Predeterminado = Convert.ToInt32(dr["Predeterminado"]);
                            MenuList.Add(clsMenu);

                        }
                    }
                    connection.Close();
                }

                return MenuList;
           

         
        }
    }
}
