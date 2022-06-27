using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
namespace OXXO.Models
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext() 
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=192.168.1.244;Database=Agm;User Id=Desarrollo;Password=C0nsulting;");
            }
        }
        public DbSet<Comercio> Comercio { get; set; }
    }
}
