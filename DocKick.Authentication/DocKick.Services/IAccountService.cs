using System.Threading.Tasks;
using DocKick.DataTransferModels.User;

namespace DocKick.Services
{
    public interface IAccountService
    {
        Task<UserProfileModel> GetProfile(string email);
        Task<UserProfileModel> UpdateProfile(string email, UserProfileRequest model);
    }
}