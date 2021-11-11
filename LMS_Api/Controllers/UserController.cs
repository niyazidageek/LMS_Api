using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using System.Web;
using DataAccess.Identity;
using Entities.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Utils;
using Microsoft.AspNetCore.Identity;
using DataAccess.Concrete;
using Entities.Models;
using LMS_Api.Utils;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly JWTConfig _jwtConfig;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IMapper _mapper;

        public UserController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTConfig> jwtConfig,
            TokenValidationParameters tokenValidationParameters,
            IMapper mapper)
        {
            _tokenValidationParameters = tokenValidationParameters;
            _jwtConfig = jwtConfig.Value;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllStudents()
        {
            var students = await _userManager.GetUsersInRoleAsync(nameof(Roles.Student));

            var studentsDto = _mapper.Map<List<AppUserDTO>>(students);

            return Ok(studentsDto);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllTeachers()
        {
            var teachers = await _userManager.GetUsersInRoleAsync(nameof(Roles.Teacher));

            var teachersDto = _mapper.Map<List<AppUserDTO>>(teachers);

            return Ok(teachersDto);
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var userWithSameEmail = await _userManager.FindByEmailAsync(registerDto.Email);

            if (userWithSameEmail is not null)
                return Conflict(new ResponseDTO
                {
                    Status = StatusTypes.EmailError.ToString(),
                    Message = "This email already exists"
                });

            var userWithSameUsername = await _userManager.FindByNameAsync(registerDto.Username);

            if (userWithSameUsername is not null)
                return Conflict(new ResponseDTO
                {
                    Status = StatusTypes.UsernameError.ToString(),
                    Message = "This username already exists"
                });

            var user = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                Name = registerDto.Name,
                Surname = registerDto.Surname
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, nameof(Roles.Student));

                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var suceeded = EmailHelper.SendConfirmationEmail(confirmationToken, user.Email, user.Id);

                if(suceeded)
                    return Ok(new ResponseDTO
                    {
                        Status = StatusTypes.Success.ToString(),
                        Message = $"Student with the username {user.UserName} has succesfully registered"
                    });

                return BadRequest(new ResponseDTO
                {
                    Status = StatusTypes.ConfirmationError.ToString(),
                    Message = "Confirmation message can't be sent!"
                });
            }
            else
            {
                return Unauthorized(new ResponseDTO
                {
                    Status = StatusTypes.RegistrationError.ToString(),
                    Message = "Unexpected error occured!"
                });
            }
        }

        [Authorize(Roles=nameof(Roles.SuperAdmin))]
        [HttpPost]
        public async Task<ActionResult> RegisterAdmin([FromBody] RegisterDTO registerDto)
        {
            var userWithSameEmail = await _userManager.FindByEmailAsync(registerDto.Email);

            if (userWithSameEmail is not null)
                return Conflict(new ResponseDTO
                {
                    Status = StatusTypes.EmailError.ToString(),
                    Message = "This email already exists"
                });

            var userWithSameUsername = await _userManager.FindByNameAsync(registerDto.Username);

            if (userWithSameUsername is not null)
                return Conflict(new ResponseDTO
                {
                    Status = StatusTypes.UsernameError.ToString(),
                    Message = "This username already exists"
                });

            var user = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                Name = registerDto.Name,
                Surname = registerDto.Surname
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                foreach (var role in registerDto.Roles)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }

                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var suceeded = EmailHelper.SendConfirmationEmail(confirmationToken, user.Email, user.Id);

                if (suceeded)
                    return Ok(new ResponseDTO
                    {
                        Status = StatusTypes.Success.ToString(),
                        Message = $"Student with the username {user.UserName} has succesfully registered"
                    });

                return BadRequest(new ResponseDTO
                {
                    Status = StatusTypes.ConfirmationError.ToString(),
                    Message = "Confirmation message can't be sent!"
                });
            }
            else
            {
                return Unauthorized(new ResponseDTO
                {
                    Status = StatusTypes.RegistrationError.ToString(),
                    Message = "Unexpected error occured!"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
                return NotFound(new LoginResponseDTO
                {
                    Status = nameof(StatusTypes.LoginError),
                    Message = "Invalid password or email!"
                });

            var succeeded = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (succeeded)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                var roles = await _userManager.GetRolesAsync(user);

                var roleClaims = new List<Claim>();

                for (int i = 0; i < roles.Count; i++)
                {
                    roleClaims.Add(new Claim("roles", roles[i]));
                }

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
                }
                .Union(userClaims)
                .Union(roleClaims);

                var expiryDate = DateTime.UtcNow.AddMinutes(_jwtConfig.DurationInMinutes);

                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));

                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwtConfig.Issuer,
                    audience: _jwtConfig.Audience,
                    claims: claims,
                    expires: expiryDate,
                    signingCredentials: signingCredentials);


                return Ok(new LoginResponseDTO
                {
                    Status = nameof(StatusTypes.Success),
                    Message = $"User with the username {user.UserName} has successfully logged in!",
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    ExpiryDate = expiryDate,
                    Roles = (List<string>)roles,
                    Name = user.Name,
                    Surname = user.Surname,
                    Username = user.UserName
                });
            }
            else
            {
                return Unauthorized(new LoginResponseDTO
                {
                    Status = nameof(StatusTypes.LoginError),
                    Message = "Invalid password or email!"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult> LoginAdmin([FromBody] LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
                return NotFound(new LoginResponseDTO
                {
                    Status = nameof(StatusTypes.LoginError),
                    Message = "Invalid password or email!"
                });

            var succeeded = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (succeeded)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                var roles = await _userManager.GetRolesAsync(user);

                var isAdmin = roles.Any(x => x.ToLower() == nameof(Roles.Admin).ToLower()
                || x.ToLower() == nameof(Roles.SuperAdmin).ToLower());

                if (isAdmin is false)
                    return Unauthorized(new LoginResponseDTO
                    {
                        Status = nameof(StatusTypes.LoginError),
                        Message = "Invalid password or email!"
                    });

                var roleClaims = new List<Claim>();

                for (int i = 0; i < roles.Count; i++)
                {
                    roleClaims.Add(new Claim("roles", roles[i]));
                }

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
                }
                .Union(userClaims)
                .Union(roleClaims);

                var expiryDate = DateTime.UtcNow.AddMinutes(_jwtConfig.DurationInMinutes);

                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));

                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwtConfig.Issuer,
                    audience: _jwtConfig.Audience,
                    claims: claims,
                    expires: expiryDate,
                    signingCredentials: signingCredentials);


                return Ok(new LoginResponseDTO
                {
                    Status = nameof(StatusTypes.Success),
                    Message = $"User with the username {user.UserName} has successfully logged in!",
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    ExpiryDate = expiryDate,
                    Roles = (List<string>)roles,
                    Name = user.Name,
                    Surname = user.Surname,
                    Username = user.UserName
                });
            }
            else
            {
                string a = "aaa";
                a.ToLower();
                return Unauthorized(new LoginResponseDTO
                {
                    Status = nameof(StatusTypes.LoginError),
                    Message = "Invalid password or email!"
                });
            }
        }

        //[Authorize]
        [HttpPost]
        public async Task<ActionResult> AddRole([FromBody] AddRoleDTO addRoleDto)
        {
            var user = await _userManager.FindByEmailAsync(addRoleDto.Email);

            if (user is null)
                return NotFound(new ResponseDTO
                {
                    Status = nameof(StatusTypes.UserError),
                    Message = "There is no such user"
                });

            var roleExists = Enum.GetNames(typeof(Roles)).Any(x => x.ToLower() == addRoleDto.Role.ToLower());

            if (roleExists)
            {
                var validRole = Enum.GetValues(typeof(Roles)).Cast<Roles>().Where(x => x.ToString().ToLower() == addRoleDto.Role.ToLower()).FirstOrDefault();
                await _userManager.AddToRoleAsync(user, validRole.ToString());
                return Ok(new ResponseDTO
                {
                    Status = nameof(StatusTypes.Success),
                    Message = $"Role '{validRole}' added to {user.UserName}"
                });
            }
            return NotFound(new ResponseDTO
            {
                Status = nameof(StatusTypes.RoleError),
                Message = "There is no such role"
            });
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmEmail([FromBody] ConfirmEmailDTO confirmEmailDto)
        {
            var user = await _userManager.FindByIdAsync(confirmEmailDto.UserId);

            if (user is null)
                return NotFound(new ResponseDTO
                {
                    Status = nameof(StatusTypes.UsernameError),
                    Message = "There is no such user"
                });

            if (user.EmailConfirmed is true)
                return Conflict(new ResponseDTO
                {
                    Status = nameof(StatusTypes.ConfirmationError),
                    Message = "Your email has already been confirmed!"
                });

            var decodedToken = WebEncoders.Base64UrlDecode(confirmEmailDto.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ConfirmEmailAsync(user, normalToken);

            if (result.Succeeded)
            {
                return Ok(new ResponseDTO
                {
                    Status = StatusTypes.Success.ToString(),
                    Message = "Email confirmed!"
                });
            }

            return BadRequest(new ResponseDTO
            {
                Status = StatusTypes.ConfirmationError.ToString(),
                Message = "Confirmation has failed!"
            });
        }

        [HttpPost]
        public async Task<ActionResult> ForgetPassword(ForgetPasswordDTO forgetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordDto.Email);

            if (user is null)
                return NotFound(new ResponseDTO
                {
                    Status = nameof(StatusTypes.UserError),
                    Message = "There is no such user!"
                });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = $"http://localhost:3000/resetpassword";

            var suceeded = EmailHelper.SendMailToOneUser(user.Email, "Reset password", token, url);

            if (suceeded)
                return Ok(new ResponseDTO
                {
                    Status = nameof(StatusTypes.Success),
                    Message = "Mail for resetting the password has been sent!"
                });

            return BadRequest(new ResponseDTO
            {
                Status = nameof(StatusTypes.EmailError),
                Message = "Email can't be sent!"
            });
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

            if (user is null)
                return NotFound(new ResponseDTO
                {
                    Status = nameof(StatusTypes.UserError),
                    Message = "There is no such user!"
                });

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);

            if (result.Succeeded)
                return Ok(new ResponseDTO
                {
                    Status = nameof(StatusTypes.Success),
                    Message = "Password has been changed successfully!"
                });

            return BadRequest(new ResponseDTO
            {
                Status = nameof(StatusTypes.ResetPasswordError),
                Message = "Failed to change the password. Try again!"
            });
        }

        [HttpPost]
        public async Task<ActionResult> SendConfirmationEmail([FromBody] SendConfirmEmailDTO sendConfirmEmailDto)
        {
            var user = await _userManager.FindByEmailAsync(sendConfirmEmailDto.Email);

            if (user is null)
                return NotFound(new ResponseDTO
                {
                    Status = nameof(StatusTypes.UserError),
                    Message = "There is no such user!"
                });

            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(confirmationToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedToken);
            string url = $"http://localhost:3000/ConfirmEmail/{user.Id}/{validEmailToken}";

            var succeeded = EmailHelper.SendMailToOneUser(user.Email, "Confirm your email", "", url);

            if (succeeded)
                return Ok(new ResponseDTO()
                {
                    Status = nameof(StatusTypes.Success),
                    Message = "Confirmation email has been sent!"
                });

            return BadRequest(new ResponseDTO
            {
                Status = nameof(StatusTypes.ConfirmationError),
                Message = "Email can't be sent!"
            });
        }
    }
}