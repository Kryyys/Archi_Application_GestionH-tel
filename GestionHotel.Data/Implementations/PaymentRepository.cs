using GestionHotel.Core.Models;
using GestionHotel.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestionHotel.Data.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly SupabaseClient _supabaseClient;
        private const string TableName = "payments";

        public PaymentRepository(SupabaseClient supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            var response = await _supabaseClient.GetDataAsync(TableName);
            return JsonSerializer.Deserialize<IEnumerable<Payment>>(response) ?? new List<Payment>();
        }

        public async Task<Payment> GetByIdAsync(int id)
        {
            var response = await _supabaseClient.GetFilteredDataAsync(TableName, $"id=eq.{id}");
            var payments = JsonSerializer.Deserialize<IEnumerable<Payment>>(response);
            return payments?.FirstOrDefault();
        }

        public async Task<Payment> CreateAsync(Payment entity)
        {
            var response = await _supabaseClient.PostDataAsync(TableName, entity);
            var payments = JsonSerializer.Deserialize<IEnumerable<Payment>>(response);
            return payments?.FirstOrDefault();
        }

        public async Task<Payment> UpdateAsync(Payment entity)
        {
            var response = await _supabaseClient.PatchDataAsync(TableName, $"id=eq.{entity.Id}", entity);
            var payments = JsonSerializer.Deserialize<IEnumerable<Payment>>(response);
            return payments?.FirstOrDefault();
        }

        public async Task DeleteAsync(int id)
        {
            await _supabaseClient.DeleteDataAsync(TableName, $"id=eq.{id}");
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByReservationIdAsync(int reservationId)
        {
            var response = await _supabaseClient.GetFilteredDataAsync(TableName, $"reservationId=eq.{reservationId}");
            return JsonSerializer.Deserialize<IEnumerable<Payment>>(response) ?? new List<Payment>();
        }

        public async Task<Payment> GetPaymentByTransactionIdAsync(string transactionId)
        {
            var response = await _supabaseClient.GetFilteredDataAsync(TableName, $"transactionId=eq.{transactionId}");
            var payments = JsonSerializer.Deserialize<IEnumerable<Payment>>(response);
            return payments?.FirstOrDefault();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status)
        {
            var response = await _supabaseClient.GetFilteredDataAsync(TableName, $"statut=eq.{status}");
            return JsonSerializer.Deserialize<IEnumerable<Payment>>(response) ?? new List<Payment>();
        }
    }
}