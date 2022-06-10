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
        [Required(ErrorMessage = "Por favor ingrese un nombre.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Por favor ingrese una contraseña.")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }
    }
}
   
