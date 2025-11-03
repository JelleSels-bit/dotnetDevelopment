using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StartSpelerAPI.Data.UnitOfWork;
using StartSpelerAPI.Dto.Gebruiker;
using StartSpelerAPI.Helpers;

namespace StartSpelerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GebruikerController : ControllerBase
    {
        private readonly UserManager<Gebruiker> _userManager;
        private readonly SignInManager<Gebruiker> _signInManager;
        private readonly IMapper _mapper;

        public GebruikerController(UserManager<Gebruiker> userManager, SignInManager<Gebruiker> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegistratieDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var gebruiker = await _userManager.FindByEmailAsync(dto.Email);
            if (gebruiker == null)
            {
                var user = new Gebruiker
                {
                    Voornaam = dto.Voornaam,
                    Familienaam = dto.Familienaam,
                    Geboortedatum = dto.Geboortedatum,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                var result = await _userManager.CreateAsync(user, dto.Password);

                var hasAdminUser = await _userManager.GetUsersInRoleAsync("");

                if (hasAdminUser.Count == 0)
                    await _userManager.AddToRoleAsync(user, "admin");
                else
                    await _userManager.AddToRoleAsync(user, "user");

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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user != null && !user.EmailConfirmed)
            {
                ModelState.AddModelError("message", "Emailadres is nog niet bevestigd.");
                return BadRequest(dto);
            }
            if (await _userManager.CheckPasswordAsync(user, dto.Password) == false)
            {
                ModelState.AddModelError("message", "Verkeerde logincombinatie!");
                return BadRequest(dto);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, dto.Password, false, true);

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
