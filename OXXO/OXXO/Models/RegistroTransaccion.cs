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
    public partial class RegistroTransaccion
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        
        public DateTime? Fecha { get; set; }

        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string Hora { get; set; }

        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string Monto { get; set; }

        [Key]
        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string Referencia {get;set;}

        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string Tienda { get; set; }

        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string Estatus { get; set; }

    }
}
   
