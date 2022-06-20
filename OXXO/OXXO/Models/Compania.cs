using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace OXXO.Models
{
    public partial class Compania
    {
        public Compania()
        {
            Comercio = new HashSet<Comercio>();
            Usuario = new HashSet<Usuario>();
        }

        public int IdCompania { get; set; }
        public string Companias { get; set; }
        public int? Activo { get; set; }
        public int? UsuarioFal { get; set; }
        public DateTime? Fal { get; set; }
        public int? UsuarioFum { get; set; }
        public DateTime? Fum { get; set; }

        public virtual ICollection<Comercio> Comercio { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
