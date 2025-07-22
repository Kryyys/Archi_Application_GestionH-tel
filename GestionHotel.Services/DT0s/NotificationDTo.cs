using System;
using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Services.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime DateEnvoi { get; set; }
        public bool Envoye { get; set; }
        public string Destinataire { get; set; }
        public string Sujet { get; set; }
        
        // Informations du client
        public string ClientNom { get; set; }
        public string ClientEmail { get; set; }
    }

    public class CreateNotificationDto
    {
        [Required]
        public int ClientId { get; set; }
        
        [Required]
        public string Type { get; set; }
        
        [Required]
        public string Message { get; set; }
        
        [Required]
        public string Destinataire { get; set; }
        
        public string Sujet { get; set; }
        
        public DateTime? DateEnvoiProgrammee { get; set; }
    }

    public class BulkNotificationDto
    {
        public string Type { get; set; }
        public string Sujet { get; set; }
        public string MessageTemplate { get; set; }
        public List<int> ClientIds { get; set; } = new List<int>();
        public DateTime? DateEnvoiProgrammee { get; set; }
    }
}