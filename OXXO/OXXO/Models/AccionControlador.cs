using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace OXXO.Models
{
    public partial class AccionControlador
    {
        public AccionControlador()
        {
            RolControlador = new HashSet<RolControlador>();
        }

        public int IdAccion { get; set; }
        public string NombreAccion { get; set; }
        public string Encabezado { get; set; }
        public int Item { get; set; }
        public int? IdControlador { get; set; }

        public virtual Controlador IdControladorNavigation { get; set; }
        public virtual ICollection<RolControlador> RolControlador { get; set; }
    }
}
