using Application.Models.Entitys;
using Application.Models.Enums;
using Application.Models.Responce;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace authUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private readonly UserManager<User> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public ClaimsController(UserManager<User> userManager , ILogger<UserController> logger, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        [HttpPost("AddNewClaimToUser")]
        public async Task<object> AddNewClaimToUser(string id, string type, string value)
        {
            try
            {
                var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    var claim = new Claim(type, value);
                    var result = await _userManager.AddClaimAsync(user, claim);
                    if (result.Succeeded)
                    {
                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Claim Has been Added", null));
                    }
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "", result.Errors.Select(x => x.Description).ToArray()));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "User does not exist", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
            
        }

        [HttpPost("AddNewClaimToRole")]
        public async Task<object> AddNewClaimToRole(string roleName, string type, string value)
        {
            try
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist) // checks on the role exist status
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Role Does not exist", null));
                }
                var role = await _roleManager.FindByNameAsync(roleName);
                var claim = new Claim(type, value);
                var result = await _roleManager.AddClaimAsync(role, claim);
                if (result.Succeeded)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Claim Has been Added to Role", null));
                }
                   return await Task.FromResult(new ResponseModel(ResponseCode.Error, "", result.Errors.Select(x => x.Description).ToArray()));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }

        [HttpDelete("DeleteClaimToUser")]
        public async Task<object> DeleteClaimToUser(string id, string type, string value)
        {
            try
            {
                var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    var claim = new Claim(type, value);
                    var result = await _userManager.RemoveClaimAsync(user, claim);
                    if (result.Succeeded)
                    {
                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Claim Has been Removed", null));
                    }
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "", result.Errors.Select(x => x.Description).ToArray()));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "User does not exist", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [HttpDelete("DeleteClaimToRole")]
        public async Task<object> DeleteClaimToRole(string name, string type, string value)
        {
            try
            {
                var roleExist = await _roleManager.RoleExistsAsync(name);
                if (!roleExist) // checks on the role exist status
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Role Does not exist", null));
                }
                var role = await _roleManager.FindByNameAsync(name);
                var claim = new Claim(type, value);
                var result = await _roleManager.RemoveClaimAsync(role, claim);
                if (result.Succeeded)
                {
                   return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Claim Has been Removed", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "", result.Errors.Select(x => x.Description).ToArray()));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [HttpGet("GetAllClaimsUser")]
        public async Task<object> GetAllClaimsUser(string id)
        {
            try
            {
                IList<Claim> allClaims ;
                var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
                if(user != null)
                {
                    allClaims = await _userManager.GetClaimsAsync(user);
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", allClaims));

                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "User does not exist", null));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [HttpGet("GetAllClaimsRole")]
        public async Task<object> GetAllClaimsRole(string nameRole)
        {
            try
            {
                IList<Claim> allClaims;
                var roleExist = await _roleManager.RoleExistsAsync(nameRole);
                if (!roleExist) // checks on the role exist status
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Role Does not exist", null));
                }
                var role = await _roleManager.FindByNameAsync(nameRole);
                allClaims = await _roleManager.GetClaimsAsync(role);
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", allClaims));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
    }
}
