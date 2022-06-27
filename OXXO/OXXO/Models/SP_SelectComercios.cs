using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace OXXO.Models
{
   
    public  class SP_SelectComercios
    {
        public int IdComercio { get; set; }

        public int IdEmisor { get; set; }     
        public string? RFC { get; set; }
        public string NombreCompleto { get; set; }         
        public string Telefono { get; set; }               
        public string Correo { get; set; }         
        public string Direccion { get; set; }      
        public string CuentaDeposito { get; set; }        
        public string Banco { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string GiroComercio { get; set; }
        public string Portal { get; set; }
        public int PersonaMoral { get; set; }
        public int PersonaFisica { get; set; }
        public string Estatus { get; set; }
        public int? Activo { get; set; }
        public string? Usuario_FAl { get; set; }
        public DateTime? FAl { get; set; }
        public string Usuario_FUM { get; set; }
        public DateTime? FUM { get; set; }
         
        public string Compania { get; set; }
         
        public string tipoDeposito { get; set; }

     
    }
}
