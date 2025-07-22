using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Services.DTOs
{
    public class AvailableRoomsRequestDto
    {
        [Required]
        public DateTime DateDebut { get; set; }
        
        [Required]
        public DateTime DateFin { get; set; }
        
        public int? NombrePersonnes { get; set; }
        
        public string TypeChambre { get; set; }
        
        public decimal? BudgetMax { get; set; }
        
        // Pour les réceptionnistes - voir l'état des chambres
        public bool IncludeRoomStatus { get; set; } = false;
    }

    public class AvailableRoomDto
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public int TypeId { get; set; }
        public string TypeNom { get; set; }
        public decimal Tarif { get; set; }
        public int Capacite { get; set; }
        public string Etat { get; set; }
        
        // Calculs pour la période demandée
        public int NombreNuits { get; set; }
        public decimal MontantTotal { get; set; }
        
        // Pour les réceptionnistes
        public string StatusDetails { get; set; }
        public DateTime? LastCleaning { get; set; }
        public bool NeedsSpecialAttention { get; set; }
    }

    public class AvailableRoomsResponseDto
    {
        public List<AvailableRoomDto> Rooms { get; set; } = new List<AvailableRoomDto>();
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public int NombreNuits { get; set; }
        public int TotalRoomsAvailable { get; set; }
        public decimal PrixMinimum { get; set; }
        public decimal PrixMaximum { get; set; }
    }
    
    public class CheckInDto
    {
        [Required]
        public int ReservationId { get; set; }
        
        public DateTime DateArrivee { get; set; } = DateTime.Now;
        
        public string NotesReception { get; set; }
        
        public bool PaiementRestantEffectue { get; set; } = false;
        
        public decimal? MontantPaiementRestant { get; set; }
        
        public int ReceptionnisteuId { get; set; }
    }

    public class CheckOutDto
    {
        [Required]
        public int ReservationId { get; set; }
        
        public DateTime DateDepart { get; set; } = DateTime.Now;
        
        public string NotesReception { get; set; }
        
        public bool PaiementComplementaireEffectue { get; set; } = false;
        
        public decimal? MontantPaiementComplementaire { get; set; }
        
        public bool DegatsSignales { get; set; } = false;
        
        public string DescriptionDegats { get; set; }
        
        public decimal? MontantDegats { get; set; }
        
        public int ReceptionnisteId { get; set; }
        
        // Satisfaction client (optionnel)
        public int? NoteSatisfaction { get; set; } // 1-5
        public string CommentaireSatisfaction { get; set; }
    }

    public class CheckInOutResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ReservationDto Reservation { get; set; }
        public List<CleaningTaskDto> CleaningTasksCreated { get; set; } = new List<CleaningTaskDto>();
    }
}