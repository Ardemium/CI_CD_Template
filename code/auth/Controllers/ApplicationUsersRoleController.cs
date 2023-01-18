using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ApplicationUsersRoleController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationUsersRoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // POST: api/ApplicationUsersRole/assignrole
    [HttpPost("assignrole")]
    public async Task<IActionResult> AssignRole([FromBody] ApplicationUserRoleDTO userRoleDTO)
    {
        var user = await _userManager.FindByEmailAsync(userRoleDTO.Email);
        if (user == null)
        {
            return NotFound();
        }

        var roleExists = await _roleManager.RoleExistsAsync(userRoleDTO.Role);
        if (!roleExists)
        {
            return BadRequest("The role does not exist");
        }

        var result = await _userManager.AddToRoleAsync(user, userRoleDTO.Role);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }
        // POST: api/ApplicationUsersRole/removerole
    [HttpPost("removerole")]
    public async Task<IActionResult> RemoveRole([FromBody] ApplicationUserRoleDTO userRoleDTO)
    {
        var user = await _userManager.FindByEmailAsync(userRoleDTO.Email);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.RemoveFromRoleAsync(user, userRoleDTO.Role);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }

    // GET: api/ApplicationUsersRole/userroles/email@email.com
    [HttpGet("userroles/{email}")]
    public async Task<IActionResult> GetUserRoles(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return NotFound();
        }

        var roles = await _userManager.GetRolesAsync(user);
        if (roles == null)
        {
            return NotFound();
        }

        return Ok(roles);
    }
}
