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
    public class Categorizacion
    {
        public int IdComercio { get; set; }
       public int IdEmisor { get; set; }

        public string RFC { get; set; }
        public string Giro { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string Cuenta { get; set; }
        public string Banco { get; set; }
        //public double Tasa { get; set; }
        public string Estatus { get; set; }
        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public int Cluster { get; set; }
        public string NombreCluster { get; set; }
        public string Comision { get; set; }
    }
}
   
