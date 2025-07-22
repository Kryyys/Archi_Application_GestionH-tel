using System;
using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Core.Models
{
    public class Damage
    {
        public int Id { get; set; }
        
        public int ChambreaId { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        public DateTime DateSignalement { get; set; } = DateTime.UtcNow;
        
        public int? UtilisateurId { get; set; }
        
        public decimal? MontantEstime { get; set; }
        
        public bool Resolu { get; set; } = false;
        
        public DateTime? DateResolution { get; set; }
        
        // Navigation properties
        public Room Chambre { get; set; }
        public User Utilisateur { get; set; }
    }
}