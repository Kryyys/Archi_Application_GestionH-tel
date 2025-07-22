using GestionHotel.Services.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionHotel.Services.Interfaces
{
    public interface ICleaningService
    {
        Task<CleaningTaskDto> CreateCleaningTaskAsync(CreateCleaningTaskDto createDto);
        Task<CleaningTaskDto> UpdateCleaningTaskAsync(UpdateCleaningTaskDto updateDto);
        Task<IEnumerable<CleaningTaskDto>> GetPendingTasksAsync();
        Task<IEnumerable<CleaningTaskDto>> GetTasksByUserIdAsync(int userId);
        Task<CleaningTaskDto> CompleteTaskAsync(int taskId, int userId, string notes = null);
        Task<CleaningTaskDto> CreateTaskForCheckoutAsync(int roomId, string notes = null);
    }
}
