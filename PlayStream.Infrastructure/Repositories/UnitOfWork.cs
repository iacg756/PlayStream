using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PlayStream.Core.Entities;
using PlayStream.Core.Interfaces;
using PlayStream.Infrastructure.Data;
using System.Data;

namespace PlayStream.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlayStreamContext _context;
        private readonly IDapperContext _dapper;
        private IDbContextTransaction? _efTransaction;

        private IRepository<Usuario> _usuarioRepository;
        private IRepository<Contenido> _contenidoRepository;
        private IRepository<Perfil> _perfilRepository;
        private IRepository<Favorito> _favoritoRepository;
        private IRepository<Calificacion> _calificacionRepository;

        public UnitOfWork(PlayStreamContext context, IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }

        public IRepository<Usuario> UsuarioRepository => _usuarioRepository ??= new BaseRepository<Usuario>(_context);
        public IRepository<Contenido> ContenidoRepository => _contenidoRepository ??= new BaseRepository<Contenido>(_context);
        public IRepository<Perfil> PerfilRepository => _perfilRepository ??= new BaseRepository<Perfil>(_context);
        public IRepository<Favorito> FavoritoRepository => _favoritoRepository ??= new BaseRepository<Favorito>(_context);
        public IRepository<Calificacion> CalificacionRepository => _calificacionRepository ??= new BaseRepository<Calificacion>(_context);

        public async Task BeginTransactionAsync()
        {
            _efTransaction = await _context.Database.BeginTransactionAsync();
            _dapper.SetAmbientConnection(_context.Database.GetDbConnection(), _efTransaction.GetDbTransaction());
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_efTransaction != null) await _efTransaction.CommitAsync();
            }
            finally
            {
                _dapper.ClearAmbientConnection();
            }
        }

        public async Task RollbackAsync()
        {
            if (_efTransaction != null) await _efTransaction.RollbackAsync();
            _dapper.ClearAmbientConnection();
        }

        public void SaveChanges() => _context.SaveChanges();
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
        public IDbConnection? GetDbConnection() => _context.Database.GetDbConnection();
        public IDbTransaction? GetDbTransaction() => _efTransaction?.GetDbTransaction();
    }
}