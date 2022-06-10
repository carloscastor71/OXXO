using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OXXO.Models
{
    public class Permisos
    {
        public int IdRol { get; set; }
        public int IdPerfil { get; set; }
        public string Encabezado { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool Leer { get; set; }
        public bool Crear { get; set; }
        public bool Editar { get; set; }
        public bool Item { get; set; }
        public string CssIcono { get; set; }
        public int IdMenuPadre { get; set; }
        public Menu MenuPadre { get; set; }

    }
    public class PermisosUsuario
    {
        public bool Leer { get; set; }
        public bool Crear { get; set; }
        public bool Editar { get; set; }
    }
}
