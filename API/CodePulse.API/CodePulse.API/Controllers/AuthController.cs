﻿using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,
            ITokenRepository tokenRepository
            )
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var identityUser = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (identityUser is not null)
            {
                var CheckPasswordResult = await _userManager.CheckPasswordAsync(identityUser, loginRequest.Password);

                if (CheckPasswordResult)
                {

                    var roles = await _userManager.GetRolesAsync(identityUser);

                    var jwtToken = _tokenRepository.CreateJwtToken(identityUser, roles.ToList());

                    var response = new LoginResponseDto
                    {
                        Email = loginRequest.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken

                    };
                    return Ok(response);
                }
            }

            ModelState.AddModelError("", "Email or Password is incorrect");
            return BadRequest();
                
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
        {
            // Create an identityUser object

            var user = new IdentityUser
            {
                UserName = registerRequest.Email?.Trim(),
                Email = registerRequest.Email?.Trim(),
            };

            var identityResult = await _userManager.CreateAsync(user, registerRequest.Password);

            if (identityResult.Succeeded)
            {
                // Add role to user (Reader)
                identityResult = await _userManager.AddToRoleAsync(user, "Reader");

                if (identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }
    }
}
