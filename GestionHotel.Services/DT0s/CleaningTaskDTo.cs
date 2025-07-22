using System;
using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Services.DTOs
{
    public class CleaningTaskDto
    {
        public int Id { get; set; }
        
        public int RoomId { get; set; }
        
        public string Status { get; set; }
        
        public DateTime DateCreation { get; set; }
        
        public DateTime? DateCompletion { get; set; }
        
        public int? AssignedToUserId { get; set; }
        
        public bool Priority { get; set; }
        
        public string Notes { get; set; }
        
        public string TypeNettoyage { get; set; }
        
        public int? DureeEstimeeMinutes { get; set; }
        
        // Informations de la chambre
        public string RoomNumero { get; set; }
        public string RoomEtat { get; set; }
        public string RoomType { get; set; }
        
        // Informations de l'assigné
        public string AssignedToUserName { get; set; }
        public string AssignedToUserEmail { get; set; }
    }

    public class CreateCleaningTaskDto
    {
        [Required]
        public int RoomId { get; set; }
        
        public bool Priority { get; set; } = false;
        
        public string Notes { get; set; }
        
        public string TypeNettoyage { get; set; } = "Standard";
        
        public int? AssignedToUserId { get; set; }
        
        public int? DureeEstimeeMinutes { get; set; }
    }

    public class UpdateCleaningTaskDto
    {
        [Required]
        public int Id { get; set; }
        
        public string Status { get; set; }
        
        public int? AssignedToUserId { get; set; }
        
        public string Notes { get; set; }
        
        public bool? Priority { get; set; }
        
        public DateTime? DateCompletion { get; set; }
    }
}