using GestionHotel.Core.Constants;
using GestionHotel.Core.Enums;
using GestionHotel.Core.Exceptions;
using GestionHotel.Core.Models;
using GestionHotel.Data.Repositories;
using GestionHotel.Services.DTOs;
using GestionHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionHotel.Services.Implementations
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IPaymentService _paymentService;
        private readonly ICleaningService _cleaningService;
        private readonly INotificationService _notificationService;

        public ReservationService(
            IReservationRepository reservationRepository,
            IPaymentService paymentService,
            ICleaningService cleaningService,
            INotificationService notificationService)
        {
            _reservationRepository = reservationRepository;
            _paymentService = paymentService;
            _cleaningService = cleaningService;
            _notificationService = notificationService;
        }

        public async Task<ReservationDto> CreateReservationAsync(CreateReservationDto createDto)
        {
            // Validation
            if (!createDto.IsValid())
            {
                throw new ReservationException("Données de réservation invalides", BusinessRules.ErrorCodes.VALIDATION_001);
            }

            // Vérifier la disponibilité des chambres
            foreach (var roomId in createDto.ChambresIds)
            {
                var isAvailable = await _reservationRepository.IsRoomAvailableAsync(
                    roomId, createDto.DateDebut, createDto.DateFin);
                
                if (!isAvailable)
                {
                    throw new ReservationException(
                        $"La chambre {roomId} n'est pas disponible pour ces dates",
                        BusinessRules.ErrorCodes.RESERVATION_002);
                }
            }

            // Créer la réservation
            var reservation = new Reservation
            {
                ClientId = createDto.ClientId,
                DateDebut = createDto.DateDebut,
                DateFin = createDto.DateFin,
                Statut = ReservationStatus.EnAttente.ToString(),
                Commentaires = createDto.Commentaires,
                DateCreation = DateTime.UtcNow
                // MontantTotal sera calculé selon les tarifs des chambres
            };

            var createdReservation = await _reservationRepository.CreateAsync(reservation);

            // Créer les associations réservation-chambre
            // (Cela nécessiterait un ReservationChambreRepository)

            return await ConvertToDto(createdReservation);
        }

        public async Task<ReservationDto> GetReservationByIdAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null)
            {
                throw new ReservationException(
                    BusinessRules.ErrorMessages.ReservationNotFound,
                    BusinessRules.ErrorCodes.RESERVATION_001);
            }

            return await ConvertToDto(reservation);
        }

        public async Task<IEnumerable<ReservationDto>> GetReservationsByClientIdAsync(int clientId)
        {
            var reservations = await _reservationRepository.GetReservationsByClientIdAsync(clientId);
            var dtos = new List<ReservationDto>();

            foreach (var reservation in reservations)
            {
                dtos.Add(await ConvertToDto(reservation));
            }

            return dtos;
        }

        public async Task<ReservationDto> UpdateReservationAsync(UpdateReservationDto updateDto)
        {
            var reservation = await _reservationRepository.GetByIdAsync(updateDto.Id);
            if (reservation == null)
            {
                throw new ReservationException(
                    BusinessRules.ErrorMessages.ReservationNotFound,
                    BusinessRules.ErrorCodes.RESERVATION_001);
            }

            // Mise à jour des propriétés
            if (updateDto.DateDebut.HasValue)
                reservation.DateDebut = updateDto.DateDebut.Value;
            
            if (updateDto.DateFin.HasValue)
                reservation.DateFin = updateDto.DateFin.Value;
            
            if (!string.IsNullOrEmpty(updateDto.Commentaires))
                reservation.Commentaires = updateDto.Commentaires;
            
            if (!string.IsNullOrEmpty(updateDto.Statut))
                reservation.Statut = updateDto.Statut;

            if (updateDto.PaiementEffectue.HasValue)
                reservation.PaiementEffectue = updateDto.PaiementEffectue.Value;

            reservation.DateModification = DateTime.UtcNow;

            var updatedReservation = await _reservationRepository.UpdateAsync(reservation);
            return await ConvertToDto(updatedReservation);
        }

        public async Task<CancellationResultDto> CancelReservationAsync(CancellationDto cancellationDto)
        {
            var reservation = await _reservationRepository.GetByIdAsync(cancellationDto.ReservationId);
            if (reservation == null)
            {
                throw new ReservationException(
                    BusinessRules.ErrorMessages.ReservationNotFound,
                    BusinessRules.ErrorCodes.RESERVATION_001);
            }

            var result = new CancellationResultDto
            {
                DateLimiteRemboursement = reservation.DateDebut.AddHours(-BusinessRules.HeuresAnnulationGratuite)
            };

            // Calculer le remboursement
            var hoursUntilStay = (reservation.DateDebut - DateTime.UtcNow).TotalHours;
            var canRefund = hoursUntilStay >= BusinessRules.HeuresAnnulationGratuite || cancellationDto.ForceRemboursement;

            if (canRefund)
            {
                result.RemboursementApplique = true;
                result.MontantRembourse = reservation.MontantTotal;
                result.FraisAppliques = 0;
            }
            else
            {
                result.RemboursementApplique = false;
                result.MontantRembourse = 0;
                result.FraisAppliques = BusinessRules.FraisAnnulationTardive;
                result.RaisonRefusRemboursement = "Annulation effectuée moins de 48h avant le séjour";
            }

            // Mettre à jour la réservation
            reservation.Statut = ReservationStatus.Annulee.ToString();
            reservation.DateModification = DateTime.UtcNow;
            reservation.Commentaires += $" | Annulée le {DateTime.UtcNow:dd/MM/yyyy} - {cancellationDto.Raison}";

            await _reservationRepository.UpdateAsync(reservation);

            // Traiter le remboursement si applicable
            if (result.RemboursementApplique && reservation.PaiementEffectue)
            {
                var payments = await _paymentService.GetPaymentsByReservationIdAsync(cancellationDto.ReservationId);
                foreach (var payment in payments.Where(p => p.Statut == PaymentStatus.Reussie.ToString()))
                {
                    await _paymentService.RefundPaymentAsync(payment.Id, payment.Montant, cancellationDto.Raison);
                }
            }

            result.Success = true;
            result.Message = "Réservation annulée avec succès";

            return result;
        }

        public async Task<CheckInOutResultDto> CheckInAsync(CheckInDto checkInDto)
        {
            var reservation = await _reservationRepository.GetByIdAsync(checkInDto.ReservationId);
            if (reservation == null)
            {
                throw new ReservationException(BusinessRules.ErrorMessages.ReservationNotFound);
            }

            // Mettre à jour le statut
            reservation.Statut = ReservationStatus.CheckedIn.ToString();
            reservation.DateModification = DateTime.UtcNow;

            if (checkInDto.PaiementRestantEffectue && checkInDto.MontantPaiementRestant.HasValue)
            {
                reservation.PaiementEffectue = true;
                // Traiter le paiement restant via PaymentService
            }

            var updatedReservation = await _reservationRepository.UpdateAsync(reservation);

            return new CheckInOutResultDto
            {
                Success = true,
                Message = "Check-in effectué avec succès",
                Reservation = await ConvertToDto(updatedReservation)
            };
        }

        public async Task<CheckInOutResultDto> CheckOutAsync(CheckOutDto checkOutDto)
        {
            var reservation = await _reservationRepository.GetByIdAsync(checkOutDto.ReservationId);
            if (reservation == null)
            {
                throw new ReservationException(BusinessRules.ErrorMessages.ReservationNotFound);
            }

            // Mettre à jour le statut
            reservation.Statut = ReservationStatus.CheckedOut.ToString();
            reservation.DateModification = DateTime.UtcNow;

            var result = new CheckInOutResultDto
            {
                Success = true,
                Message = "Check-out effectué avec succès",
                Reservation = await ConvertToDto(reservation)
            };

            // Créer les tâches de nettoyage pour toutes les chambres
            // (Nécessiterait l'accès aux chambres de la réservation)

            await _reservationRepository.UpdateAsync(reservation);

            // Programmer notification post-séjour
            await _notificationService.SendPostStayNotificationAsync(checkOutDto.ReservationId);

            return result;
        }

        public async Task<IEnumerable<ReservationDto>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var reservations = await _reservationRepository.GetReservationsByDateRangeAsync(startDate, endDate);
            var dtos = new List<ReservationDto>();

            foreach (var reservation in reservations)
            {
                dtos.Add(await ConvertToDto(reservation));
            }

            return dtos;
        }

        public async Task<IEnumerable<ReservationDto>> GetTodayArrivalsAsync()
        {
            var today = DateTime.Today;
            var reservations = await _reservationRepository.GetReservationsByDateRangeAsync(today, today);
            var arrivals = reservations.Where(r => r.DateDebut.Date == today && 
                                                  (r.Statut == ReservationStatus.Confirmee.ToString() || 
                                                   r.Statut == ReservationStatus.EnAttente.ToString()));
            
            var dtos = new List<ReservationDto>();
            foreach (var reservation in arrivals)
            {
                dtos.Add(await ConvertToDto(reservation));
            }

            return dtos;
        }

        public async Task<IEnumerable<ReservationDto>> GetTodayDeparturesAsync()
        {
            var today = DateTime.Today;
            var reservations = await _reservationRepository.GetReservationsByDateRangeAsync(today, today);
            var departures = reservations.Where(r => r.DateFin.Date == today && 
                                                    r.Statut == ReservationStatus.CheckedIn.ToString());
            
            var dtos = new List<ReservationDto>();
            foreach (var reservation in departures)
            {
                dtos.Add(await ConvertToDto(reservation));
            }

            return dtos;
        }

        private async Task<ReservationDto> ConvertToDto(Reservation reservation)
        {
            return new ReservationDto
            {
                Id = reservation.Id,
                ClientId = reservation.ClientId,
                DateDebut = reservation.DateDebut,
                DateFin = reservation.DateFin,
                Statut = reservation.Statut,
                PaiementEffectue = reservation.PaiementEffectue,
                RemboursementEffectue = reservation.RemboursementEffectue,
                MontantTotal = reservation.MontantTotal,
                DateCreation = reservation.DateCreation,
                Commentaires = reservation.Commentaires
                // Les informations client et chambres seraient chargées ici
            };
        }
    }
}