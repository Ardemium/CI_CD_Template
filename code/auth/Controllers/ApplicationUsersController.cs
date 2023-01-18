using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;

        public ApplicationUsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        // POST: api/ApplicationUsers/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ApplicationUserDTO applicationUserDTO)
        {
            var user = new ApplicationUser { UserName = applicationUserDTO.UserName, Email = applicationUserDTO.Email };

            var result = await _userManager.CreateAsync(user, applicationUserDTO.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok();
        }

        // POST: api/ApplicationUsers/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ApplicationUserDTO applicationUserDTO)
        {
            var user = await _userManager.FindByEmailAsync(applicationUserDTO.Email);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, applicationUserDTO.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("JWT:SigningKey")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config.GetValue<string>("JWT:Issuer"),
                _config.GetValue<string>("JWT:Issuer"),
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        // PUT: api/ApplicationUsers/modify
        [HttpPut("modify")]
        public async Task<IActionResult> Modify([FromBody] ApplicationUserDTO applicationUserDTO)
        {
            var user = await _userManager.FindByEmailAsync(applicationUserDTO.Email);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ChangePasswordAsync(user, applicationUserDTO.Password, applicationUserDTO.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        // DELETE: api/ApplicationUsers/delete
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] ApplicationUserDTO applicationUserDTO)
        {
            var user = await _userManager.FindByEmailAsync(applicationUserDTO.Email);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok();
        }
    }
}