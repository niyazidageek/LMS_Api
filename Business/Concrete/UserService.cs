using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Concrete;
using DataAccess.Identity;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
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

        public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            IJwtService jwtService,AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _jwtService = jwtService;
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

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null) return new LoginResponseDTO
            {
                Status = StatusTypes.LoginError.ToString(),
                Message = "Invalid password or email"
            };

            var succeeded = await _userManager.CheckPasswordAsync(user, request.Password);

            if (succeeded)
            {
                JWT token = await _jwtService.CreateJwtTokenAsync(user);

                return new LoginResponseDTO
                {
                    Status = StatusTypes.Success.ToString(),
                    Message = $"User with the username {user.UserName} has successfully logged in",
                    Token = token.Token
                };
            }
            else
            {
                return new LoginResponseDTO
                {
                    Status = StatusTypes.LoginError.ToString(),
                    Message = "Invalid password or email"
                };
            }

        }

        public async Task<ResponseDTO> AddRoleAsync(AddRoleDTO request)
        {
            var validatedToken = _jwtService.GetPrincipalFromToken(request.Token);

            if (validatedToken == null) return new ResponseDTO
            {
                Status = StatusTypes.InvalidToken.ToString(),
                Message="Token is invalid"
            };
            

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null) return new ResponseDTO
            {
                Status = StatusTypes.UserError.ToString(),
                Message = "There is no such user"
            };

            var roleExists = Enum.GetNames(typeof(Roles)).Any(x => x.ToLower() == request.Role.ToLower());
            if (roleExists)
            {
                var validRole = Enum.GetValues(typeof(Roles)).Cast<Roles>().Where(x => x.ToString().ToLower() == request.Role.ToLower()).FirstOrDefault();
                await _userManager.AddToRoleAsync(user, validRole.ToString());
                return new ResponseDTO
                {
                    Status = StatusTypes.Success.ToString(),
                    Message = $"Role '{validRole}' added to {user.UserName}"
                };
            }
            return new ResponseDTO
            {
                Status = StatusTypes.RoleError.ToString(),
                Message = "There is no such role"
            };
        }
    }
}
