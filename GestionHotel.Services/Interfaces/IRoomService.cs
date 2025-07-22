using GestionHotel.Core.Models;
using GestionHotel.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionHotel.Services.Interfaces
{
    public interface IRoomService
    {
        Task<AvailableRoomsResponseDto> GetAvailableRoomsAsync(AvailableRoomsRequestDto request);
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<Room> GetRoomByIdAsync(int id);
        Task<Room> UpdateRoomStatusAsync(int roomId, string newStatus);
    }
}