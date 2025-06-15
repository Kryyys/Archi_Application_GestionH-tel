using GestionHotel.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionHotel.Services
{
    public interface IRoomService
    {
        /// <summary>
        /// Récupère toutes les chambres, peu importe leur disponibilité.
        /// </summary>
        Task<List<Room>> GetAllRoomsAsync();

        /// <summary>
        /// Récupère les chambres disponibles entre deux dates données.
        /// </summary>
        /// <param name="startDate">Date de début souhaitée.</param>
        /// <param name="endDate">Date de fin souhaitée.</param>
        Task<List<Room>> GetAvailableRooms(DateTime startDate, DateTime endDate);
    }
}
