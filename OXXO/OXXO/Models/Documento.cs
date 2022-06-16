using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace OXXO.Models
{
    public partial class Documento
    {
        public int IdArchivo { get; set; }
        public string Nombre { get; set; }
        public byte[] archivo { get; set; }
        public string extension { get; set; }        
        public int IdTipoDeposito { get; set; }

        public virtual Comercio IdComercioNavigation { get; set; }
    }
}
