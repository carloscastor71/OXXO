using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace OXXO.Models
{
    public partial class GiroComercio
    {
        public GiroComercio()
        {
            Comercio = new HashSet<Comercio>();
        }
        [Key]
        public int IdGiroComercio { get; set; }
        public string GiroComercial { get; set; }
        public int? UsuarioFal { get; set; }
        public DateTime? Fal { get; set; }
        public int? UsuarioFum { get; set; }
        public DateTime? Fum { get; set; }
        public int Activo { get; set; }

        public virtual ICollection<Comercio> Comercio { get; set; }
    }
}
