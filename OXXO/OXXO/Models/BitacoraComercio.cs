﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace OXXO.Models
{
    public partial class BitacoraComercio
    {
        public string IdEmisor { get; set; }
        public string Comentarios { get; set; }
        public int? Estatus { get; set; }
        public int? Activo { get; set; }
        public int? UsuarioFal { get; set; }
        public DateTime? Fal { get; set; }
        public int? UsuarioFum { get; set; }
        public DateTime? Fum { get; set; }
    }
}
