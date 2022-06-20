using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace OXXO.Models
{
    public partial class Documento
    {
        public int IdArchivo { get; set; }
        public int IdAComercio { get; set; }
        public string Nombre { get; set; }
        public byte[] Archivo { get; set; }
        public string Extension { get; set; }
        public bool? Activo { get; set; }

        public IFormFile FormFile { get; set; }
    }
}
