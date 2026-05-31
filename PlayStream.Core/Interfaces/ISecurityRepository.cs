using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayStream.Core.Entities;
using PlayStream.Core.DTOs;

namespace PlayStream.Core.Interfaces
{
    public interface ISecurityRepository : IRepository<Security>
    {
        Task<Security?> GetLoginByCredentials(UserLogin login);
    }
}