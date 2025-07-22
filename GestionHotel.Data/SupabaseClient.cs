using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestionHotel.Data
{
    public class SupabaseClient
    {
        private readonly HttpClient _client;
        private readonly string _url;
        private readonly string _apiKey;
        private readonly JsonSerializerOptions _jsonOptions;

        public SupabaseClient(string url, string apiKey)
        {
            _url = url.TrimEnd('/');
            _apiKey = apiKey;

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _client.DefaultRequestHeaders.Add("apikey", _apiKey);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
        }

        public async Task<string> GetDataAsync(string tableName)
        {
            var response = await _client.GetAsync($"{_url}/rest/v1/{tableName}?select=*");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        
        public async Task<string> GetFilteredDataAsync(string tableName, string filter)
        {
            var response = await _client.GetAsync($"{_url}/rest/v1/{tableName}?{filter}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostDataAsync(string tableName, object data)
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.Add("Prefer", "return=representation");
            
            var response = await _client.PostAsync($"{_url}/rest/v1/{tableName}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PatchDataAsync(string tableName, string filter, object data)
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.Add("Prefer", "return=representation");
            
            var response = await _client.PatchAsync($"{_url}/rest/v1/{tableName}?{filter}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task DeleteDataAsync(string tableName, string filter)
        {
            var response = await _client.DeleteAsync($"{_url}/rest/v1/{tableName}?{filter}");
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}