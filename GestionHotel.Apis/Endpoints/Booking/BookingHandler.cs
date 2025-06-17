using Microsoft.AspNetCore.Mvc;
using GestionHotel.Services;

namespace GestionHotel.Apis.Endpoints.Booking;

public static class BookingHandler
{
    public static async Task<BookingView> GetAvailableRooms(
        HttpContext context,
        [AsParameters] GetAvailableRoomsInput input,
        IRoomService roomService)
    {
        var availableRooms = await roomService.GetAvailableRooms(input.StartDate, input.EndDate);
        
        // Appliquer des filtres optionnels si nécessaire
        if (input.Capacity.HasValue)
        {
            availableRooms = availableRooms.Where(r => r.Capacity >= input.Capacity.Value).ToList();
        }
        
        if (!string.IsNullOrEmpty(input.RoomType))
        {
            availableRooms = availableRooms.Where(r => r.Type.Equals(input.RoomType, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        return new BookingView
        {
            AvailableRooms = availableRooms,
            SearchPeriod = new SearchPeriod
            {
                StartDate = input.StartDate,
                EndDate = input.EndDate
            }
        };
    }

    public static async Task<IResult> Create(
        [FromBody] BookingInput input)
    {
        // Simule la création de réservation
        var result = new BookingResult
        {
            Success = true,
            Message = "Réservation créée avec succès"
        };

        return Results.Ok(result);
    }
}