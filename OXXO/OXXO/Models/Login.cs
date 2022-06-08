using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OXXO.Models
{
    public class Login
    {
        public int IdUsuario { get; set; }
        
        [Required(ErrorMessage = "Por favor ingrese un nombre.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Por favor ingrese una contraseña.")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }
        public int? IdRol { get; set; }
        public string RolName { get; set; }
    }
}
   
