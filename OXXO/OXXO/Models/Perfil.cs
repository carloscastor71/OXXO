using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OXXO.Models
{
    public class Perfil
    {
        [Key]
        public string IdPerfil { get; set; }
        [Required(ErrorMessage = " * Ingrese el nombre")]
        [MaxLength(50, ErrorMessage = "El número máximo de caracteres que se pueden ingresar es 50")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = " * Ingrese una descripción")]
        [MaxLength(25, ErrorMessage = "El número máximo de caracteres que se pueden ingresar es 100")]
        public string Descripcion { get; set; }
        public Boolean Activo { get; set; }
        public DateTime? FechaAlta { get; set; }
        public DateTime? FechaUltimaMod { get; set; }
        public string IdUsuarioFA { get; set; }
        public string IdUsuarioFUM { get; set; }
        public List<Perfil> PerfilList { get; set; }
    }
}
