using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Services.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        
        [Required]
        public int ClientId { get; set; }
        
        [Required]
        public DateTime DateDebut { get; set; }
        
        [Required]
        public DateTime DateFin { get; set; }
        
        public string Statut { get; set; }
        
        public bool PaiementEffectue { get; set; }
        
        public bool RemboursementEffectue { get; set; }
        
        public decimal MontantTotal { get; set; }
        
        public DateTime DateCreation { get; set; }
        
        public string Commentaires { get; set; }
        
        // Informations du client
        public string ClientNom { get; set; }
        public string ClientEmail { get; set; }
        public string ClientTelephone { get; set; }
        
        // Chambres réservées
        public List<int> ChambresIds { get; set; } = new List<int>();
        public List<ReservationChambreDto> Chambres { get; set; } = new List<ReservationChambreDto>();
    }

    public class ReservationChambreDto
    {
        public int ChambreaId { get; set; }
        public string ChambreaNumero { get; set; }
        public string ChambreaType { get; set; }
        public decimal ChambreaTarif { get; set; }
        public int ChambreaCapacite { get; set; }
        public string ChambreaEtat { get; set; }
    }
    
    public class CreateReservationDto
    {
        [Required]
        public int ClientId { get; set; }
        
        [Required]
        public DateTime DateDebut { get; set; }
        
        [Required]
        public DateTime DateFin { get; set; }
        
        [Required]
        [MinLength(1, ErrorMessage = "Au moins une chambre doit être sélectionnée")]
        public List<int> ChambresIds { get; set; } = new List<int>();
        
        public string Commentaires { get; set; }
        
        // Validation personnalisée
        public bool IsValid()
        {
            return DateDebut < DateFin && 
                   DateDebut >= DateTime.Today &&
                   ChambresIds.Count > 0;
        }
    }
    
    public class UpdateReservationDto
    {
        [Required]
        public int Id { get; set; }
        
        public DateTime? DateDebut { get; set; }
        
        public DateTime? DateFin { get; set; }
        
        public List<int> ChambresIds { get; set; }
        
        public string Commentaires { get; set; }
        
        public string Statut { get; set; }
        
        // Pour la réception
        public bool? PaiementEffectue { get; set; }
    }
}