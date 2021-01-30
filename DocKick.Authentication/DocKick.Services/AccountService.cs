using System;
using System.Threading.Tasks;
using AutoMapper;
using DocKick.DataTransferModels.Users;
using DocKick.Entities.Users;
using DocKick.Exceptions;
using DocKick.Extensions;
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

        public async Task<UserProfileModel> UpdateProfile(string email, UserProfileRequest model)
        {
            var user = await _userManager.FindByEmailAsync(email);

            ExceptionHelper.ThrowNotFoundIfNull(user, "User");

            var mappedUser = _mapper.Map(model, user);

            var identityResult = await _userManager.UpdateAsync(mappedUser);
            
            ExceptionHelper.ThrowParameterInvalidIfTrue(!identityResult.Succeeded, identityResult.CombineErrors());

            return _mapper.Map<UserProfileModel>(mappedUser);
        }
    }
}