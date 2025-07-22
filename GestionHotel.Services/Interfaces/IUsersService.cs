using GestionHotel.Services.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionHotel.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto createDto);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role);
        Task<UserDto> UpdateUserAsync(int id, CreateUserDto updateDto);
        Task<bool> DeactivateUserAsync(int id);
    }
}