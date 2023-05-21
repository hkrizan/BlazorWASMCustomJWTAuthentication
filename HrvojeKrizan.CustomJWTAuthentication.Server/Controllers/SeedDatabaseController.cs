using HrvojeKrizan.CustomJWTAuthentication.Data;
using HrvojeKrizan.CustomJWTAuthentication.Data.Models;
using HrvojeKrizan.CustomJWTAuthentication.Server.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HrvojeKrizan.CustomJWTAuthentication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedDatabaseController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public SeedDatabaseController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext dbContext
            ) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }
        [HttpGet("generate")]

        public async Task<ActionResult> Generate()
        {
            try
            {
                if (!_dbContext.Users.Any())
                {
                    var adminUser = new ApplicationUser
                    {
                        Id = Guid.NewGuid(),
                        Email = "admin@example.com",
                        UserName = "admin",
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var adminResult = await _userManager.CreateAsync(adminUser, "admin");

                    var regularUser = new ApplicationUser
                    {
                        Id = Guid.NewGuid(),
                        Email = "user@example.com",
                        UserName = "user",
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var userResult = await _userManager.CreateAsync(regularUser, "user");

                    ApplicationRole adminRole;
                    ApplicationRole userRole;

                    if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    {
                        adminRole = await _roleManager.FindByNameAsync(UserRoles.Admin);
                    }
                    else
                    {
                        adminRole = new ApplicationRole(UserRoles.Admin)
                        {
                            Id = Guid.NewGuid(),
                            ConcurrencyStamp = Guid.NewGuid().ToString()
                        };

                        await _roleManager.CreateAsync(adminRole);
                    }


                    if (await _roleManager.RoleExistsAsync(UserRoles.User))
                    {
                        userRole = await _roleManager.FindByNameAsync(UserRoles.User);
                    }
                    else
                    {
                        userRole = new ApplicationRole(UserRoles.User)
                        {
                            Id = Guid.NewGuid(),
                            ConcurrencyStamp = Guid.NewGuid().ToString()
                        };
                        await _roleManager.CreateAsync(userRole);
                    }
                        

                    await _userManager.AddToRoleAsync(adminUser, UserRoles.Admin);

                    await _userManager.AddToRoleAsync(regularUser, UserRoles.User);

                    await _roleManager.AddClaimAsync(adminRole, new System.Security.Claims.Claim("ApplicationClaim", RoleClaims.SuperAdmin));

                    await _roleManager.AddClaimAsync(userRole, new System.Security.Claims.Claim("ApplicationClaim", RoleClaims.SpecialPermissions));

                    return Ok("Created");
                }

                return Ok("Already created");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
