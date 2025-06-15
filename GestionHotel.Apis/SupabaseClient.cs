namespace GestionHotel.Data
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class SupabaseClient
    {
        private readonly HttpClient _client;
        private readonly string _url;
        private readonly string _apiKey;

        public SupabaseClient(string url, string apiKey)
        {
            _url = url.TrimEnd('/');
            _apiKey = apiKey;

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _client.DefaultRequestHeaders.Add("apikey", _apiKey);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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


        // Ajouter POST, PATCH, DELETE ici si besoin
    }
}