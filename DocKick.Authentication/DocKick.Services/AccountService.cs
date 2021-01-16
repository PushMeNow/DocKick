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

        public async Task<UserProfileModel> GetProfile(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            ExceptionHelper.ThrowNotFoundIfNull(user, "User");

            return _mapper.Map<UserProfileModel>(user);
        }
    }
}