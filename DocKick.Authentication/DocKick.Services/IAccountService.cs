using System;
using System.Threading.Tasks;
using DocKick.DataTransferModels.User;

namespace DocKick.Services
{
    public interface IAccountService
    {
        Task<UserProfileModel> GetProfile(Guid userId);
    }
}