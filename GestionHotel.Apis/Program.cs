using System;
using System.Threading.Tasks;
using GestionHotel.Data; // adapte selon ton namespace réel

class Program
{
    static async Task Main()
    {
        var supabaseUrl = "https://btsjagbyufbyxerqzpgp.supabase.co";  // URL API Supabase, pas le dashboard
        var supabaseApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImJ0c2phZ2J5dWZieXhlcnF6cGdwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDk1NDM1NDgsImV4cCI6MjA2NTExOTU0OH0.jvymbrUC69f_wabVPU2XMrOitrScpb_VUyMD2RlX6Cg";               // Ta vraie clé API

        var client = new SupabaseClient(supabaseUrl, supabaseApiKey);

        try
        {
            string json = await client.GetDataAsync("chambres");  // Nom de ta table
            Console.WriteLine(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur : " + ex.Message);
        }
    }
}

