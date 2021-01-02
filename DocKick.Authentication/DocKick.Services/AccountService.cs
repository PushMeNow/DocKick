using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DocKick.DataTransferModels.User;
using DocKick.Entities.Users;
using DocKick.Exceptions;
using DocKick.Services.Constants;
using DocKick.Services.Settings;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DocKick.Services
{
    public class AccountService : IAccountService
    {
        private readonly AuthSettings _authSettings;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AccountService(AuthSettings authSettings,
                              UserManager<User> userManager,
                              IMapper mapper)
        {
            _authSettings = authSettings;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AuthenticatedUserResult> Authenticate(string token)
        {
            ExceptionHelper.ThrowArgumentNullIfNull(token, nameof(token));

            var payload = await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings());
            var user = await _userManager.FindByEmailAsync(payload.Email);

            if (user is null)
            {
                user = new User
                       {
                           Email = payload.Email,
                           UserName = payload.Email
                       };

                var result = await _userManager.CreateAsync(user);

                ExceptionHelper.ThrowIfTrue<AuthenticationException>(!result.Succeeded);
            }

            return GetAuthenticatedUserResult(user);
        }

        public async Task<AuthenticatedUserResult> Authenticate(InternalUserAuthModel model)
        {
            ExceptionHelper.ThrowArgumentNullIfNull(model, nameof(model));

            var user = await _userManager.FindByEmailAsync(model.Email);

            ExceptionHelper.ThrowIfNull<AuthenticationException>(user);

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            ExceptionHelper.ThrowIfTrue<AuthenticationException>(!result);

            return GetAuthenticatedUserResult(user);
        }

        public async Task<UserProfileModel> GetProfile(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            ExceptionHelper.ThrowNotFoundIfNull(user, "User");

            return _mapper.Map<UserProfileModel>(user);
        }

        public async Task<AuthenticatedUserResult> SignUp(SignUpModel model)
        {
            ExceptionHelper.ThrowArgumentNullIfNull(model, nameof(model));

            var checkUser = await _userManager.FindByEmailAsync(model.Email);
            
            ExceptionHelper.ThrowIfNotNull<AuthenticationException>(checkUser);

            var user = new User
                       {
                           Email = model.Email,
                           UserName = model.Email
                       };

            var createResult = await _userManager.CreateAsync(user, model.Password);

            ExceptionHelper.ThrowIfTrue<AuthenticationException>(!createResult.Succeeded);

            return GetAuthenticatedUserResult(user);
        }

        private AuthenticatedUserResult GetAuthenticatedUserResult(User user)
        {
            var claims = new[]
                         {
                             new Claim(JwtRegisteredClaimNames.Email, user.Email),
                             new Claim(ClaimNames.UserId, user.Id.ToString())
                         };
            
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authSettings.TokenSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(string.Empty,
                                             string.Empty,
                                             claims,
                                             expires: DateTime.Now.AddSeconds(55 * 60),
                                             signingCredentials: credentials);

            var handler = new JwtSecurityTokenHandler();

            var authenticatedUser = new AuthenticatedUserResult
                                    {
                                        Email = user.Email,
                                        Token = handler.WriteToken(token)
                                    };

            return authenticatedUser;
        }
    }
}