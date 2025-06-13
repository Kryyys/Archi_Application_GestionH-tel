using Microsoft.AspNetCore.Mvc;

namespace GestionHotel.Apis.Endpoints.Booking;

public static class BookingHandler
{
    public static async Task<IResult> GetAvailableRooms(
        [FromServices] IRoomService roomService,
        [AsParameters] GetAvailableRoomsInput input)
    {
        var rooms = await roomService.GetAvailableRooms(input.StartDate, input.EndDate);
        return Results.Ok(rooms);
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
