using System.Threading.Tasks;
using DocKick.DataTransferModels.User;

namespace DocKick.Services
{
    public interface IAccountService
    {
        Task<AuthenticatedUserResult> Authenticate(string token);
    }
}