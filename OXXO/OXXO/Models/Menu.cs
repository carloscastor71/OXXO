using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OXXO.Models
{
    public class Menu
    {
        public int IdMenu { get; set; }
        public string NombreMenu { get; set; }
        public int Orden { get; set; }
        public int Predeterminado { get; set; }

    }

    public class MenuUser
    {
        public List<MenuUserItem> Menus { get; set; }

        public MenuUser()
        {
            this.Menus = new List<MenuUserItem>();
        }
    }

    public class MenuUserItem
    {
        public Menu MenuMain { get; set; }

       public List<Permisos> SubMenus { get; set; }


        public MenuUserItem()
        {
            this.MenuMain = new Menu();
            this.SubMenus = new List<Permisos>();
        }
    }

}
   
