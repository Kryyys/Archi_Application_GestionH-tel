using GestionHotel.Services.Interfaces;
using GestionHotel.Services.DTOs;
using System.Threading.Tasks;

namespace GestionHotel.Apis.Endpoints.Cleaning
{
    public class CleaningHandler
    {
        private readonly ICleaningService _cleaningService;

        public CleaningHandler(ICleaningService cleaningService)
        {
            _cleaningService = cleaningService;
        }

        public async Task<CleaningResult> HandleGetPendingTasksAsync()
        {
            var tasks = await _cleaningService.GetPendingTasksAsync();
            
            return new CleaningResult
            {
                Success = true,
                Tasks = tasks
            };
        }

        public async Task<CleaningResult> HandleGetMyTasksAsync(int userId)
        {
            var tasks = await _cleaningService.GetTasksByUserIdAsync(userId);
            
            return new CleaningResult
            {
                Success = true,
                Tasks = tasks
            };
        }

        public async Task<CleaningResult> HandleCreateTaskAsync(CleaningInput input)
        {
            var createDto = new CreateCleaningTaskDto
            {
                RoomId = input.RoomId,
                Priority = input.Priority,
                Notes = input.Notes,
                TypeNettoyage = input.TypeNettoyage,
                AssignedToUserId = input.AssignedToUserId
            };

            var task = await _cleaningService.CreateCleaningTaskAsync(createDto);
            
            return new CleaningResult
            {
                Success = true,
                Message = "Tâche créée avec succès",
                TaskId = task.Id,
                Task = task
            };
        }

        public async Task<CleaningResult> HandleGetTaskAsync(int id)
        {
            // Cette méthode nécessiterait une implémentation dans ICleaningService
            return new CleaningResult
            {
                Success = true,
                Message = "Fonctionnalité à implémenter"
            };
        }

        public async Task<CleaningResult> HandleCompleteTaskAsync(int taskId, int userId, string notes)
        {
            var task = await _cleaningService.CompleteTaskAsync(taskId, userId, notes);
            
            return new CleaningResult
            {
                Success = true,
                Message = "Tâche marquée comme terminée",
                Task = task
            };
        }
    }

    public class CleaningResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int? TaskId { get; set; }
        public CleaningTaskDto Task { get; set; }
        public System.Collections.Generic.IEnumerable<CleaningTaskDto> Tasks { get; set; }
    }
}
