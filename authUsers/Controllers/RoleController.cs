using Application.Configuration;
using Application.Models.DTO;
using Application.Models;
using Application.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace authUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {

        private readonly UserManager<User> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly JWTConfig _jWTConfig;

        public RoleController( UserManager<User> userManager, IOptions<JWTConfig> jwtConfig, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jWTConfig = jwtConfig.Value;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddRole")]
        public async Task<object> AddRole([FromBody] AddRoleModel model)
        {
            try
            {
                if (model == null || model.Role == "")
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Parameters are missing", null));
                }
                if (await _roleManager.RoleExistsAsync(model.Role))
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Role already exist", null));
                }
                var role = new IdentityRole();
                role.Name = model.Role;
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Role added successfully", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllRoles")]
        public async Task<object> GetAllRoles()
        {
            try
            {
                var roles = _roleManager.Roles.Select(x => x.Name).ToList();
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", roles));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteRole/{name}")]
        public async Task<object> DeleteRole(string name)
        {
            try
            {
                var role = _roleManager.Roles.FirstOrDefault(u => u.Name == name);
                if (role != null)
                {
                    await _roleManager.DeleteAsync(role);
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "role has been Deleted", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [Authorize]
        [HttpGet("GetUsersByRole/{name}")]
        public async Task<object> GetUsersByRole(string name)
        {
            try
            {
                List<UserDTO> allUserDTO = new List<UserDTO>();
                var users = _userManager.Users.ToList();
                foreach (var user in users)
                {
                    var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                    if (role == name)
                    {
                        allUserDTO.Add(new UserDTO(user.Id, user.FullName, user.UserName, user.Email, user.PhoneNumber, user.Adresse, role));
                    }
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", allUserDTO));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [Authorize]
        [HttpDelete("RemoveUserFromRole/{id}/{roleName}")]
        public async Task<object> RemoveUserFromRole(string id,  string roleName)
        {
            try
            {
                // Check if the user exist
                var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
                if (user == null) // User does not exist
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "User Does not exist", null));
                }
                // Check if the role exist
                var roleExist = await _roleManager.RoleExistsAsync(roleName);

                if (!roleExist) // checks on the role exist status
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Role Does not exist", null));
                }
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "User has been removed from role", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [Authorize]
        [HttpPost("AddUserToRole/{id}/{roleName}")]
        public async Task<object> AddUserToRole(string id, string roleName)
        {
            try
            {
                // Check if the user exist
                var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
                if (user == null) // User does not exist
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "User Does not exist", null));
                }
                // Check if the role exist
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist) // checks on the role exist status
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Role Does not exist", null));
                }
                var result = await _userManager.AddToRoleAsync(user, roleName);
                // Check if the user is assigned to the role successfully
                if (result.Succeeded)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Success, user has been added to the role", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
    }

}
