using System.Threading.Tasks;
using DocKick.Services.Models;
using Microsoft.AspNetCore.Identity;

namespace DocKick.Services
{
    public interface IAccountService
    {
        Task<ExternalUserInfoModel> GetUserInfoFromExternalCallback();
        Task<IdentityResult> ExternalLogin(ExternalUserInfoModel model);
    }
}