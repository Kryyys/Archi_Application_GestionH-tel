using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionHotel.Services.Interfaces;
using GestionHotel.Services.DTOs;
using System.Threading.Tasks;

namespace GestionHotel.Apis.Endpoints.Booking
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingHandler _handler;

        public BookingController(BookingHandler handler)
        {
            _handler = handler;
        }

        [HttpGet("available-rooms")]
        public async Task<IActionResult> GetAvailableRooms([FromQuery] GetAvailableRoomsInput input)
        {
            var result = await _handler.HandleGetAvailableRoomsAsync(input);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBooking([FromBody] BookingInput input)
        {
            var result = await _handler.HandleCreateBookingAsync(input);
            return CreatedAtAction(nameof(GetBooking), new { id = result.ReservationId }, result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetBooking(int id)
        {
            var result = await _handler.HandleGetBookingAsync(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Receptionniste,Administrateur")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] BookingInput input)
        {
            var result = await _handler.HandleUpdateBookingAsync(id, input);
            return Ok(result);
        }

        [HttpPost("{id}/cancel")]
        [Authorize]
        public async Task<IActionResult> CancelBooking(int id, [FromBody] CancellationInput input)
        {
            var result = await _handler.HandleCancelBookingAsync(id, input);
            return Ok(result);
        }

        [HttpPost("{id}/payment")]
        [Authorize]
        public async Task<IActionResult> ProcessPayment(int id, [FromBody] PaymentInput input)
        {
            var result = await _handler.HandlePaymentAsync(id, input);
            return Ok(result);
        }
    }
}
