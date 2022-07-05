using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Models.Entitys;
using Application.Models.DTO;
using Application.Configuration;
using Application.Models.Enums;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Application.Models.Request;
using Application.Models.Responce;

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

        private readonly MailSettings _mailSettings;

        public UserController(ILogger<UserController> logger, UserManager<User> userManager, SignInManager<User> signInManager, IOptions<JWTConfig> jwtConfig, RoleManager<IdentityRole> roleManager , IOptions<MailSettings> mailSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _jWTConfig = jwtConfig.Value;
            _mailSettings = mailSettings.Value;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("AddNewUser")]
        public async Task<object> AddNewUser([FromBody] AddUpdateUserModel model)
        {
            try
            {
                var user = new User() { FullName = model.FullName, Email = model.Email, UserName = model.UserName , Adresse = model.Adresse, PhoneNumber = model.PhoneNumber , Gender = model.Gender , BirthDate = model.BirthDate , Facebook = model.Facebook , Google = model.Google , Linkedin = model.Linkedin , LastTimeLogedIn = model.LastTimeLogedIn};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if(model.Role != "")
                    {
                        var tempUser = await _userManager.FindByEmailAsync(model.Email);
                        await _userManager.AddToRoleAsync(tempUser, model.Role);
                    }
                    var mailRequest = new MailRequest(model.Email, "Welcome", "Email : " + model.Email + " Password : " + model.Password);
                    SendEmailAsync(mailRequest);
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "User Has been Added", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "", result.Errors.Select(x => x.Description).ToArray()));
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
                    if (userEmail != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(userEmail.UserName, model.Password, false, false);
                        if (result.Succeeded)
                        {
                            var appUser = await _userManager.FindByNameAsync(userEmail.UserName);
                            var role = (await _userManager.GetRolesAsync(appUser)).FirstOrDefault();
                            var user = new UserDTO(appUser.Id, appUser.FullName, appUser.UserName, appUser.Email, appUser.PhoneNumber, appUser.Adresse, appUser.Gender, appUser.BirthDate, appUser.Facebook, appUser.Google, appUser.Linkedin, appUser.LastTimeLogedIn, role);
                            user.Token = GenerateToken(appUser, role);
                            return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", user));
                        }
                        return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Check your password ", null));
                    }
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Email does not exist", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Invalid Email or Passwod", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        /* GetAllUsers retourne la liste de tous les utilisateurs sauf les Admins */
       // [Authorize(Roles = "Admin")]
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
                        allUserDTO.Add(new UserDTO(user.Id, user.FullName, user.UserName, user.Email, user.PhoneNumber, user.Adresse, user.Gender, user.BirthDate, user.Facebook, user.Google, user.Linkedin, user.LastTimeLogedIn, role));
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
                    var newUser = new UserDTO(user.Id, user.FullName, user.UserName, user.Email, user.PhoneNumber, user.Adresse, user.Gender, user.BirthDate, user.Facebook, user.Google, user.Linkedin, user.LastTimeLogedIn, roles);
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", newUser));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something went wrong please try again", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

       // [Authorize]
        [HttpPut("UpdateUser/{id}")]
        public async Task<object> UpdateUser(string id, [FromBody] AddUpdateUserModel model)
        {
            try
            {
                var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                    if(userRole != null)
                    {
                        await _userManager.RemoveFromRoleAsync(user, userRole);
                    }
                    user.FullName = model.FullName;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.Adresse = model.Adresse;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Gender = model.Gender;
                    user.BirthDate = model.BirthDate;
                    user.Facebook = model.Facebook;
                    user.Google = model.Google;
                    user.Linkedin = model.Linkedin;
                    await _userManager.AddToRoleAsync(user, model.Role);
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

        [HttpPut("Logout/{id}")]
        public async Task<object> Logout(string id)
        {
            try
            {
                var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    user.LastTimeLogedIn = DateTime.Now;
                    await _userManager.UpdateAsync(user);
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "User has Loged out successfuly", null));
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
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Can't Delete Admin", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "user does not exist", null));
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

        [HttpPost("[action]")]
        public async Task<object> SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            if(email != null)
            {
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;
                var builder = new BodyBuilder();
                builder.HtmlBody = mailRequest.Body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Email has been send", null));
            }
            return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Something wrong", null));
        }
    }
}