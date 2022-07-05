using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace OXXO.Models
{
    [Table("Comercio")]
    public  class Comercio
    {
        public Comercio  ()
        {
            Documento = new HashSet<Documento>();
        }

        public int IdEmisor { get; set; }
        public int IdComercio { get; set; }
        [Key]
        public string? RFC { get; set; }
        public string NombreCompleto { get; set; }
         
        public string Telefono { get; set; }


        [EmailAddress(ErrorMessage = "¡Correo no válido!")]
        public string Correo { get; set; }
         
        public string Direccion { get; set; }
         
        public string CuentaDeposito { get; set; }
         
        public int? IdBanco { get; set; }
        public string Banco { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public int? IdGiroComercio { get; set; }
        public string GiroComercio { get; set; }
        [Url(ErrorMessage = "¡Portal no válido!")]
        public string Portal { get; set; }
        public int PersonaMoral { get; set; }
        public int PersonaFisica { get; set; }
        public string Estatus { get; set; }
        public int? Activo { get; set; }
        public int? UsuarioFal { get; set; }
        public DateTime? Fal { get; set; }
        public int? UsuarioFum { get; set; }
        public DateTime? Fum { get; set; }
         
        public string IdCompania { get; set; }
         
        public string IdTipoDeposito { get; set; }
        public string tipoDeposito { get; set; }


        public virtual Estatus EstatusNavigation { get; set; }
        public virtual Banco IdBancoNavigation { get; set; }
        public virtual Compania IdCompaniaNavigation { get; set; }
        public virtual GiroComercio IdGiroComercioNavigation { get; set; }
        public virtual TipoDeposito IdTipoDepositoNavigation { get; set; }
        public virtual ICollection<Documento> Documento { get; set; }
        public IEnumerable<Comercio> Comercios { get; set; }
    }

   
}
