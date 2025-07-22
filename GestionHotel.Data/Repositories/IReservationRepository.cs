using GestionHotel.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionHotel.Data.Repositories
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> GetReservationsByClientIdAsync(int clientId);
        Task<IEnumerable<Reservation>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Reservation>> GetReservationsByStatusAsync(string status);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime startDate, DateTime endDate, int? excludeReservationId = null);
        Task<IEnumerable<Reservation>> GetConflictingReservationsAsync(int roomId, DateTime startDate, DateTime endDate);
    }
}