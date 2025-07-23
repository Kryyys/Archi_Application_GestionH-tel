using GestionHotel.Apis.Endpoints.Booking;
using GestionHotel.Data;

var builder = WebApplication.CreateBuilder(args);

// Config Supabase
builder.Services.AddSingleton(new SupabaseClient(
    "https://btsjagbyufbyxerqzpgp.supabase.co", 
    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImJ0c2phZ2J5dWZieXhlcnF6cGdwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDk1NDM1NDgsImV4cCI6MjA2NTExOTU0OH0.jvymbrUC69f_wabVPU2XMrOitrScpb_VUyMD2RlX6Cg"     
));

// Handler simple
builder.Services.AddScoped<BookingHandler>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Services API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "GestionHotel API", 
        Version = "v1",
        Description = "API de gestion d'hôtel - Version de démonstration"
    });
});

var app = builder.Build();

// Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GestionHotel API v1");
        c.RoutePrefix = string.Empty; // Swagger à la racine
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

// Endpoints
app.MapControllers();

// Endpoint d'accueil
app.MapGet("/api/status", () => new
{
    Service = "GestionHotel API",
    Status = "Running",
    Version = "1.0.0",
    Timestamp = DateTime.UtcNow,
    Features = new[]
    {
        "Recherche de chambres disponibles",
        "Création de réservations",
        "Simulation de paiements",
        "Annulation avec règle 48h",
        "Interface Swagger complète"
    }
});

app.Run();