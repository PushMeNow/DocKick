using System;
using System.Threading.Tasks;
using DocKick.DataTransferModels.Users;

namespace DocKick.Services
{
    public interface IAccountService
    {
        Task<UserProfileModel> GetProfile(Guid userId);
        Task<UserProfileModel> UpdateProfile(Guid userId, UserProfileRequest model);
    }
}