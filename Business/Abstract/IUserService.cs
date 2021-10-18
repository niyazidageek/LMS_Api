using System;
using System.Threading.Tasks;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<ResponseDTO> RegisterAsync(RegisterDTO request);

        Task<LoginResponseDTO> LoginAsync(LoginDTO request);

        Task<ResponseDTO> AddRoleAsync(AddRoleDTO request);

        //Task<bool> ConfirmEmailAsync(string userId, string token);

        //Task<bool> ForgetPasswordAsync(string email);

        //Task<bool> ResetPasswordAsync(TokenRequestDTO model);

        //Task<AuthenticationModel> RefreshTokenAsync(TokenRequestDTO model);
    }
}
