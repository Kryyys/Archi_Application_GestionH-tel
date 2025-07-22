using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionHotel.Apis.Endpoints.Reception
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Receptionniste,Administrateur")]
    public class ReceptionController : ControllerBase
    {
        private readonly ReceptionHandler _handler;

        public ReceptionController(ReceptionHandler handler)
        {
            _handler = handler;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _handler.HandleGetDashboardAsync();
            return Ok(result);
        }

        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInOutInput input)
        {
            var result = await _handler.HandleCheckInAsync(input);
            return Ok(result);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut([FromBody] CheckInOutInput input)
        {
            var result = await _handler.HandleCheckOutAsync(input);
            return Ok(result);
        }

        [HttpGet("reservations/today")]
        public async Task<IActionResult> GetTodayReservations()
        {
            var result = await _handler.HandleGetTodayReservationsAsync();
            return Ok(result);
        }
    }
}