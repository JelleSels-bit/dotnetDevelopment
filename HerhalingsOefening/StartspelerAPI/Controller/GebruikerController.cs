using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using StartspelerAPI.DTO.Gebruiker;

namespace StartspelerAPI.Controller
{
    public class GebruikerController : ControllerBase
    {
        private readonly UserManager<Gebruiker> _userManager;
        private readonly SignInManager<Gebruiker> _signInManager;
        private readonly RoleManager<Gebruiker> _roleManager;
        private readonly IMapper _mapper;

        public GebruikerController(UserManager<Gebruiker> userManager, SignInManager<Gebruiker> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(GebruikerRegistratieDto dto)
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
                    UserName = dto.Email,
                    Geboortedatum = dto.Geboortedatum,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                var result = await _userManager.CreateAsync(user, dto.Password);

                var hasAdminUser = await _userManager.GetUsersInRoleAsync("admin");

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
        [HttpPost("Login")]
        public async Task<IActionResult> Login(GebruikerLoginDTO dto)
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
    

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("CreateCommunityManager")]
        public async Task<IActionResult> CreateCommunityManager([FromBody] GebruikerRegistratieDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 1. Check of email al bestaat
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return BadRequest("Gebruiker met dit emailadres bestaat al.");

            // 2. Nieuwe user aanmaken
            var user = new Gebruiker
            {
                UserName = dto.Email,
                Email = dto.Email,
                Voornaam = dto.Voornaam,
                Familienaam = dto.Familienaam,
                PhoneNumber = dto.PhoneNumber,
                EmailConfirmed = true // indien je geen verificatie gebruikt
            };

            var createResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
                return BadRequest(createResult.Errors);

            // 3. Rol "communitymanager" toevoegen
            var roleExists = await _roleManager.RoleExistsAsync("communitymanager");
            if (!roleExists)
                return BadRequest("Rol 'communitymanager' bestaat niet. Seed eerst de rollen.");

            var roleResult = await _userManager.AddToRoleAsync(user, "communitymanager");
            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);

            return Ok($"Gebruiker {user.Email} is succesvol aangemaakt als Community Manager.");
        }

    }
}
