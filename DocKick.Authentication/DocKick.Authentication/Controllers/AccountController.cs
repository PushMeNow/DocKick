﻿using System.Threading.Tasks;
using DocKick.Authentication.Models;
using DocKick.DataTransferModels.User;
using DocKick.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Authentication.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("google-login")]
        public async Task<AuthenticatedUserResult> GoogleLogin([FromBody] UserAuthModel model)
        {
            var result = await _accountService.Authenticate(model.TokenId);

            return result;
        }
    }
}