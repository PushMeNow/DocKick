using System;
using System.Threading.Tasks;
using DocKick.DataTransferModels.User;
using Microsoft.AspNetCore.Authentication;

namespace DocKick.Services
{
    public interface IAuthService
    {
        Task<bool> Login(InternalUserAuthModel model);

        Task<AuthenticatedUserResult> SignUp(SignUpModel model);

        Task<string> Logout(string logoutId, string subjectId, string displayName);
        Task<string> ExternalLogin();
        AuthenticationProperties PrepareAuthProperties(string provider, string returnUrl, string redirectUrl);
    }
}