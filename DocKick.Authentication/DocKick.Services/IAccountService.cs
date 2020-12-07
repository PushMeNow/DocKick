using System.Threading.Tasks;
using DocKick.Services.Models;

namespace DocKick.Services
{
    public interface IAccountService
    {
        Task<ExternalUserInfoModel> ExternalSignIn();
    }
}