using System.Text.Json;
using GestionHotel.Models;
using GestionHotel.Data;

namespace GestionHotel.Apis.Endpoints.Booking;

public class GetAvailableRoomsInput
{
    private readonly SupabaseClient _supabaseClient;

    // Injection via constructeur
    public GetAvailableRoomsInput(SupabaseClient supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<List<Room>> GetAvailableRooms(DateTime startDate, DateTime endDate)
    {
        var chambresJson = await _supabaseClient.GetDataAsync("chambres");
        var allRooms = JsonSerializer.Deserialize<List<Room>>(chambresJson)!;

        var reservationsJson = await _supabaseClient.GetDataAsync("reservations");
        var allReservations = JsonSerializer.Deserialize<List<Reservation>>(reservationsJson)!;

        var reservationChambresJson = await _supabaseClient.GetDataAsync("reservation_chambres");
        var reservationLinks = JsonSerializer.Deserialize<List<ReservationChambre>>(reservationChambresJson)!;

        var conflictingReservations = allReservations
            .Where(r => !(endDate <= r.DateDebut || startDate >= r.DateFin))
            .Select(r => r.Id)
            .ToList();

        var bookedRoomIds = reservationLinks
            .Where(rc => conflictingReservations.Contains(rc.ReservationId))
            .Select(rc => rc.ChambreId)
            .Distinct()
            .ToList();

        var availableRooms = allRooms
            .Where(r => !bookedRoomIds.Contains(r.Id))
            .ToList();

        return availableRooms;
    }
}