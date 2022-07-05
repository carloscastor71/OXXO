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
    public class Mantenimiento
    {
        public int IdComercio { get; set; }
       public int IdEmisor { get; set; }
        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string RFC { get; set; }
        public string Giro { get; set; }
        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string RazonSocial { get; set; }
        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string NombreComercial { get; set; }
        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string Cuenta { get; set; }
        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string? Banco { get; set; }
        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public int Activo { get; set; }
        public int Estatus { get; set; }
        [Url(ErrorMessage = "¡Portal no válido!")]
        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string Portal { get; set; }
        [EmailAddress(ErrorMessage = "¡Correo no válido!")]
        [Required(ErrorMessage = "Este campo no puede estar vacío.")]
        public string Correo { get; set; }
        public string IdBanco { get; set; }
        public string IdGiroComercio { get; set; }
    }
}
   
