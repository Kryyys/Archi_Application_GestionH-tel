using GestionHotel.Core.Models;
using GestionHotel.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestionHotel.Data.Implementations
{
    public class CleaningRepository : ICleaningRepository
    {
        private readonly SupabaseClient _supabaseClient;
        private const string TableName = "cleaning_tasks";

        public CleaningRepository(SupabaseClient supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<IEnumerable<CleaningTask>> GetAllAsync()
        {
            var response = await _supabaseClient.GetDataAsync(TableName);
            return JsonSerializer.Deserialize<IEnumerable<CleaningTask>>(response) ?? new List<CleaningTask>();
        }

        public async Task<CleaningTask> GetByIdAsync(int id)
        {
            var response = await _supabaseClient.GetFilteredDataAsync(TableName, $"id=eq.{id}");
            var tasks = JsonSerializer.Deserialize<IEnumerable<CleaningTask>>(response);
            return tasks?.FirstOrDefault();
        }

        public async Task<CleaningTask> CreateAsync(CleaningTask entity)
        {
            var response = await _supabaseClient.PostDataAsync(TableName, entity);
            var tasks = JsonSerializer.Deserialize<IEnumerable<CleaningTask>>(response);
            return tasks?.FirstOrDefault();
        }

        public async Task<CleaningTask> UpdateAsync(CleaningTask entity)
        {
            var response = await _supabaseClient.PatchDataAsync(TableName, $"id=eq.{entity.Id}", entity);
            var tasks = JsonSerializer.Deserialize<IEnumerable<CleaningTask>>(response);
            return tasks?.FirstOrDefault();
        }

        public async Task DeleteAsync(int id)
        {
            await _supabaseClient.DeleteDataAsync(TableName, $"id=eq.{id}");
        }

        public async Task<IEnumerable<CleaningTask>> GetTasksByStatusAsync(string status)
        {
            var response = await _supabaseClient.GetFilteredDataAsync(TableName, $"status=eq.{status}");
            return JsonSerializer.Deserialize<IEnumerable<CleaningTask>>(response) ?? new List<CleaningTask>();
        }

        public async Task<IEnumerable<CleaningTask>> GetTasksByRoomIdAsync(int roomId)
        {
            var response = await _supabaseClient.GetFilteredDataAsync(TableName, $"roomId=eq.{roomId}");
            return JsonSerializer.Deserialize<IEnumerable<CleaningTask>>(response) ?? new List<CleaningTask>();
        }

        public async Task<CleaningTask> GetLatestTaskForRoomAsync(int roomId)
        {
            var response = await _supabaseClient.GetFilteredDataAsync(TableName, $"roomId=eq.{roomId}&order=dateCreation.desc&limit=1");
            var tasks = JsonSerializer.Deserialize<IEnumerable<CleaningTask>>(response);
            return tasks?.FirstOrDefault();
        }

        public async Task<IEnumerable<CleaningTask>> GetPendingTasksAsync()
        {
            var response = await _supabaseClient.GetFilteredDataAsync(TableName, "status=eq.EnAttente");
            return JsonSerializer.Deserialize<IEnumerable<CleaningTask>>(response) ?? new List<CleaningTask>();
        }
    }
}
