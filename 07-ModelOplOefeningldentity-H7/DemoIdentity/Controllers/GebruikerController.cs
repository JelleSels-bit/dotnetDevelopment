using DemoIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using DemoIdentity.Helper;
using Microsoft.AspNetCore.Http.HttpResults;
using IdentityModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DemoIdentity.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class GebruikerController : ControllerBase
    {
        private UserManager<CustomUser> _userManager;
        private SignInManager<CustomUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        public GebruikerController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<CustomUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // Registeren als nieuwe gebruiker
        [HttpPost("register"), AllowAnonymous]
        public async Task<IActionResult> Register(RegistratieModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           
            var gebruiker = await _userManager.FindByEmailAsync(request.Email);

            if (gebruiker != null)
            {
                ModelState.AddModelError("message", "Gebruiker is aanwezig is database.");
                return BadRequest(ModelState);
            }

            var user = new CustomUser {
                    UserName = request.Email,
                    NormalizedUserName = request.Email,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return Ok();
                }
                else
                {
                    if (result.Errors.Count() > 0)
                    {
                        foreach (var error in result.Errors)
                            ModelState.AddModelError("message", error.Description);
                    }
                    return BadRequest(ModelState);
                }
            } catch (Exception ex)
            {
                throw;
            }
        }

        // Login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && !user.EmailConfirmed)
            {
                ModelState.AddModelError("message", "Emailadres is nog niet bevestigd.");
                return BadRequest(ModelState);
            }
            if (await _userManager.CheckPasswordAsync(user, model.Password) == false)
            {
                // Nooit exacte informatie geven: zeg alleen dat combinatie vekeerd is...
                ModelState.AddModelError("message", "Verkeerde logincombinatie!");
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);
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

                if (await _userManager.IsInRoleAsync(user, "Superadmin"))
                    authClaims.Add(new Claim(ClaimTypes.Role, "Superadmin"));
                else if (await _userManager.IsInRoleAsync(user, "Admin"))
                    authClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
                else
                    authClaims.Add(new Claim(ClaimTypes.Role, "User"));

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

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost("GetAlleUsersMetRollen")]
        public async Task<IActionResult> GetAlleUsersMetRollen()
        {
            var users = await _userManager.Users.ToListAsync();

            var userWithRolesList = new List<UserWithRoles>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userWithRolesList.Add(new UserWithRoles
                {
                    User = new BeperkteUser 
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName
                    },
                    Roles = roles
                });
            }

            return Ok(userWithRolesList);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost("GrantPermissions")]
        public async Task<IActionResult> GrantPermissions(GrantPermissionsModel gpm)
        {
               CustomUser? gb = await _userManager.FindByEmailAsync(gpm.Email); // Gebruiker zoeken via email opgegeven
               IdentityRole? rol = await _roleManager.FindByNameAsync(gpm.Rolnaam); // nieuwe rol!
                
                // Gebruiker bestaat en de nieuwe is ook beschikbaar?
                if (gb != null && rol != null)
                {
                    // Huidige rollen opvragen in systeem
                    var huidigeRoles = await _userManager.GetRolesAsync(gb);
      
                    if (huidigeRoles.Any()){
                            var huidigeRole = huidigeRoles.First(); // Slechts een rol
                            var removeResult = await _userManager.RemoveFromRoleAsync(gb, huidigeRole);
                            if (!removeResult.Succeeded)
                            {
                                if (removeResult.Errors.Count() > 0)
                                {
                                    foreach (var error in removeResult.Errors)
                                        ModelState.AddModelError("message", error.Description);
                                }
                                return BadRequest(ModelState);
                            }
                        }

                    IdentityResult res = await _userManager.AddToRoleAsync(gb, rol.Name);

                        if (res.Succeeded)
                            return Ok();
                        else
                        {
                            foreach (IdentityError error in res.Errors)
                                ModelState.AddModelError("", error.Description);

                            return BadRequest(ModelState);
                        }
                } else
                    ModelState.AddModelError("", "Gebruiker bestaat niet.");

            ModelState.AddModelError("message", "Onbekende fout");
            return Unauthorized(ModelState);
        }

        /* CUD-operaties */
        // Een gebruiker wijzigen

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] CustomUser user)
        {
            if (user == null || id != user.Id) return BadRequest();

            CustomUser? gebruiker = await _userManager.FindByIdAsync(id);

            if (gebruiker == null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
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
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return BadRequest();
        }

        [Authorize(Roles ="SuperAdmin, Admin")]
        [HttpPost]
        public async Task<IActionResult> Nieuw([FromBody] CustomUser user, string paswoord)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IdentityResult result = await _userManager.CreateAsync(user, paswoord);
                    if (result.Succeeded)
                        return (IActionResult)result;
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
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return BadRequest();
        }

        // Enkel superadmin
        [Authorize(Roles = "Superadmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            CustomUser? gebruiker = await _userManager.FindByIdAsync(id);
            if (gebruiker != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(gebruiker);
                if (result.Succeeded)
                    return (IActionResult)result;
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
            return NotFound();
        }


        /* ***************/
        // Lijst gebruikers
        [Route("Lijst")]
        [HttpGet]
        public async Task<ActionResult> GetLijst()
        {
            return Ok(await _userManager.Users.ToListAsync());
        }

        // Eén gebruiker op basis van id
        [HttpGet("{id}")]
        public async Task<ActionResult> Details(string id)
        {
            CustomUser? gebruiker = await _userManager.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

            if (gebruiker != null)
                return Ok(gebruiker);
            else return Ok(null);
        }

    }
}
