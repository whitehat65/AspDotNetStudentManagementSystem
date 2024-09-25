using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentManagementSystem.Models.DTOs;
using StudentManagementSystem.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Security.Claims;
using System.Text;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };
            var identityResult = await userManager.CreateAsync(identityUser,registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded) 
                    {
                        return Ok("User Registered Successfully Please Login");
                    }
                }
                
            }
            return BadRequest("Something Went Wrong");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.UserName);

            if (user != null && await userManager.CheckPasswordAsync(user, loginRequestDto.Password))
            {
                // User credentials are valid, generate JWT token
                var roles = await userManager.GetRolesAsync(user);
                if (roles != null) 
                {
                    var jwtToken = tokenRepository.GenerateJwtToken(user, roles.ToList());


                    var response = new LoginResponseDto
                    {
                        JwtToken = jwtToken,
                        Message = "Login Successfully and Token Expire in 15 minutes"
                    };

                    return Ok(response);
                }
                
            }

            return Unauthorized("Invalid Username or Password.");
        }
    }
}
