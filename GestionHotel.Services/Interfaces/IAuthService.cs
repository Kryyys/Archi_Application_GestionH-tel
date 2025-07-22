using GestionHotel.Services.DTOs;
using System.Threading.Tasks;

namespace GestionHotel.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResultDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(CreateUserDto registerDto);
        Task<bool> ValidateTokenAsync(string token);
        Task<UserDto> GetCurrentUserAsync(string token);
        string GenerateJwtToken(UserDto user);
    }
}