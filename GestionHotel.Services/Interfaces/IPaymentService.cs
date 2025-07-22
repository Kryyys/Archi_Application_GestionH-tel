using GestionHotel.Services.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionHotel.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResultDto> ProcessPaymentAsync(CreatePaymentDto paymentDto);
        Task<PaymentDto> GetPaymentByIdAsync(int id);
        Task<IEnumerable<PaymentDto>> GetPaymentsByReservationIdAsync(int reservationId);
        Task<PaymentResultDto> RefundPaymentAsync(int paymentId, decimal amount, string reason);
        Task<bool> ValidatePaymentAsync(int paymentId);
    }
}