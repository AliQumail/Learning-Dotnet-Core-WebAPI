using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager; 
        private readonly ITokenRepository tokenRepository;
        public AuthController(UserManager<IdentityUser> _userManager, ITokenRepository _tokenRepository)
        {
            this.userManager = _userManager;
            this.tokenRepository = _tokenRepository; 
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)  
        {
            var identityUser = new IdentityUser()
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded) 
            {
                // Add roles 
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any()) 
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded) 
                    {
                        return Ok("User has been registered");
                    }  
                }
            }

            return BadRequest("Something went wrong");

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto) 
        {
           var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
           if (user != null)
           {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPasswordResult) 
                {
                    var roles = await userManager.GetRolesAsync(user);
                    var token = tokenRepository.CreateJwtToken(user, roles.ToList());
                    var loginResponseDto = new LoginResponseDto()
                    {
                        JwtToken = token,
                    };

                    return Ok(loginResponseDto);
                }

           }
           return BadRequest("Username or Password is incorrect");
        }
    }
}
