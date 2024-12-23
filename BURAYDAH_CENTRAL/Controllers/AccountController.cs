using BURAYDAH_CENTRAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BURAYDAH_CENTRAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<IdentityUser> userManager,RoleManager<IdentityRole>roleManager,IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Register(RegisterDto registerDTO)
        {
            if (ModelState.IsValid)
            {

                IdentityUser appuser = new IdentityUser()
                {
                    UserName = registerDTO.Email,
                    Email = registerDTO.Email,
                   
                };


                IdentityResult result = await userManager.CreateAsync(appuser, registerDTO.Password);

                if (result.Succeeded)
                {

                    var roleExists = await roleManager.RoleExistsAsync("Admin");
                    if (!roleExists)
                    {
                        IdentityRole role = new IdentityRole("Admin");
                        await roleManager.CreateAsync(role);
                    }


                    await userManager.AddToRoleAsync(appuser, "Admin");

                    return Ok("Account Created and Admin role assigned :)");
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(ModelState);


        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] RegisterDto RegisterDto)
        {
            if (ModelState.IsValid)
            {
                IdentityUser userFromDb = await userManager.FindByNameAsync(RegisterDto.Email);
                if (userFromDb != null)
                {
                    bool found = await userManager.CheckPasswordAsync(userFromDb, RegisterDto.Password);
                    if (found)
                    {
                        //create list of Claim
                        List<Claim> myclaims = new List<Claim>();
                        myclaims.Add(new Claim(ClaimTypes.Name, userFromDb.UserName));
                        myclaims.Add(new Claim(ClaimTypes.NameIdentifier, userFromDb.Id));
                        myclaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));


                        var roles = await userManager.GetRolesAsync(userFromDb);
                        foreach (var role in roles)
                        {
                            myclaims.Add(new Claim(ClaimTypes.Role, role));
                        }
                        var rolee = roles.FirstOrDefault();
                        var SignKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["JwtSetting:SecritKey"]));

                        SigningCredentials signingCredentials =
                            new SigningCredentials(SignKey, SecurityAlgorithms.HmacSha256);


                        JwtSecurityToken mytoken = new JwtSecurityToken
                            (
                                issuer: configuration["JwtSetting:issuer"],
                                audience: configuration["JwtSetting:audience"],
                                expires: DateTime.Now.AddHours(5),
                                claims: myclaims,
                                signingCredentials: signingCredentials
                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expired = mytoken.ValidTo,
                            role = rolee
                        });
                    }
                }
                return Unauthorized("UserName or Password Invalid");
            }
            return BadRequest(ModelState);
        }
    }
}
