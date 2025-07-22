using GestionHotel.Core.Models;
using GestionHotel.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestionHotel.Data.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly SupabaseClient _supabaseClient;
        private const string TableName = "utilisateurs";

        public UserRepository(SupabaseClient supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var response = await _supabaseClient.GetDataAsync(TableName);
            return JsonSerializer.Deserialize<IEnumerable<User>>(response) ?? new List<User>();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var response = await _supabaseClient.GetFilteredDataAsync(TableName, $"id=eq.{id}");
            var users = JsonSerializer.Deserialize<IEnumerable<User>>(response);
            return users?.FirstOrDefault();
        }

        public async Task<User> CreateAsync(User entity)
        {
            var response = await _supabaseClient.PostDataAsync(TableName, entity);
            var users = JsonSerializer.Deserialize<IEnumerable<User>>(response);
            return users?.FirstOrDefault();
        }

        public async Task<User> UpdateAsync(User entity)
        {
            var response = await _supabaseClient.PatchDataAsync(TableName, $"id=eq.{entity.Id}", entity);
            var users = JsonSerializer.Deserialize<IEnumerable<User>>(response);
            return users?.FirstOrDefault();
        }

        public async Task DeleteAsync(int id)
        {
            await _supabaseClient.DeleteDataAsync(TableName, $"id=eq.{id}");
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var response = await _supabaseClient.GetFilteredDataAsync(TableName, $"email=eq.{email}");
            var users = JsonSerializer.Deserialize<IEnumerable<User>>(response);
            return users?.FirstOrDefault();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var response = await _supabaseClient.GetFilteredDataAsync(TableName, $"nomUtilisateur=eq.{username}");
            var users = JsonSerializer.Deserialize<IEnumerable<User>>(response);
            return users?.FirstOrDefault();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var user = await GetByEmailAsync(email);
            return user != null;
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            var user = await GetByUsernameAsync(username);
            return user != null;
        }
    }
}
