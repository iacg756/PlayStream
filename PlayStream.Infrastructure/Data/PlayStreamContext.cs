using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PlayStream.Core.Entities;

namespace PlayStream.Infrastructure.Data;

public partial class PlayStreamContext : DbContext
{
    public PlayStreamContext() { }

    public PlayStreamContext(DbContextOptions<PlayStreamContext> options)
        : base(options) { }

    public virtual DbSet<Calificacion> Calificacions { get; set; }
    public virtual DbSet<Contenido> Contenidos { get; set; }
    public virtual DbSet<Favorito> Favoritos { get; set; }
    public virtual DbSet<Perfil> Perfils { get; set; }
    public virtual DbSet<Rol> Rols { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}