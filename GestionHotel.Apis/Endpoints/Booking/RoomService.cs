using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GestionHotel.Data;
using GestionHotel.Models;

namespace GestionHotel.Services
{
    public class RoomService : IRoomService
    {
        private readonly SupabaseClient _supabaseClient;

        public RoomService(SupabaseClient supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<List<Room>> GetAllRoomsAsync()
        {
            var json = await _supabaseClient.GetDataAsync("chambres?select=*,types_chambres(*)");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var rawRooms = JsonSerializer.Deserialize<List<RawRoom>>(json, options);

            return rawRooms.Select(MapToRoom).ToList();
        }

        public async Task<List<Room>> GetAvailableRooms(DateTime startDate, DateTime endDate)
        {
            // 1. Récupérer toutes les chambres
            var allRooms = await GetAllRoomsAsync();

            // 2. Récupérer toutes les chambres réservées dans l'intervalle
            var reservationsJson = await _supabaseClient.GetDataAsync(
                $"reservations?select=id,date_debut,date_fin,reservation_chambres(chambre_id)&" +
                $"or=(and(date_debut.lte.{endDate:yyyy-MM-dd},date_fin.gte.{startDate:yyyy-MM-dd}))"
            );

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var reserved = JsonSerializer.Deserialize<List<ReservationRaw>>(reservationsJson, options);

            var reservedRoomIds = reserved
                .SelectMany(r => r.Reservation_Chambres)
                .Select(rc => rc.Chambre_Id)
                .Distinct()
                .ToHashSet();

            return allRooms.Where(room => !reservedRoomIds.Contains(room.Id)).ToList();
        }

        // === Private Mapping ===

        private Room MapToRoom(RawRoom raw)
        {
            return new Room
            {
                Id = raw.Id,
                Type = raw.Types_Chambres?.Nom ?? "Inconnu",
                Capacity = raw.Capacite,
                PricePerNight = raw.Types_Chambres?.Tarif ?? 0,
                Status = ParseRoomStatus(raw.Etat)
            };
        }

        private RoomStatus ParseRoomStatus(string etat)
        {
            return etat?.ToLower() switch
            {
                "neuf" => RoomStatus.Neuf,
                "refaite" => RoomStatus.Refaite,
                "arefaire" => RoomStatus.ARefaire,
                "rienasignaler" => RoomStatus.RienASignaler,
                "grosdegats" => RoomStatus.GrosDegats,
                _ => RoomStatus.RienASignaler
            };
        }

        // === Internal DTOs pour la désérialisation JSON ===

        private class RawRoom
        {
            public int Id { get; set; }
            public int Capacite { get; set; }
            public string Etat { get; set; }
            public RawRoomType Types_Chambres { get; set; }
        }

        private class RawRoomType
        {
            public string Nom { get; set; }
            public decimal Tarif { get; set; }
        }

        private class ReservationRaw
        {
            public int Id { get; set; }
            public DateTime Date_Debut { get; set; }
            public DateTime Date_Fin { get; set; }
            public List<ReservationRoomLink> Reservation_Chambres { get; set; }
        }

        private class ReservationRoomLink
        {
            public int Chambre_Id { get; set; }
        }
    }
}

