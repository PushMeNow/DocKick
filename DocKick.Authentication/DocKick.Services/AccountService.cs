using System;
using System.Threading.Tasks;
using AutoMapper;
using DocKick.DataTransferModels.User;
using DocKick.Entities.Users;
using DocKick.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace DocKick.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AccountService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserProfileModel> GetProfile(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            ExceptionHelper.ThrowNotFoundIfNull(user, "User");

            return _mapper.Map<UserProfileModel>(user);
        }
    }
}