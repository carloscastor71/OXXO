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
    public class Tablero
    {
       public int id{ get; set; }
        public string Nombre { get; set; }
        public string Transaccion { get; set; }
        public string Monto { get; set; }

    }
}
   
