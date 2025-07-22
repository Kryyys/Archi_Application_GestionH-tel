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
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IReservationRepository _reservationRepository;

        public PaymentService(IPaymentRepository paymentRepository, IReservationRepository reservationRepository)
        {
            _paymentRepository = paymentRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<PaymentResultDto> ProcessPaymentAsync(CreatePaymentDto paymentDto)
        {
            try
            {
                // Simulation du service de paiement
                var paymentResult = await SimulatePaymentProcessingAsync(paymentDto);
                
                if (!paymentResult.Success)
                {
                    return new PaymentResultDto
                    {
                        Success = false,
                        Message = "Échec du traitement du paiement : " + paymentResult.Message
                    };
                }

                // Créer l'enregistrement du paiement
                var payment = new Payment
                {
                    ReservationId = paymentDto.ReservationId,
                    Montant = paymentDto.Montant,
                    Statut = PaymentStatus.Reussie.ToString(),
                    TransactionId = paymentResult.TransactionId,
                    MethodePaiement = paymentDto.MethodePaiement,
                    DatePaiement = DateTime.UtcNow,
                    DetailsCarte = MaskCardNumber(paymentDto.NumeroCarte)
                };

                var createdPayment = await _paymentRepository.CreateAsync(payment);

                // Mettre à jour la réservation
                var reservation = await _reservationRepository.GetByIdAsync(paymentDto.ReservationId);
                if (reservation != null)
                {
                    reservation.PaiementEffectue = true;
                    reservation.Statut = ReservationStatus.Confirmee.ToString();
                    await _reservationRepository.UpdateAsync(reservation);
                }

                return new PaymentResultDto
                {
                    Success = true,
                    TransactionId = createdPayment.TransactionId,
                    Message = "Paiement traité avec succès",
                    Payment = await ConvertToDto(createdPayment)
                };
            }
            catch (Exception ex)
            {
                throw new PaymentException("Erreur lors du traitement du paiement", BusinessRules.ErrorCodes.PAYMENT_001, ex);
            }
        }

        public async Task<PaymentDto> GetPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
            {
                throw new PaymentException("Paiement introuvable");
            }

            return await ConvertToDto(payment);
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByReservationIdAsync(int reservationId)
        {
            var payments = await _paymentRepository.GetPaymentsByReservationIdAsync(reservationId);
            var dtos = new List<PaymentDto>();

            foreach (var payment in payments)
            {
                dtos.Add(await ConvertToDto(payment));
            }

            return dtos;
        }

        public async Task<PaymentResultDto> RefundPaymentAsync(int paymentId, decimal amount, string reason)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                throw new PaymentException("Paiement introuvable");
            }

            if (payment.Statut != PaymentStatus.Reussie.ToString())
            {
                throw new PaymentException("Le paiement ne peut pas être remboursé");
            }

            // Simulation du remboursement
            var refundResult = await SimulateRefundProcessingAsync(payment, amount, reason);
            
            if (!refundResult.Success)
            {
                return new PaymentResultDto
                {
                    Success = false,
                    Message = "Échec du remboursement : " + refundResult.Message
                };
            }

            // Mettre à jour le statut du paiement
            if (amount >= payment.Montant)
            {
                payment.Statut = PaymentStatus.Remboursee.ToString();
            }
            else
            {
                payment.Statut = PaymentStatus.PartielleRemboursee.ToString();
            }

            await _paymentRepository.UpdateAsync(payment);

            // Créer l'enregistrement du remboursement
            var refund = new Refund
            {
                PaymentId = paymentId,
                ReservationId = payment.ReservationId,
                Montant = amount,
                Raison = reason,
                DateRemboursement = DateTime.UtcNow
            };

            return new PaymentResultDto
            {
                Success = true,
                TransactionId = refundResult.TransactionId,
                Message = "Remboursement effectué avec succès"
            };
        }

        public async Task<bool> ValidatePaymentAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            return payment != null && payment.Statut == PaymentStatus.Reussie.ToString();
        }

        private async Task<PaymentProcessingResult> SimulatePaymentProcessingAsync(CreatePaymentDto paymentDto)
        {
            // Simulation d'un service de paiement externe
            await Task.Delay(1000); // Simule le délai réseau

            // Validation basique des données de carte
            if (string.IsNullOrEmpty(paymentDto.NumeroCarte) || paymentDto.NumeroCarte.Length < 16)
            {
                return new PaymentProcessingResult { Success = false, Message = "Numéro de carte invalide" };
            }

            if (string.IsNullOrEmpty(paymentDto.CodeCVV) || paymentDto.CodeCVV.Length != 3)
            {
                return new PaymentProcessingResult { Success = false, Message = "Code CVV invalide" };
            }

            // Simulation d'un échec aléatoire (5% de chance)
            var random = new Random();
            if (random.Next(100) < 5)
            {
                return new PaymentProcessingResult { Success = false, Message = "Fonds insuffisants" };
            }

            return new PaymentProcessingResult
            {
                Success = true,
                TransactionId = GenerateTransactionId(),
                Message = "Paiement autorisé"
            };
        }

        private async Task<PaymentProcessingResult> SimulateRefundProcessingAsync(Payment originalPayment, decimal amount, string reason)
        {
            // Simulation du service de remboursement
            await Task.Delay(500);

            return new PaymentProcessingResult
            {
                Success = true,
                TransactionId = GenerateTransactionId(),
                Message = "Remboursement traité"
            };
        }

        private string GenerateTransactionId()
        {
            return $"TXN_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }

        private string MaskCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 4)
                return "****";

            return "**** **** **** " + cardNumber[^4..];
        }

        private async Task<PaymentDto> ConvertToDto(Payment payment)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                ReservationId = payment.ReservationId,
                Montant = payment.Montant,
                Statut = payment.Statut,
                TransactionId = payment.TransactionId,
                MethodePaiement = payment.MethodePaiement,
                DatePaiement = payment.DatePaiement,
                DetailsCarte = payment.DetailsCarte
                // Les informations de réservation et client seraient chargées ici
            };
        }

        private class PaymentProcessingResult
        {
            public bool Success { get; set; }
            public string TransactionId { get; set; }
            public string Message { get; set; }
        }
    }
}
