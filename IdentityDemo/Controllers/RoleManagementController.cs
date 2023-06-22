using IdentityDemo.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleManagementController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagementController
            (
            AppDbContext context, 
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _context=context;
            _userManager = userManager;
            _roleManager = roleManager;
        }



        [HttpGet]
        [Route("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            return Ok(_roleManager.Roles.ToList());
        }

        [HttpPost]
        [Route("CreateRoleAsync")]
        public async Task<IActionResult> CreateRoleAsync(string name)
        {
            if (!await _roleManager.RoleExistsAsync(name)) 
            {
                var IsAdded = await _roleManager.CreateAsync(new IdentityRole(name));
                if (IsAdded.Succeeded)
                    return Ok(new { result=$"Role {name} has been added succeesfuly"});
                else
                    return BadRequest(new { error = $"Role {name} has not been added" });
            }
            return BadRequest(new {error=$"Role {name} already exist"});



            return Ok();
        }

        [HttpPost]
        [Route("AddUserToRoleAsync")]
        public async Task<IActionResult> AddUserToRoleAsync(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return BadRequest(new { error ="This user is not exsit"});

            var IsRoleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!IsRoleExist)
                return BadRequest(new { error = "This Role is not exsit" });

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if(result.Succeeded)
                return Ok(new{result = "Adding success"});
            else
                return BadRequest(new { error = "Adding faild" });
        }
        
        [HttpPost]
        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return BadRequest(new { error ="This user is not exsit"});
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }
        
        [HttpPost]
        [Route("RemoveUserFromRoleAsync")]
        public async Task<IActionResult> RemoveUserFromRoleAsync(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return BadRequest(new { error ="This user is not exsit"});

            var IsRoleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!IsRoleExist)
                return BadRequest(new { error = "This Role is not exsit" });

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if(result.Succeeded)
                return Ok(new{result = "Removing Done"});
            else
                return BadRequest(new { error = "Removing faild" });
        }
        
        
        
    }

}
 