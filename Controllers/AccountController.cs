using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using No_Core_Auth.Helpers;
using No_Core_Auth.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace No_Core_Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _sigManager;
        private readonly AppSettings _appSettings;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _sigManager = signInManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("action")]
        [Route("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterViewModel formdata)
        {
            List<string> errorList = new List<string>();
            var user = new IdentityUser
            {
                Email = formdata.Email,
                UserName = formdata.UserName,
                SecurityStamp = Guid.NewGuid().ToString()

            };
            var result = await _userManager.CreateAsync(user, formdata.Password);
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
                //Send confirmation email

                return Ok(new { username = user.UserName, email = user.Email, status = 1, message = " Registration Successfull" });
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    errorList.Add(error.Description);   
                }
            }
            return BadRequest(new JsonResult(errorList));
        }

        //Login
        [HttpPost("action")]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel formdata)
        {
            //get the user from database
            var user = await _userManager.FindByNameAsync(formdata.Username);
            var roles = await _userManager.GetRolesAsync(user);
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret));

            double tokenExpiryTime = Convert.ToDouble(_appSettings.ExpireTime);

            if(user != null && await _userManager.CheckPasswordAsync(user, formdata.Password))
            {
                //confirmation of email
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(JwtRegisteredClaimNames.Sub, formdata.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                        new Claim("LoggedOn", DateTime.Now.ToString())
                    }),
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _appSettings.Site,
                    Audience= _appSettings.Audience,
                    Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTime),

                };
                //Generate Token
                var test = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new
                {
                    token = tokenHandler.WriteToken(test),
                    expiration = test.ValidTo,
                    username = user.UserName,
                    userRole = roles.FirstOrDefault()
                });
            }

            //return Error
            ModelState.AddModelError("", "UserName/Password was not found");
            return Unauthorized(new { LoginError = " Please check the login credentials - Invalid UserName/Password was entered" });
        }
    }
}
