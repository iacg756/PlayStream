using PlayStream.Core.Entities;
using System.Data;

namespace PlayStream.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Usuario> UsuarioRepository { get; }
        IRepository<Contenido> ContenidoRepository { get; }
        IRepository<Perfil> PerfilRepository { get; }
        IRepository<Favorito> FavoritoRepository { get; }
        IRepository<Calificacion> CalificacionRepository { get; }

        void SaveChanges();
        Task SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        IDbConnection? GetDbConnection();
        IDbTransaction? GetDbTransaction();
    }
}