using System;
using System.Collections.Generic;
using Infraestructure.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Infraestructure.Context;

public partial class TransactivaDbContext : DbContext
{
    public TransactivaDbContext()
    {
    }

    public TransactivaDbContext(DbContextOptions<TransactivaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Boletum> Boleta { get; set; }

    public virtual DbSet<Envio> Envios { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Pedidosproducto> Pedidosproductos { get; set; }

    public virtual DbSet<Preparacion> Preparacions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userprofile> Userprofiles { get; set; }

    public virtual DbSet<Usertype> Usertypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Boletum>(entity =>
        {
            entity.HasKey(e => e.IdBoleta).HasName("PRIMARY");

            entity.ToTable("boleta");

            entity.Property(e => e.IdBoleta).HasColumnType("int(11)");
            entity.Property(e => e.Ruta).HasMaxLength(255);
        });

        modelBuilder.Entity<Envio>(entity =>
        {
            entity.HasKey(e => e.IdEnvio).HasName("PRIMARY");

            entity.ToTable("envio");

            entity.Property(e => e.IdEnvio).HasColumnType("int(11)");
            entity.Property(e => e.Asesor).HasMaxLength(100);
            entity.Property(e => e.DireccionEnvio).HasMaxLength(255);
            entity.Property(e => e.DireccionRecojo).HasMaxLength(255);
            entity.Property(e => e.Estado).HasDefaultValueSql("'0'");
            entity.Property(e => e.FechaLlegada).HasColumnType("datetime");
            entity.Property(e => e.Llegada)
                .HasDefaultValueSql("'0'")
                .HasColumnName("LLegada");
            entity.Property(e => e.NombreEmpresa).HasMaxLength(100);
            entity.Property(e => e.NroGuia).HasMaxLength(50);
            entity.Property(e => e.NumeroTelefonico).HasMaxLength(20);
            entity.Property(e => e.RucEmpresa).HasMaxLength(11);
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PRIMARY");

            entity.ToTable("pago");

            entity.HasIndex(e => e.IdBoleta, "IdBoleta");

            entity.Property(e => e.IdPago).HasColumnType("int(11)");
            entity.Property(e => e.Estado).HasDefaultValueSql("'0'");
            entity.Property(e => e.FechaPago).HasColumnType("datetime");
            entity.Property(e => e.IdBoleta).HasColumnType("int(11)");
            entity.Property(e => e.Monto).HasPrecision(10, 2);

            entity.HasOne(d => d.IdBoletaNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdBoleta)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("pago_ibfk_1");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PRIMARY");

            entity.ToTable("pedido");

            entity.HasIndex(e => e.IdComprador, "IdComprador");

            entity.HasIndex(e => e.IdPedidosProductos, "IdPedidosProductos");

            entity.HasIndex(e => e.IdProveedor, "IdProveedor");

            entity.Property(e => e.IdPedido).HasColumnType("int(11)");
            entity.Property(e => e.Estado).HasDefaultValueSql("'0'");
            entity.Property(e => e.IdComprador).HasColumnType("int(11)");
            entity.Property(e => e.IdPedidosProductos).HasColumnType("int(11)");
            entity.Property(e => e.IdProveedor).HasColumnType("int(11)");

            entity.HasOne(d => d.IdCompradorNavigation).WithMany(p => p.PedidoIdCompradorNavigations)
                .HasForeignKey(d => d.IdComprador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pedido_ibfk_2");

            entity.HasOne(d => d.IdPedidosProductosNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdPedidosProductos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pedido_ibfk_3");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.PedidoIdProveedorNavigations)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pedido_ibfk_1");
        });

        modelBuilder.Entity<Pedidosproducto>(entity =>
        {
            entity.HasKey(e => e.IdPedidosProductos).HasName("PRIMARY");

            entity.ToTable("pedidosproducto");

            entity.HasIndex(e => e.IdPago, "IdPago");

            entity.HasIndex(e => e.IdPreparacion, "IdPreparacion");

            entity.Property(e => e.IdPedidosProductos).HasColumnType("int(11)");
            entity.Property(e => e.Cantidad).HasColumnType("int(11)");
            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.DireccionEntrega).HasMaxLength(255);
            entity.Property(e => e.FechaLlegadaAcordada).HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitada).HasColumnType("datetime");
            entity.Property(e => e.IdPago).HasColumnType("int(11)");
            entity.Property(e => e.IdPreparacion).HasColumnType("int(11)");
            entity.Property(e => e.NombreTransaccion).HasMaxLength(255);
            entity.Property(e => e.Producto).HasMaxLength(255);

            entity.HasOne(d => d.IdPagoNavigation).WithMany(p => p.Pedidosproductos)
                .HasForeignKey(d => d.IdPago)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("pedidosproducto_ibfk_1");

            entity.HasOne(d => d.IdPreparacionNavigation).WithMany(p => p.Pedidosproductos)
                .HasForeignKey(d => d.IdPreparacion)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("pedidosproducto_ibfk_2");
        });

        modelBuilder.Entity<Preparacion>(entity =>
        {
            entity.HasKey(e => e.IdPreparacion).HasName("PRIMARY");

            entity.ToTable("preparacion");

            entity.HasIndex(e => e.IdEnvio, "IdEnvio");

            entity.Property(e => e.IdPreparacion).HasColumnType("int(11)");
            entity.Property(e => e.ComoEnvia).HasMaxLength(255);
            entity.Property(e => e.Detalles).HasColumnType("text");
            entity.Property(e => e.Estado).HasDefaultValueSql("'0'");
            entity.Property(e => e.IdEnvio).HasColumnType("int(11)");

            entity.HasOne(d => d.IdEnvioNavigation).WithMany(p => p.Preparacions)
                .HasForeignKey(d => d.IdEnvio)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("preparacion_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.HasIndex(e => e.UserTypeId, "UserTypeId");

            entity.Property(e => e.UserId).HasColumnType("int(11)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FailedLoginAttempts).HasColumnType("int(11)");
            entity.Property(e => e.LockoutUntil).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.UserTypeId).HasColumnType("int(11)");

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserTypeId)
                .HasConstraintName("users_ibfk_1");
        });

        modelBuilder.Entity<Userprofile>(entity =>
        {
            entity.HasKey(e => e.UserProfileId).HasName("PRIMARY");

            entity.ToTable("userprofile");

            entity.HasIndex(e => e.ManagerEmail, "ManagerEmail").IsUnique();

            entity.HasIndex(e => e.UserId, "UserId").IsUnique();

            entity.Property(e => e.UserProfileId).HasColumnType("int(11)");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ManagerDni).HasMaxLength(8);
            entity.Property(e => e.ManagerEmail).HasMaxLength(150);
            entity.Property(e => e.ManagerName).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PaymentPassword)
                .HasMaxLength(255)
                .HasColumnName("Payment_password");
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Ruc).HasMaxLength(11);
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.User).WithOne(p => p.Userprofile)
                .HasForeignKey<Userprofile>(d => d.UserId)
                .HasConstraintName("userprofile_ibfk_1");
        });

        modelBuilder.Entity<Usertype>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("PRIMARY");

            entity.ToTable("usertype");

            entity.Property(e => e.UserTypeId).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
