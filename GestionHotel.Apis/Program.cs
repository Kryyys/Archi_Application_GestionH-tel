using GestionHotel.Apis.Endpoints.Booking;
using GestionHotel.Data;
using GestionHotel.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Configuration Supabase
builder.Services.AddSingleton(new SupabaseClient(
    "https://btsjagbyufbyxerqzpgp.supabase.co", 
    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImJ0c2phZ2J5dWZieXhlcnF6cGdwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDk1NDM1NDgsImV4cCI6MjA2NTExOTU0OH0.jvymbrUC69f_wabVPU2XMrOitrScpb_VUyMD2RlX6Cg"     
));

// 💡 Injection des services
builder.Services.AddScoped<GetAvailableRoomsInput>();
builder.Services.AddScoped<IRoomService, RoomService>();

// 📦 Services nécessaires pour une API
builder.Services.AddControllers(); // si tu utilises des Controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🌍 Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Swagger UI sur /swagger
}

app.UseHttpsRedirection();
app.UseAuthorization();

// 🔗 Endpoints
app.MapControllers(); // Pour les Controllers (si tu en as)
app.MapBookingsEndpoints(); // Ton extension de booking endpoints

app.Run();