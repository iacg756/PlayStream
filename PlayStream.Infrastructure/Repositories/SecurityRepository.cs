using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlayStream.Core.Entities;
using PlayStream.Core.Interfaces;
using PlayStream.Infrastructure.Data;

namespace PlayStream.Infrastructure.Repositories
{
    public class SecurityRepository : BaseRepository<Security>, ISecurityRepository
    {
        public SecurityRepository(PlayStreamContext context)
            : base(context)
        {
        }

        public async Task<Security?> GetLoginByCredentials(UserLogin userLogin)
        {
            return await _entities.FirstOrDefaultAsync(
                x => x.Login == userLogin.User
            );
        }
    }
}