using System.Threading.Tasks;
using DocKick.DataTransferModels.Users;
using Microsoft.AspNetCore.Authentication;

namespace DocKick.Services
{
    public interface IAuthService
    {
        Task<bool> Login(SingInModel model);

        Task<bool> SignUp(SignUpModel model);

        Task<string> Logout(string logoutId, string subjectId, string displayName);
        Task<string> ExternalLogin();
        AuthenticationProperties PrepareAuthProperties(string provider, string returnUrl, string redirectUrl);
    }
}