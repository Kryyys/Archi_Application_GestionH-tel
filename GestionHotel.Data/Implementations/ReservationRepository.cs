using GestionHotel.Core.Models;
using GestionHotel.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestionHotel.Data.Implementations
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly SupabaseClient _supabaseClient;
        private const string TableName = "reservations";

        public ReservationRepository(SupabaseClient supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            var response = await _supabaseClient.GetDataAsync(TableName);
            return JsonSerializer.Deserialize<IEnumerable<Reservation>>(response) ?? new List<Reservation>();
        }

        public async Task<Reservation> GetByIdAsync(int id)
        {
            var response = await _supabaseClient.GetFilteredDataAsync(TableName, $"id=eq.{id}");
            var reservations = JsonSerializer.Deserialize<IEnumerable<Reservation>>(response);
            return reservations?.FirstOrDefault();
        }

        public async Task<Reservation> CreateAsync(Reservation entity)
        {
            var response = await _supabaseClient.PostDataAsync(TableName, entity);
            var reservations = JsonSerializer.Deserialize<IEnumerable<Reservation>>(response);
            return reservations?.FirstOrDefault();
        }

        public async Task<Reservation> UpdateAsync(Reservation entity)
        {
            var response = await _supabaseClient.PatchDataAsync(TableName, $"id=eq.{entity.Id}", entity);
            var reservations = JsonSerializer.Deserialize<IEnumerable<Reservation>>(response);
            return reservations?.FirstOrDefault();
        }

        public async Task DeleteAsync(int id)
        {
            await _supabaseClient.DeleteDataAsync(TableName, $"id=eq.{id}");
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByClientIdAsync(int clientId)
        {
            var response = await _supabaseClient.GetFilteredDataAsync(TableName, $"clientId=eq.{clientId}");
            return JsonSerializer.Deserialize<IEnumerable<Reservation>>(response) ?? new List<Reservation>();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var filter = $"dateDebut=gte.{startDate:yyyy-MM-dd}&dateFin=lte.{endDate:yyyy-MM-dd}";
            var response = await _supabaseClient.GetFilteredDataAsync(TableName, filter);
            return JsonSerializer.Deserialize<IEnumerable<Reservation>>(response) ?? new List<Reservation>();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByStatusAsync(string status)
        {
            var response = await _supabaseClient.GetFilteredDataAsync(TableName, $"statut=eq.{status}");
            return JsonSerializer.Deserialize<IEnumerable<Reservation>>(response) ?? new List<Reservation>();
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime startDate, DateTime endDate, int? excludeReservationId = null)
        {
            var conflictingReservations = await GetConflictingReservationsAsync(roomId, startDate, endDate);
            
            if (excludeReservationId.HasValue)
            {
                conflictingReservations = conflictingReservations.Where(r => r.Id != excludeReservationId.Value);
            }
            
            return !conflictingReservations.Any();
        }

        public async Task<IEnumerable<Reservation>> GetConflictingReservationsAsync(int roomId, DateTime startDate, DateTime endDate)
        {
            // Récupérer toutes les réservations de cette chambre qui ne sont pas annulées
            var response = await _supabaseClient.GetFilteredDataAsync(TableName, $"statut=neq.Annulee");
            var allReservations = JsonSerializer.Deserialize<IEnumerable<Reservation>>(response) ?? new List<Reservation>();

            // Filtrer les réservations conflictuelles côté client (car Supabase REST API a des limitations sur les filtres complexes)
            return allReservations.Where(r => 
                // Vérifier si les dates se chevauchent
                r.DateDebut <= endDate && r.DateFin >= startDate &&
                r.Statut != "Annulee"
            );
        }
    }
}