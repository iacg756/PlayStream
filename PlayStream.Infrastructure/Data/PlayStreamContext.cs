using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PlayStream.Core.Entities;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace PlayStream.Infrastructure.Data;

public partial class PlayStreamContext : DbContext
{
    public PlayStreamContext()
    {
    }

    public PlayStreamContext(DbContextOptions<PlayStreamContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Calificacion> Calificacions { get; set; }

    public virtual DbSet<Contenido> Contenidos { get; set; }

    public virtual DbSet<Favorito> Favoritos { get; set; }

    public virtual DbSet<Perfil> Perfils { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=PlayStreamDB;uid=root;pwd=password", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.11.14-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Calificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("calificacion");

            entity.HasIndex(e => e.ContenidoId, "FK_Calificacion_Contenido");

            entity.HasIndex(e => e.PerfilId, "FK_Calificacion_Perfil");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Comentario).HasMaxLength(500);
            entity.Property(e => e.ContenidoId).HasColumnType("int(11)");
            entity.Property(e => e.PerfilId).HasColumnType("int(11)");
            entity.Property(e => e.Puntuacion).HasColumnType("int(11)");

            entity.HasOne(d => d.Contenido).WithMany(p => p.Calificacions)
                .HasForeignKey(d => d.ContenidoId)
                .HasConstraintName("FK_Calificacion_Contenido");

            entity.HasOne(d => d.Perfil).WithMany(p => p.Calificacions)
                .HasForeignKey(d => d.PerfilId)
                .HasConstraintName("FK_Calificacion_Perfil");
        });

        modelBuilder.Entity<Contenido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("contenido");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.AnioLanzamiento).HasColumnType("int(11)");
            entity.Property(e => e.Categoria).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.Titulo).HasMaxLength(200);
        });

        modelBuilder.Entity<Favorito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("favorito");

            entity.HasIndex(e => e.ContenidoId, "FK_Favorito_Contenido");

            entity.HasIndex(e => e.PerfilId, "FK_Favorito_Perfil");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.ContenidoId).HasColumnType("int(11)");
            entity.Property(e => e.PerfilId).HasColumnType("int(11)");

            entity.HasOne(d => d.Contenido).WithMany(p => p.Favoritos)
                .HasForeignKey(d => d.ContenidoId)
                .HasConstraintName("FK_Favorito_Contenido");

            entity.HasOne(d => d.Perfil).WithMany(p => p.Favoritos)
                .HasForeignKey(d => d.PerfilId)
                .HasConstraintName("FK_Favorito_Perfil");
        });

        modelBuilder.Entity<Perfil>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("perfil");

            entity.HasIndex(e => e.UsuarioId, "FK_Perfil_Usuario");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.AvatarUrl).HasMaxLength(255);
            entity.Property(e => e.NombrePerfil).HasMaxLength(50);
            entity.Property(e => e.UsuarioId).HasColumnType("int(11)");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Perfils)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_Perfil_Usuario");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("rol");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Correo, "Correo").IsUnique();

            entity.HasIndex(e => e.RolId, "FK_Usuario_Rol");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.RolId).HasColumnType("int(11)");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
