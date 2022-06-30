using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.ComponentModel;


namespace OXXO.Models
{
    public class TipoDocumento
    {
       public int IdTipoDocumento { get; set; }

        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string NombreDocumento { get; set; }
        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string TipoArchivo { get; set; }
        public bool PersonaFisica { get; set; }
        public bool PersonaMoral { get; set; }

        public bool Obligatorio { get; set; }

    }
}
   
