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

        Task<ResponseDTO> ConfirmEmailAsync(string userId, string token);

        Task<ResponseDTO> ForgetPasswordAsync(ForgetPasswordDTO request);

        Task<ResponseDTO> ResetPasswordAsync(ResetPasswordDTO request);

        //Task<AuthenticationModel> RefreshTokenAsync(TokenRequestDTO model);
    }
}
