using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace OXXO.Models
{
    public partial class Controlador
    {
        public Controlador()
        {
            AccionControlador = new HashSet<AccionControlador>();
            RolControlador = new HashSet<RolControlador>();
        }

        public int IdControlador { get; set; }
        public string NombreControlador { get; set; }
        public string Texto { get; set; }
        public int? IdMenuPadre { get; set; }

        public virtual Menu IdMenuPadreNavigation { get; set; }
        public virtual ICollection<AccionControlador> AccionControlador { get; set; }
        public virtual ICollection<RolControlador> RolControlador { get; set; }
    }
}
