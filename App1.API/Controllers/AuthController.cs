using App1.API.Models.DTOs;
using App1.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> login(LoginRequestDto loginRequestDto) 
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Email);
            if(user is not null)
            {
                var passwordResult = await userManager.CheckPasswordAsync(user,loginRequestDto.Password);
                if (passwordResult)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    string token = tokenRepository.generateJwtToken(user, roles.ToList());
                    var response = new LoginResponseDto
                    {
                        Email = loginRequestDto.Email,
                        token=token,
                        roles=roles.ToList()
                    };
                    return Ok(response);
                }
            }
            ModelState.AddModelError("", "Incorrect Email or password");
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> register(RegistrationRequestDto registrationRequestDto)
        {
            var user = new IdentityUser
            {
                Email = registrationRequestDto.Email.Trim(),
                UserName = registrationRequestDto.Email.Trim(),
            };
            var createdUser = await userManager.CreateAsync(user,registrationRequestDto.Password);
            if (createdUser.Succeeded)
            {
                createdUser = await userManager.AddToRoleAsync(user, "Reader");


                if (createdUser.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (createdUser.Errors.Any())
                    {
                        foreach (var error in createdUser.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if (createdUser.Errors.Any())
                {
                    foreach (var error in createdUser.Errors)
                    {
                      
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return ValidationProblem(ModelState);
        }
    }
}

