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

    public class OpDiaria
    {
        public int id { get; set; }
        //public int Dias { get; set; }
        public int fondos { get; set; }
    }
    public class ActMeses
    {
        public int ENERO { get; set; }
        public int FEBRERO { get; set; }
        public int MARZO { get; set; }
    }
}
   
