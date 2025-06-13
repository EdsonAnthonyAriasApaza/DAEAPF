using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace DAEAPF.Infrastructure.Models;

public partial class NegociosAppContext : DbContext
{
    public NegociosAppContext()
    {
    }

    public NegociosAppContext(DbContextOptions<NegociosAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriasProducto> CategoriasProductos { get; set; }

    public virtual DbSet<EstadosNegocio> EstadosNegocios { get; set; }

    public virtual DbSet<EstadosProducto> EstadosProductos { get; set; }

    public virtual DbSet<Negocio> Negocios { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=negocios_app;user id=root;password=123456", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<CategoriasProducto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("categorias_productos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<EstadosNegocio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("estados_negocio");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasColumnName("estado");
        });

        modelBuilder.Entity<EstadosProducto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("estados_producto");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasColumnName("estado");
        });

        modelBuilder.Entity<Negocio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("negocios");

            entity.HasIndex(e => e.EstadoId, "estado_id");

            entity.HasIndex(e => e.UsuarioId, "usuario_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categoria)
                .HasMaxLength(100)
                .HasColumnName("categoria");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .HasColumnName("direccion");
            entity.Property(e => e.EstadoId)
                .HasDefaultValueSql("'1'")
                .HasColumnName("estado_id");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Estado).WithMany(p => p.Negocios)
                .HasForeignKey(d => d.EstadoId)
                .HasConstraintName("negocios_ibfk_2");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Negocios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("negocios_ibfk_1");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("productos");

            entity.HasIndex(e => e.CategoriaId, "categoria_id");

            entity.HasIndex(e => e.EstadoId, "estado_id");

            entity.HasIndex(e => e.NegocioId, "negocio_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.EstadoId)
                .HasDefaultValueSql("'1'")
                .HasColumnName("estado_id");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(255)
                .HasColumnName("imagen_url");
            entity.Property(e => e.NegocioId).HasColumnName("negocio_id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("productos_ibfk_2");

            entity.HasOne(d => d.Estado).WithMany(p => p.Productos)
                .HasForeignKey(d => d.EstadoId)
                .HasConstraintName("productos_ibfk_3");

            entity.HasOne(d => d.Negocio).WithMany(p => p.Productos)
                .HasForeignKey(d => d.NegocioId)
                .HasConstraintName("productos_ibfk_1");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Correo, "correo").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContrasenaHash)
                .HasMaxLength(255)
                .HasColumnName("contrasena_hash");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Rol)
                .HasDefaultValueSql("'cliente'")
                .HasColumnType("enum('cliente','negocio','admin')")
                .HasColumnName("rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
