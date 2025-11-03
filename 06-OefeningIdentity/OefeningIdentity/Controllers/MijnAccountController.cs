using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OefeningIdentity.Helper;
using OefeningIdentity.Models;

namespace OefeningIdentity.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MijnAccountController : ControllerBase
    {
        private UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;

        public MijnAccountController(UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register"), AllowAnonymous]
        public async Task<IActionResult> Register(RegistratieDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var gebruiker = await _userManager.FindByEmailAsync(request.Email);
            if (gebruiker == null)
            {
                var user = new CustomUser
                {
                    UserName = request.Email,
                    NormalizedUserName = request.Email,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                //await _userManager.AddToRoleAsync(user, request.Rol);

                if (result.Succeeded)
                    return Ok();
                else
                {
                    if (result.Errors.Count() > 0)
                    {
                        foreach (var error in result.Errors)
                            ModelState.AddModelError("message", error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }
            else
            {
                ModelState.AddModelError("message", "Gebruiker is aanwezig is database.");
                return BadRequest(ModelState);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && !user.EmailConfirmed)
            {
                ModelState.AddModelError("message", "Emailadres is nog niet bevestigd.");
                return BadRequest(model);
            }
            if (await _userManager.CheckPasswordAsync(user, model.Password) == false)
            {
                // Nooit exacte informatie geven: zeg alleen dat combinatie vekeerd is...
                ModelState.AddModelError("message", "Verkeerde logincombinatie!");
                return BadRequest(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, true);

            if (result.IsLockedOut)
                ModelState.AddModelError("message", "Account geblokkeerd!!");
            if (result.Succeeded)
            {
                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles != null)
                {
                    foreach (var userRole in userRoles)
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = Token.GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            ModelState.AddModelError("message", "Ongeldige loginpoging");
            return Unauthorized(ModelState);
        }


    }
}
