using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OXXO.Models
{
    public class searchConceptos
    {
        //persona, rfc , razon social, estatus, nombre completo
        public string IdEmisor { get; set; }
        public string rfc { get; set; }
        public string NombreCompleto { get; set; }
        public string RazonSocial { get; set; }
        public string Estatus { get; set; }
        public string Persona { get; set; }

        public string EmailConfirmado { get; set; }

    }
}
