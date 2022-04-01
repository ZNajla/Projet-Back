using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Models;
using Application.Models.DTO;
using Application.Configuration;
using Application.Models.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace authUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly JWTConfig _jWTConfig;

        public UserController(ILogger<UserController> logger, UserManager<User> userManager, SignInManager<User> signInManager, IOptions<JWTConfig> jwtConfig, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _jWTConfig = jwtConfig.Value;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddNewUser")]
        public async Task<object> AddNewUser([FromBody] AddUpdateUserModel model)
        {
            try
            {
                //if (!await _roleManager.RoleExistsAsync(model.Role))
                //{
                  //  return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Role does not exist", null));
                //}
                var user = new User() { FullName = model.FullName, Email = model.Email, UserName = model.UserName , Adresse = model.Adresse, PhoneNumber = model.PhoneNumber};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var tempUser = await _userManager.FindByEmailAsync(model.Email);
                    await _userManager.AddToRoleAsync(tempUser, model.Role);
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "User Has been Added", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "", result.Errors.Select(x => x.Description).ToArray()));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<object> GetAllUsers()
        {
            try
            {
                List<UserDTO> allUserDTO = new List<UserDTO>();
                var users = _userManager.Users.ToList();
                foreach (var user in users)
                {
                    var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                    if (role != "Admin")
                    {
                        allUserDTO.Add(new UserDTO(user.Id, user.FullName, user.UserName, user.Email, user.PhoneNumber , user.Adresse, role));
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
        [HttpGet("GetUserById/{id}")]
        public async Task<object> GetUserById(string id)
        {
            try
            {
                var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
                if(user != null)
                {
                    var roles = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                    var newUser = new UserDTO(user.Id, user.FullName, user.UserName, user.Email, user.PhoneNumber, user.Adresse, roles);
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", newUser));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [Authorize]
        [HttpPut("UpdateUser/{id}")]
        public async Task<object> UpdateUser(string id, [FromBody] AddUpdateUserModel model)
        {
            try
            {
                var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    user.FullName = model.FullName;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.Adresse = model.Adresse;
                    user.PhoneNumber = model.PhoneNumber;
                    await _userManager.CreateAsync(user, model.Password);
                    await _userManager.UpdateAsync(user);
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "User has been updated", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser/{id}")]
        public async Task<object> DeleteUser(string id)
        {

            try
            {
                var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                    if (role != "Admin")
                    {
                        await _userManager.DeleteAsync(user);
                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "User has been Deleted", null));
                    }
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Can't Delete Admin", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [HttpPost("Login")]
        public async Task<object> Login([FromBody] LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userEmail = await _userManager.FindByEmailAsync(model.Email);
                    var result = await _signInManager.PasswordSignInAsync(userEmail.UserName, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        var appUser = await _userManager.FindByNameAsync(userEmail.UserName);
                        var role = (await _userManager.GetRolesAsync(appUser)).FirstOrDefault();
                        var user = new UserDTO(appUser.Id, appUser.FullName, appUser.UserName, appUser.Email, appUser.PhoneNumber, appUser.Adresse, role);
                        user.Token = GenerateToken(appUser, role);
                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", user));
                    }
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Invalid User Name or Passwod", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        private string GenerateToken(User user, string role)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jWTConfig.Key);
            var tokenDescreptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.NameId,user.Id),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new System.Security.Claims.Claim(ClaimTypes.Role,role),
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jWTConfig.Audience,
                Issuer = _jWTConfig.Issuer
            };
            var token = jwtTokenHandler.CreateToken(tokenDescreptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
