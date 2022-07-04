using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace OXXO.Models
{
    public partial class Cluster
    {
        //public Cluster()
        //{
        //    Comercio = new HashSet<Comercio>();
        //}
        [Key]
        public int IdCluster{ get; set; }
        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string NombreCluster { get; set; }
        public int? UsuarioFAI { get; set; }
        public DateTime? FAI { get; set; }
        public int? Usuario_FUM { get; set; }
        [Required(ErrorMessage = "Debe asignar una tasa o comisión en %.")]
        public DateTime? FUM { get; set; }
        public int Activo { get; set; }

        //public virtual ICollection<Comercio> Comercio { get; set; }
    }
}
