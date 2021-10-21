using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Concrete;
using DataAccess.Identity;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Utils;

namespace Business.Concrete
{
    public class UserService:IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;

        public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            IJwtService jwtService,AppDbContext context
            ,IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        public async Task<ResponseDTO> RegisterAsync(RegisterDTO request)
        {
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameEmail is not null) return new ResponseDTO
            {
                Status = StatusTypes.EmailError.ToString(),
                Message = "This email already exists"
            };

            var userWithSameUsername = await _userManager.FindByNameAsync(request.Username);

            if (userWithSameUsername is not null) return new ResponseDTO
            {
                Status = StatusTypes.UsernameError.ToString(),
                Message = "This username already exists"
            };

            var user = new AppUser
            {
                UserName = request.Username,
                Email = request.Email,
                Name = request.Name,
                Surname = request.Surname
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Student.ToString());

                await SendConfirmationEmailAsync(user);

                return new ResponseDTO
                {
                    Status = StatusTypes.Success.ToString(),
                    Message = $"Student with the username {user.UserName} has succesfully registered"
                };
            }
            else
            {
                return new ResponseDTO
                {
                    Status = StatusTypes.RegistrationError.ToString(),
                    Message = "Unexpected error occured"
                };
            }
        }

        public async Task<ResponseDTO> SendConfirmationEmailAsync(AppUser user)
        {
            try
            {
                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = Encoding.UTF8.GetBytes(confirmationToken);
                var validEmailToken = WebEncoders.Base64UrlEncode(encodedToken);
                string url = $"http://localhost:3000/ConfirmEmail/{user.Id}/{validEmailToken}";
                _emailService.SendMailToOneUser(user.Email, "Confirm your email", "", url);
                return new ResponseDTO()
                {
                    Status = nameof(StatusTypes.Success),
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = nameof(StatusTypes.ConfirmationError),
                    Message = ex.Message 
                };
            }
        }

        public async Task<ResponseDTO> SendConfirmationEmailAsync(SendConfirmEmailDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null) return new ResponseDTO
            {
                Status = nameof(StatusTypes.UserError),
                Message = "There is no such user!"
            };

            try
            {
                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = Encoding.UTF8.GetBytes(confirmationToken);
                var validEmailToken = WebEncoders.Base64UrlEncode(encodedToken);
                string url = $"http://localhost:3000/ConfirmEmail/{user.Id}/{validEmailToken}";
                _emailService.SendMailToOneUser(user.Email, "Confirm your email", "", url);
                return new ResponseDTO()
                {
                    Status = nameof(StatusTypes.Success),
                    Message = "Confirmation email has been sent!"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = nameof(StatusTypes.ConfirmationError),
                    Message = ex.Message
                };
            }
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null) return new LoginResponseDTO
            {
                Status = nameof(StatusTypes.LoginError),
                Message = "Invalid password or email"
            };

            var succeeded = await _userManager.CheckPasswordAsync(user, request.Password);

            if (succeeded)
            {
                JWT token = await _jwtService.CreateJwtTokenAsync(user);

                return new LoginResponseDTO
                {
                    Status = nameof(StatusTypes.Success),
                    Message = $"User with the username {user.UserName} has successfully logged in!",
                    Token = token.Token,
                    ExpiryDate = token.ExpiryDate
                };
            }
            else
            {
                return new LoginResponseDTO
                {
                    Status = nameof(StatusTypes.LoginError),
                    Message = "Invalid password or email"
                };
            }
        }

        public async Task<ResponseDTO> AddRoleAsync(AddRoleDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null) return new ResponseDTO
            {
                Status = nameof(StatusTypes.UserError),
                Message = "There is no such user"
            };

            var roleExists = Enum.GetNames(typeof(Roles)).Any(x => x.ToLower() == request.Role.ToLower());
            if (roleExists)
            {
                var validRole = Enum.GetValues(typeof(Roles)).Cast<Roles>().Where(x => x.ToString().ToLower() == request.Role.ToLower()).FirstOrDefault();
                await _userManager.AddToRoleAsync(user, validRole.ToString());
                return new ResponseDTO
                {
                    Status = nameof(StatusTypes.Success),
                    Message = $"Role '{validRole}' added to {user.UserName}"
                };
            }
            return new ResponseDTO
            {
                Status = nameof(StatusTypes.RoleError),
                Message = "There is no such role"
            };
        }

        public async Task<ResponseDTO> ConfirmEmailAsync(ConfirmEmailDTO request) 
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null) return new ResponseDTO
            {
                Status = nameof(StatusTypes.UsernameError),
                Message = "There is no such user"
            };

            if (user.EmailConfirmed is true) return new ResponseDTO
            {
                Status = nameof(StatusTypes.ConfirmationError),
                Message = "Your email has already been confirmed!"
            };

            var decodedToken = WebEncoders.Base64UrlDecode(request.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ConfirmEmailAsync(user, normalToken);

            if (result.Succeeded)
            {
                return new ResponseDTO
                {
                    Status = StatusTypes.Success.ToString(),
                    Message = "Email confirmed!"
                };
            }
            return new ResponseDTO
            {
                Status=StatusTypes.ConfirmationError.ToString(),
                Message = "Confirmation has failed!"
            };
        }

        public async Task<ResponseDTO> ForgetPasswordAsync(ForgetPasswordDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null) return new ResponseDTO
            {
                Status = nameof(StatusTypes.UserError),
                Message = "There is no such user!"
            };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = $"http://localhost:3000/resetpassword";

            _emailService.SendMailToOneUser(user.Email, "Reset password",token,url);

            return new ResponseDTO
            {
                Status = nameof(StatusTypes.Success),
                Message = "Mail for resetting the password has been sent!"
            };
        }

        public async Task<ResponseDTO> ResetPasswordAsync(ResetPasswordDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null) return new ResponseDTO
            {
                Status = nameof(StatusTypes.UserError),
                Message = "There is no such user!"
            }; 

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (result.Succeeded) return new ResponseDTO
            {
                Status = nameof(StatusTypes.Success),
                Message = "Password has been changed successfully!"
            };

            return new ResponseDTO
            {
                Status = nameof(StatusTypes.ResetPasswordError),
                Message = "Failed to change the password. Try again!"
            };
        }

        
    }
}
