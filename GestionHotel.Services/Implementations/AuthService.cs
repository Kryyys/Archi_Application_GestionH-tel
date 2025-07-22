using GestionHotel.Core.Constants;
using GestionHotel.Core.Exceptions;
using GestionHotel.Data.Repositories;
using GestionHotel.Services.DTOs;
using GestionHotel.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace GestionHotel.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;

        public AuthService(IUserRepository userRepository, IUserService userService, string jwtSecret = "your-super-secret-jwt-key-that-is-long-enough", string jwtIssuer = "GestionHotel")
        {
            _userRepository = userRepository;
            _userService = userService;
            _jwtSecret = jwtSecret;
            _jwtIssuer = jwtIssuer;
        }

        public async Task<LoginResultDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(loginDto.NomUtilisateur);
                
                if (user == null || !user.Actif)
                {
                    return new LoginResultDto
                    {
                        Success = false,
                        Message = BusinessRules.ErrorMessages.InvalidCredentials
                    };
                }

                // Vérifier le mot de passe
                if (!BCrypt.Net.BCrypt.Verify(loginDto.MotDePasse, user.MotDePasseHache))
                {
                    return new LoginResultDto
                    {
                        Success = false,
                        Message = BusinessRules.ErrorMessages.InvalidCredentials
                    };
                }

                // Mettre à jour la dernière connexion
                user.DerniereConnexion = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                var userDto = await _userService.GetUserByIdAsync(user.Id);
                var token = GenerateJwtToken(userDto);

                return new LoginResultDto
                {
                    Success = true,
                    Message = "Connexion réussie",
                    Token = token,
                    User = userDto,
                    ExpirationToken = DateTime.UtcNow.AddHours(24)
                };
            }
            catch (Exception ex)
            {
                return new LoginResultDto
                {
                    Success = false,
                    Message = "Erreur lors de la connexion"
                };
            }
        }

        public async Task<UserDto> RegisterAsync(CreateUserDto registerDto)
        {
            return await _userService.CreateUserAsync(registerDto);
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSecret);
                
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtIssuer,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserDto> GetCurrentUserAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwt = tokenHandler.ReadJwtToken(token);
                
                var userIdClaim = jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return await _userService.GetUserByIdAsync(userId);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public string GenerateJwtToken(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.NomUtilisateur),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = _jwtIssuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
