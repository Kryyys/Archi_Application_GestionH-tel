using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using GestionHotel.Apis.Endpoints.Booking;
using GestionHotel.Apis.Endpoints.Auth;
using GestionHotel.Apis.Endpoints.Cleaning;
using GestionHotel.Apis.Endpoints.Reception;
using GestionHotel.Apis.Middleware;
using GestionHotel.Data;
using GestionHotel.Data.Repositories;
using GestionHotel.Data.Implementations;
using GestionHotel.Services;
using GestionHotel.Services.Interfaces;
using GestionHotel.Services.Implementations;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==============================================
// CONFIG SUPABASE (ton config existant)
// ==============================================
builder.Services.AddSingleton(new SupabaseClient(
    "https://btsjagbyufbyxerqzpgp.supabase.co", 
    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImJ0c2phZ2J5dWZieXhlcnF6cGdwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDk1NDM1NDgsImV4cCI6MjA2NTExOTU0OH0.jvymbrUC69f_wabVPU2XMrOitrScpb_VUyMD2RlX6Cg"     
));

// ==============================================
// CONFIGURATION JWT
// ==============================================
var secretKey = "your-super-secret-jwt-key-that-is-at-least-256-bits-long-for-security";
var issuer = "GestionHotel";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(5),
            RequireExpirationTime = true
        };
    });

builder.Services.AddAuthorization();

// ==============================================
// INJECTION DES REPOSITORIES
// ==============================================
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICleaningRepository, CleaningRepository>();

// ==============================================
// INJECTION DES SERVICES (ton service existant + nouveaux)
// ==============================================
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICleaningService, CleaningService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Service d'authentification avec JWT configuré
builder.Services.AddScoped<IAuthService>(provider =>
    new AuthService(
        provider.GetService<IUserRepository>(),
        provider.GetService<IUserService>(),
        secretKey,
        issuer
    ));

// ==============================================
// INJECTION DES HANDLERS
// ==============================================
builder.Services.AddScoped<BookingHandler>();
builder.Services.AddScoped<AuthHandler>();
builder.Services.AddScoped<CleaningHandler>();
builder.Services.AddScoped<ReceptionHandler>();

// ==============================================
// CONF CORS (ton config existant étendu)
// ==============================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
    
    // Politique plus restrictive pour la production
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",  // React
                "http://localhost:5173",  // Vite
                "http://localhost:8080"   // Vue
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// ==============================================
// SERVICES API (ton config existant + JWT pour Swagger)
// ==============================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "GestionHotel API", 
        Version = "v1",
        Description = "API de gestion d'hôtel avec réservations, paiements et ménage"
    });

    // Support JWT dans Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// ==============================================
// MIDDLEWARES (ton config existant + nouveaux)
// ==============================================
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

// Nouveaux middlewares pour la gestion des erreurs et l'auth
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// ==============================================
// ENDPOINTS (ton mapping existant + nouveaux)
// ==============================================
app.MapControllers();
app.MapBookingsEndpoints(); // Ton endpoint existant

// Endpoint d'information sur l'API
app.MapGet("/", () => new
{
    Service = "GestionHotel API",
    Version = "v1.0.0",
    Status = "Running",
    Timestamp = DateTime.UtcNow,
    Endpoints = new[]
    {
        "/api/auth/login - POST - Connexion",
        "/api/auth/register - POST - Inscription", 
        "/api/booking - Réservations (ton endpoint existant)",
        "/api/reception - Check-in/out",
        "/api/cleaning - Gestion ménage"
    }
});

app.Run();