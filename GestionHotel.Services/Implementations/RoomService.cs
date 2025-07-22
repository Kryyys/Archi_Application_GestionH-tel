using GestionHotel.Core.Models;
using GestionHotel.Data.Repositories;
using GestionHotel.Services.DTOs;
using GestionHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionHotel.Services.Implementations
{
    public class RoomService : IRoomService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICleaningRepository _cleaningRepository;

        public RoomService(IReservationRepository reservationRepository, ICleaningRepository cleaningRepository)
        {
            _reservationRepository = reservationRepository;
            _cleaningRepository = cleaningRepository;
        }

        public async Task<AvailableRoomsResponseDto> GetAvailableRoomsAsync(AvailableRoomsRequestDto request)
        {
            var response = new AvailableRoomsResponseDto
            {
                DateDebut = request.DateDebut,
                DateFin = request.DateFin,
                NombreNuits = (request.DateFin - request.DateDebut).Days
            };

            return response;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            throw new NotImplementedException("À implémenter avec le service Room existant");
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            throw new NotImplementedException("À implémenter avec le service Room existant");
        }

        public async Task<Room> UpdateRoomStatusAsync(int roomId, string newStatus)
        {
            throw new NotImplementedException("À implémenter avec le service Room existant");
        }
    }
}