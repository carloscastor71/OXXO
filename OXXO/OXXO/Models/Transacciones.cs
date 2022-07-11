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
    public class Transacciones
    {
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public string Fecha { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public string Fecha2 { get; set; }
        public string Hora { get; set; }
        public string IdEmisor { get; set; }
        public string Monto { get; set; }
        public string Referencia {get;set;}
        public string Nombre { get; set; }
        public string TipoOperacion { get; set; }

    }
}
   
