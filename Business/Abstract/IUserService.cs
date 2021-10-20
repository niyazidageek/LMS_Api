using System;
using System.Threading.Tasks;
using Entities.DTOs;
using Entities.Models;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<ResponseDTO> RegisterAsync(RegisterDTO request);

        Task<LoginResponseDTO> LoginAsync(LoginDTO request);

        Task<ResponseDTO> AddRoleAsync(AddRoleDTO request);

        Task<ResponseDTO> ConfirmEmailAsync(ConfirmEmailDTO request);

        Task<ResponseDTO> ForgetPasswordAsync(ForgetPasswordDTO request);

        Task<ResponseDTO> ResetPasswordAsync(ResetPasswordDTO request);

        Task<ResponseDTO> SendConfirmationEmailAsync(AppUser user);

        Task<ResponseDTO> SendConfirmationEmailAsync(SendConfirmEmailDTO request);

        //Task<AuthenticationModel> RefreshTokenAsync(TokenRequestDTO model);
    }
}
