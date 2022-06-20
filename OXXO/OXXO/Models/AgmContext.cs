//using System;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata;

//// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
//// If you have enabled NRTs for your project, then un-comment the following line:
//// #nullable disable

//namespace OXXO.Models
//{
//    public partial class AgmContext : DbContext
//    {
//        public AgmContext()
//        {
//        }

//        public AgmContext(DbContextOptions<AgmContext> options)
//            : base(options)
//        {
//        }

//        public virtual DbSet<AccionControlador> AccionControlador { get; set; }
//        public virtual DbSet<Banco> Banco { get; set; }
//        public virtual DbSet<BitacoraComercio> BitacoraComercio { get; set; }
//        public virtual DbSet<Comercio> Comercio { get; set; }
//        public virtual DbSet<Compania> Compania { get; set; }
//        public virtual DbSet<Controlador> Controlador { get; set; }
//        public virtual DbSet<Documento> Documento { get; set; }
//        public virtual DbSet<Estatus> Estatus { get; set; }
//        public virtual DbSet<GiroComercio> GiroComercio { get; set; }
//        public virtual DbSet<Menu> Menu { get; set; }
//        public virtual DbSet<Perfil> Perfil { get; set; }
//        public virtual DbSet<RolControlador> RolControlador { get; set; }
//        public virtual DbSet<TipoDeposito> TipoDeposito { get; set; }
//        public virtual DbSet<Usuario> Usuario { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=192.168.1.244;Database=Agm;User Id=Desarrollo;Password=C0nsulting;");
//            }
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
           
//            modelBuilder.Entity<AccionControlador>(entity =>
//            {
//                entity.HasKey(e => e.IdAccion)
//                    .HasName("PK__AccionCo__9845169B0050D7E9");

//                entity.Property(e => e.Encabezado)
//                    .IsRequired()
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.NombreAccion)
//                    .IsRequired()
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.HasOne(d => d.IdControladorNavigation)
//                    .WithMany(p => p.AccionControlador)
//                    .HasForeignKey(d => d.IdControlador)
//                    .HasConstraintName("FK__AccionCon__IdCon__48CFD27E");
//            });

//            modelBuilder.Entity<Banco>(entity =>
//            {
//                entity.HasKey(e => e.IdBanco)
//                    .HasName("PK__Bancos__2D3F553E4FB4C327");

//                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

//                entity.Property(e => e.Bancos)
//                    .HasColumnName("Banco")
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.Fal)
//                    .HasColumnName("FAl")
//                    .HasColumnType("smalldatetime")
//                    .HasDefaultValueSql("(getdate())");

//                entity.Property(e => e.Fum)
//                    .HasColumnName("FUM")
//                    .HasColumnType("smalldatetime")
//                    .HasDefaultValueSql("(getdate())");

//                entity.Property(e => e.UsuarioFal).HasColumnName("Usuario_FAl");

//                entity.Property(e => e.UsuarioFum).HasColumnName("Usuario_FUM");
//            });

//            modelBuilder.Entity<BitacoraComercio>(entity =>
//            {
//                entity.HasNoKey();

//                entity.ToTable("Bitacora_Comercio");

//                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

//                entity.Property(e => e.Comentarios)
//                    .HasMaxLength(150)
//                    .IsUnicode(false);

//                entity.Property(e => e.Fal)
//                    .HasColumnName("FAl")
//                    .HasColumnType("smalldatetime")
//                    .HasDefaultValueSql("(getdate())");

//                entity.Property(e => e.Fum)
//                    .HasColumnName("FUM")
//                    .HasColumnType("smalldatetime")
//                    .HasDefaultValueSql("(getdate())");

//                entity.Property(e => e.IdEmisor)
//                    .HasMaxLength(10)
//                    .IsUnicode(false);

//                entity.Property(e => e.UsuarioFal).HasColumnName("Usuario_FAl");

//                entity.Property(e => e.UsuarioFum).HasColumnName("Usuario_FUM");
//            });

//            modelBuilder.Entity<Comercio>(entity =>
//            {
//                entity.HasKey(e => e.Rfc)
//                    .HasName("PK__Comercio__CAFFA85FE9D4C9AE");

//                entity.Property(e => e.Rfc)
//                    .HasColumnName("RFC")
//                    .HasMaxLength(13)
//                    .IsUnicode(false);

//                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

//                entity.Property(e => e.Correo)
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.CuentaDeposito)
//                    .HasMaxLength(20)
//                    .IsUnicode(false);

//                entity.Property(e => e.Direccion)
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.Fal)
//                    .HasColumnName("FAl")
//                    .HasColumnType("smalldatetime")
//                    .HasDefaultValueSql("(getdate())");

//                entity.Property(e => e.Fum)
//                    .HasColumnName("FUM")
//                    .HasColumnType("smalldatetime")
//                    .HasDefaultValueSql("(getdate())");

//                entity.Property(e => e.IdComercio).ValueGeneratedOnAdd();

//                entity.Property(e => e.NombreComercial)
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.NombreCompleto)
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.Portal)
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.RazonSocial)
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.Telefono)
//                    .HasMaxLength(20)
//                    .IsUnicode(false);

//                entity.Property(e => e.UsuarioFal).HasColumnName("Usuario_FAl");

//                entity.Property(e => e.UsuarioFum).HasColumnName("Usuario_FUM");

//                entity.HasOne(d => d.EstatusNavigation)
//                    .WithMany(p => p.Comercio)
//                    .HasForeignKey(d => d.Estatus)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK__Comercios__Estat__1A14E395");

//                entity.HasOne(d => d.IdBancoNavigation)
//                    .WithMany(p => p.Comercio)
//                    .HasForeignKey(d => d.IdBanco)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK__Comercios__IdBan__1920BF5C");

//                entity.HasOne(d => d.IdCompaniaNavigation)
//                    .WithMany(p => p.Comercio)
//                    .HasForeignKey(d => d.IdCompania)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK__Comercio__IdComp__72C60C4A");

//                entity.HasOne(d => d.IdGiroComercioNavigation)
//                    .WithMany(p => p.Comercio)
//                    .HasForeignKey(d => d.IdGiroComercio)
//                    .HasConstraintName("FK__Comercio__IdGiro__38996AB5");

//                entity.HasOne(d => d.IdTipoDepositoNavigation)
//                    .WithMany(p => p.Comercio)
//                    .HasForeignKey(d => d.IdTipoDeposito)
//                    .HasConstraintName("FK__Comercio__IdTipo__1F98B2C1");
//            });

//            modelBuilder.Entity<Compania>(entity =>
//            {
//                entity.HasKey(e => e.IdCompania)
//                    .HasName("PK__Compania__B32BA1C724A60E10");

//                entity.Property(e => e.Companias)
//                    .HasColumnName("Compania")
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.Fal)
//                    .HasColumnName("FAl")
//                    .HasColumnType("smalldatetime")
//                    .HasDefaultValueSql("(getdate())");

//                entity.Property(e => e.Fum)
//                    .HasColumnName("FUM")
//                    .HasColumnType("smalldatetime")
//                    .HasDefaultValueSql("(getdate())");

//                entity.Property(e => e.UsuarioFal).HasColumnName("Usuario_FAl");

//                entity.Property(e => e.UsuarioFum).HasColumnName("Usuario_FUM");
//            });

//            //modelBuilder.Entity<Controlador>(entity =>
//            //{
//            //    entity.HasKey(e => e.IdControlador)
//            //        .HasName("PK__Controla__1344D62B510EC0C5");

//            //    entity.Property(e => e.NombreControlador)
//            //        .HasMaxLength(100)
//            //        .IsUnicode(false);

//            //    entity.Property(e => e.Texto)
//            //        .HasMaxLength(100)
//            //        .IsUnicode(false);

//            //    entity.HasOne(d => d.IdMenuPadreNavigation)
//            //        .WithMany(p => p.NombreMenu)
//            //        .HasForeignKey(d => d.IdMenuPadre)
//            //        .HasConstraintName("FK__Controlad__IdMen__45F365D3");
//            //});

//            modelBuilder.Entity<Documento>(entity =>
//            {
//                entity.HasNoKey();

//                entity.Property(e => e.Activo)
//                    .HasColumnName("activo")
//                    .HasDefaultValueSql("((1))");

//                entity.Property(e => e.Archivo).HasColumnName("archivo");

//                entity.Property(e => e.Extension)
//                    .HasColumnName("extension")
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.IdArchivo).ValueGeneratedOnAdd();

//                entity.Property(e => e.Nombre).HasColumnName("nombre");
//            });

//            modelBuilder.Entity<Estatus>(entity =>
//            {
//                entity.HasKey(e => e.IdEstatus)
//                    .HasName("PK__Estatus__B32BA1C724A60E10");

//                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

//                entity.Property(e => e.Color)
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.Estatus1)
//                    .HasColumnName("Estatus")
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.Fal)
//                    .HasColumnName("FAl")
//                    .HasColumnType("smalldatetime")
//                    .HasDefaultValueSql("(getdate())");

//                entity.Property(e => e.Fum)
//                    .HasColumnName("FUM")
//                    .HasColumnType("smalldatetime")
//                    .HasDefaultValueSql("(getdate())");

//                entity.Property(e => e.UsuarioFal).HasColumnName("Usuario_FAl");

//                entity.Property(e => e.UsuarioFum).HasColumnName("Usuario_FUM");
//            });

//            modelBuilder.Entity<GiroComercio>(entity =>
//            {
//                entity.HasKey(e => e.IdGiroComercio)
//                    .HasName("PK__GiroCome__0DD7D34643705331");

//                entity.Property(e => e.Fal)
//                    .HasColumnName("FAl")
//                    .HasColumnType("smalldatetime")
//                    .HasDefaultValueSql("(getdate())");

//                entity.Property(e => e.Fum)
//                    .HasColumnName("FUM")
//                    .HasColumnType("smalldatetime")
//                    .HasDefaultValueSql("(getdate())");

//                entity.Property(e => e.GiroComercial)
//                    .HasMaxLength(20)
//                    .IsUnicode(false);

//                entity.Property(e => e.UsuarioFal).HasColumnName("Usuario_FAl");

//                entity.Property(e => e.UsuarioFum).HasColumnName("Usuario_FUM");
//            });

//            modelBuilder.Entity<Menu>(entity =>
//            {
//                entity.HasKey(e => e.IdMenu)
//                    .HasName("PK__Menu__4D7EA8E10944283B");

//                entity.Property(e => e.NombreMenu)
//                    .IsRequired()
//                    .HasMaxLength(50)
//                    .IsUnicode(false);
//            });

//            modelBuilder.Entity<Perfil>(entity =>
//            {
//                entity.HasKey(e => e.IdPerfil)
//                    .HasName("PK__Perfil__C7BD5CC123278971");

//                entity.Property(e => e.Descripcion)
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.FechaAlta).HasColumnType("smalldatetime");

//                entity.Property(e => e.FechaUltimaMod).HasColumnType("smalldatetime");

//                entity.Property(e => e.IdUsuarioFA).HasColumnName("IdUsuarioFA");

//                entity.Property(e => e.IdUsuarioFUM).HasColumnName("IdUsuarioFUM");

//                entity.Property(e => e.Nombre)
//                    .HasMaxLength(100)
//                    .IsUnicode(false);
//            });

//            //modelBuilder.Entity<RolControlador>(entity =>
//            //{
//            //    entity.HasKey(e => e.IdRol)
//            //        .HasName("PK__RolContr__2A49584C7468D4B5");

//            //    entity.HasOne(d => d.IdAccionNavigation)
//            //        .WithMany(p => p.RolControlador)
//            //        .HasForeignKey(d => d.IdAccion)
//            //        .OnDelete(DeleteBehavior.ClientSetNull)
//            //        .HasConstraintName("FK__RolContro__IdAcc__4D94879B");

//            //    entity.HasOne(d => d.IdControladorNavigation)
//            //        .WithMany(p => p.RolControlador)
//            //        .HasForeignKey(d => d.IdControlador)
//            //        .OnDelete(DeleteBehavior.ClientSetNull)
//            //        .HasConstraintName("FK__RolContro__IdCon__4CA06362");

//            //    entity.HasOne(d => d.IdPerfilNavigation)
//            //        .WithMany(p => p.pe)
//            //        .HasForeignKey(d => d.IdPerfil)
//            //        .OnDelete(DeleteBehavior.ClientSetNull)
//            //        .HasConstraintName("FK__RolContro__IdPer__4BAC3F29");
//            //});

//            modelBuilder.Entity<TipoDeposito>(entity =>
//            {
//                entity.HasKey(e => e.IdTipoDeposito)
//                    .HasName("PK__TipoDepo__FA4F2115BC4A9FEA");

//                entity.Property(e => e.Fal)
//                    .HasColumnType("smalldatetime")
//                    .HasDefaultValueSql("(getdate())");

//                entity.Property(e => e.Fum)
//                    .HasColumnType("smalldatetime")
//                    .HasDefaultValueSql("(getdate())");

//                entity.Property(e => e.TipoDeposito1)
//                    .HasColumnName("TipoDeposito")
//                    .HasMaxLength(20)
//                    .IsUnicode(false);
//            });

//            modelBuilder.Entity<Usuario>(entity =>
//            {
//                entity.HasKey(e => e.IdUsuario)
//                    .HasName("PK__Usuario__5B65BF9761CFE863");

//                entity.Property(e => e.Apellido)
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.Contrasena).HasMaxLength(32);

//                entity.Property(e => e.Correo)
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.FechaAlta).HasColumnType("smalldatetime");

//                entity.Property(e => e.FechaUltimaMod).HasColumnType("smalldatetime");

//                entity.Property(e => e.IdUsuarioFA).HasColumnName("IdUsuarioFA");

//                entity.Property(e => e.IdUsuarioFUM).HasColumnName("IdUsuarioFUM");

//                entity.Property(e => e.Nombre)
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.Puesto)
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.UserName)
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.Property(e => e.Vigencia).HasColumnType("smalldatetime");

//                //entity.HasOne(d => d.IdCompaniaNavigation)
//                //    .WithMany(p => p.Usuario)
//                //    .HasForeignKey(d => d.IdCompania)
//                //    .HasConstraintName("FK__Usuario__IdCompa__68487DD7");

//                //entity.HasOne(d => d.IdPerfilNavigation)
//                //    .WithMany(p => p.Usuario)
//                //    .HasForeignKey(d => d.IdPerfil)
//                //    .HasConstraintName("FK__Usuario__IdPerfi__412EB0B6");
//            });

//            OnModelCreatingPartial(modelBuilder);
//        }

//        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//    }
//}
