using GestionHotel.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionHotel.Data.Repositories
{
    public interface ICleaningRepository : IRepository<CleaningTask>
    {
        Task<IEnumerable<CleaningTask>> GetTasksByStatusAsync(string status);
        Task<IEnumerable<CleaningTask>> GetTasksByRoomIdAsync(int roomId);
        Task<CleaningTask> GetLatestTaskForRoomAsync(int roomId);
        Task<IEnumerable<CleaningTask>> GetPendingTasksAsync();
    }
}