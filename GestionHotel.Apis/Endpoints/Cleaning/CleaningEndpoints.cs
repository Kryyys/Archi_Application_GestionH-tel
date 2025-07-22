using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GestionHotel.Apis.Endpoints.Cleaning
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "PersonnelMenage,Receptionniste,Administrateur")]
    public class CleaningController : ControllerBase
    {
        private readonly CleaningHandler _handler;

        public CleaningController(CleaningHandler handler)
        {
            _handler = handler;
        }

        [HttpGet("tasks/pending")]
        public async Task<IActionResult> GetPendingTasks()
        {
            var result = await _handler.HandleGetPendingTasksAsync();
            return Ok(result);
        }

        [HttpGet("tasks/my")]
        [Authorize(Roles = "PersonnelMenage")]
        public async Task<IActionResult> GetMyTasks()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Utilisateur non identifié");
            }

            var result = await _handler.HandleGetMyTasksAsync(userId);
            return Ok(result);
        }

        [HttpPost("tasks")]
        [Authorize(Roles = "Receptionniste,Administrateur")]
        public async Task<IActionResult> CreateTask([FromBody] CleaningInput input)
        {
            var result = await _handler.HandleCreateTaskAsync(input);
            return CreatedAtAction(nameof(GetTask), new { id = result.TaskId }, result);
        }

        [HttpGet("tasks/{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var result = await _handler.HandleGetTaskAsync(id);
            return Ok(result);
        }

        [HttpPost("tasks/{id}/complete")]
        [Authorize(Roles = "PersonnelMenage")]
        public async Task<IActionResult> CompleteTask(int id, [FromBody] CleaningInput input)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Utilisateur non identifié");
            }

            var result = await _handler.HandleCompleteTaskAsync(id, userId, input.Notes);
            return Ok(result);
        }
    }
}