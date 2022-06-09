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
    public class Usuario
    {
        [Key]
        public string IdUsuario { get; set; }
        [Required(ErrorMessage = " * Ingrese el Nombre")]
        [MaxLength(25, ErrorMessage = "El número máximo de caracteres que se pueden ingresar es 25")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = " * Ingrese Apellido(s)")]
        [MaxLength(50, ErrorMessage = "El número máximo de caracteres que se pueden ingresar es 50")]
        [DisplayName("Apellido(s)")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = " * Ingrese el usuario")]
        [MaxLength(50, ErrorMessage = "El número máximo de caracteres que se pueden ingresar es 50")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = " * Ingrese la contraseña")]
        [MaxLength(32, ErrorMessage = "El número máximo de caracteres que se pueden ingresar es 32")]
        public string Contrasena { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no son iguales.")]
        public string ConfirmarContrasena{ get; set; }

        
        [Required(ErrorMessage = " * Ingrese el correo")]
        public string Correo { get; set; }

        [MaxLength(25, ErrorMessage = "El número máximo de caracteres que se pueden ingresar es 32")]
        public string Puesto { get; set; }
        public Boolean Activo { get; set; }
        [Required(ErrorMessage = " * Ingrese la vigencia")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Vigencia { get; set; }
        [Required(ErrorMessage = " * Seleccione el perfil ")]
        public int IdPerfil { get; set; }
        [Required(ErrorMessage = " * Seleccione una compania ")]
        public int IdCompania { get; set; }
        public DateTime? FechaAlta { get; set; }
        public DateTime? FechaUltimaMod { get; set; }
        public int IdUsuarioFA { get; set; }
        public int IdUsuarioFUM { get; set; }


        //intento para encriptar contraseña
        public static String GetMD5Hash(String contrasena)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(contrasena);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            String hash = s.ToString();
            return hash;
        }

    }
}
   
