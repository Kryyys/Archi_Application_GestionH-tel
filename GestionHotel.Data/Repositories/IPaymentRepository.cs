using GestionHotel.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionHotel.Data.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetPaymentsByReservationIdAsync(int reservationId);
        Task<Payment> GetPaymentByTransactionIdAsync(string transactionId);
        Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status);
    }
}