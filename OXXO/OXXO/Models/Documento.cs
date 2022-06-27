using Microsoft.AspNetCore.Http;
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

        

        [Key]
        public int IdArchivo { get; set; }
        public string IdComercio { get; set; }
        public string NombreDocumento { get; set; }
        public string Descripcion { get; set; }
        public string RFC { get; set; }
        public int IdTipoDocumento { get; set; }
        public string nombre { get; set; }
        public byte[] archivo { get; set; }
        public string extension { get; set; }
        public bool? activo { get; set; }

    }
}
