using System;
using System.Threading.Tasks;
using DocKick.DataTransferModels.User;

namespace DocKick.Services
{
    public interface IAuthService
    {
        Task<AuthenticatedUserResult> Authenticate(string token);

        Task<bool> Authenticate(InternalUserAuthModel model);

        Task<AuthenticatedUserResult> SignUp(SignUpModel model);

        Task<AuthenticatedUserResult> RefreshToken(RefreshTokenModel model);
        Task<string> Logout(string logoutId, string subjectId, string displayName);
    }
}