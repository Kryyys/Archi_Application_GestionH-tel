using GestionHotel.Services.Interfaces;
using GestionHotel.Services.DTOs;
using System.Threading.Tasks;

namespace GestionHotel.Apis.Endpoints.Auth
{
    public class AuthHandler
    {
        private readonly IAuthService _authService;

        public AuthHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthResult> HandleLoginAsync(LoginInput input)
        {
            var loginDto = new LoginDto
            {
                NomUtilisateur = input.NomUtilisateur,
                MotDePasse = input.MotDePasse
            };

            var result = await _authService.LoginAsync(loginDto);
            
            return new AuthResult
            {
                Success = result.Success,
                Message = result.Message,
                Token = result.Token,
                User = result.User,
                ExpirationToken = result.ExpirationToken
            };
        }

        public async Task<AuthResult> HandleRegisterAsync(CreateUserDto registerDto)
        {
            var user = await _authService.RegisterAsync(registerDto);
            var token = _authService.GenerateJwtToken(user);

            return new AuthResult
            {
                Success = true,
                Message = "Inscription réussie",
                Token = token,
                User = user
            };
        }
    }
}