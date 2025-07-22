using GestionHotel.Services.Interfaces;
using GestionHotel.Services.DTOs;
using System.Threading.Tasks;

namespace GestionHotel.Apis.Endpoints.Booking
{
    public class BookingHandler
    {
        private readonly IReservationService _reservationService;
        private readonly IRoomService _roomService;
        private readonly IPaymentService _paymentService;

        public BookingHandler(
            IReservationService reservationService,
            IRoomService roomService,
            IPaymentService paymentService)
        {
            _reservationService = reservationService;
            _roomService = roomService;
            _paymentService = paymentService;
        }

        public async Task<BookingResult> HandleGetAvailableRoomsAsync(GetAvailableRoomsInput input)
        {
            var request = new AvailableRoomsRequestDto
            {
                DateDebut = input.DateDebut,
                DateFin = input.DateFin,
                NombrePersonnes = input.NombrePersonnes,
                TypeChambre = input.TypeChambre,
                BudgetMax = input.BudgetMax
            };

            var availableRooms = await _roomService.GetAvailableRoomsAsync(request);
            
            return new BookingResult
            {
                Success = true,
                Message = "Chambres disponibles récupérées",
                AvailableRooms = availableRooms
            };
        }

        public async Task<BookingResult> HandleCreateBookingAsync(BookingInput input)
        {
            var createDto = new CreateReservationDto
            {
                ClientId = input.ClientId,
                DateDebut = input.DateDebut,
                DateFin = input.DateFin,
                ChambresIds = input.ChambresIds,
                Commentaires = input.Commentaires
            };

            var reservation = await _reservationService.CreateReservationAsync(createDto);
            
            return new BookingResult
            {
                Success = true,
                Message = "Réservation créée avec succès",
                ReservationId = reservation.Id,
                Reservation = reservation
            };
        }

        public async Task<BookingResult> HandleGetBookingAsync(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            
            return new BookingResult
            {
                Success = true,
                Reservation = reservation
            };
        }

        public async Task<BookingResult> HandleUpdateBookingAsync(int id, BookingInput input)
        {
            var updateDto = new UpdateReservationDto
            {
                Id = id,
                DateDebut = input.DateDebut,
                DateFin = input.DateFin,
                ChambresIds = input.ChambresIds,
                Commentaires = input.Commentaires
            };

            var reservation = await _reservationService.UpdateReservationAsync(updateDto);
            
            return new BookingResult
            {
                Success = true,
                Message = "Réservation mise à jour",
                Reservation = reservation
            };
        }

        public async Task<BookingResult> HandleCancelBookingAsync(int id, CancellationInput input)
        {
            var cancellationDto = new CancellationDto
            {
                ReservationId = id,
                Raison = input.Raison,
                ForceRemboursement = input.ForceRemboursement
            };

            var result = await _reservationService.CancelReservationAsync(cancellationDto);
            
            return new BookingResult
            {
                Success = result.Success,
                Message = result.Message,
                CancellationResult = result
            };
        }

        public async Task<BookingResult> HandlePaymentAsync(int reservationId, PaymentInput input)
        {
            var paymentDto = new CreatePaymentDto
            {
                ReservationId = reservationId,
                Montant = input.Montant,
                MethodePaiement = input.MethodePaiement,
                NumeroCarte = input.NumeroCarte,
                NomPorteur = input.NomPorteur,
                DateExpiration = input.DateExpiration,
                CodeCVV = input.CodeCVV
            };

            var paymentResult = await _paymentService.ProcessPaymentAsync(paymentDto);
            
            return new BookingResult
            {
                Success = paymentResult.Success,
                Message = paymentResult.Message,
                PaymentResult = paymentResult
            };
        }
    }
}