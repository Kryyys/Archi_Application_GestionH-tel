using Microsoft.AspNetCore.Mvc;

namespace GestionHotel.Apis.Endpoints.Booking;

public static class BookingHandler
{
    public static Task<BookingView> GetAvailableRooms(
        HttpContext context,
        [AsParameters] GetAvailableRoomsInput input)
    {
        return Task.FromResult(new BookingView());
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


