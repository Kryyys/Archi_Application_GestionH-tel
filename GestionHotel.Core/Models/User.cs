using System;
using System.ComponentModel.DataAnnotations;
using GestionHotel.Core.Enums;

namespace GestionHotel.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string NomUtilisateur { get; set; }
        
        [Required]
        public string MotDePasseHache { get; set; }
        
        public string Role { get; set; } = UserRole.Client.ToString();
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        public string Prenom { get; set; }
        
        public string Nom { get; set; }
        
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        
        public DateTime? DerniereConnexion { get; set; }
        
        public bool Actif { get; set; } = true;
    }
}