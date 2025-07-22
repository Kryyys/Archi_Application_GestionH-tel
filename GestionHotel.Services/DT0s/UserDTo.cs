using System;
using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Services.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string NomUtilisateur { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? DerniereConnexion { get; set; }
        public bool Actif { get; set; }
    }

    public class CreateUserDto
    {
        [Required]
        [StringLength(50)]
        public string NomUtilisateur { get; set; }
        
        [Required]
        [MinLength(6)]
        public string MotDePasse { get; set; }
        
        [Required]
        public string Role { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        public string Prenom { get; set; }
        public string Nom { get; set; }
    }

    public class LoginDto
    {
        [Required]
        public string NomUtilisateur { get; set; }
        
        [Required]
        public string MotDePasse { get; set; }
    }

    public class LoginResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public UserDto User { get; set; }
        public DateTime? ExpirationToken { get; set; }
    }
}