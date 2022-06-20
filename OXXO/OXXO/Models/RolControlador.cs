using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace OXXO.Models
{
    public partial class RolControlador
    {   [Key]
        public int IdRol { get; set; }
        public int IdPerfil { get; set; }
        public int IdControlador { get; set; }
        public int IdAccion { get; set; }
        public int Leer { get; set; }
        public int Crear { get; set; }
        public int Editar { get; set; }

        public virtual AccionControlador IdAccionNavigation { get; set; }
        public virtual Controlador IdControladorNavigation { get; set; }
        public virtual Perfil IdPerfilNavigation { get; set; }
    }
}
