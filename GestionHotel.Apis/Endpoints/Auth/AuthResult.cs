using GestionHotel.Services.DTOs;
using System;

namespace GestionHotel.Apis.Endpoints.Auth
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public UserDto User { get; set; }
        public DateTime? ExpirationToken { get; set; }
    }
}