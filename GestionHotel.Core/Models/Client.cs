using System;
using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Core.Models
{
    public class Client
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Nom { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        public string Telephone { get; set; }
        
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
    }
}