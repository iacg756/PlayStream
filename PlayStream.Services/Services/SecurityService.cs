using PlayStream.Core.Entities;
using PlayStream.Core.DTOs;
using PlayStream.Core.Interfaces;
using PlayStream.Services.Interfaces;

namespace PlayStream.Services.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SecurityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Security> GetLoginByCredentials(UserLogin userLogin)
        {
            return await _unitOfWork.SecurityRepository.GetLoginByCredentials(userLogin);
        }

        public async Task RegisterUser(Security security)
        {
            await _unitOfWork.SecurityRepository.AddAsync(security);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}