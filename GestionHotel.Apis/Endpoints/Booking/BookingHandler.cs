using System.Threading.Tasks;

namespace GestionHotel.Apis.Endpoints.Booking
{
    public class BookingHandler
    {
        public BookingHandler()
        {
            // Constructeur simple
        }

        public async Task<BookingResult> HandleGetAvailableRoomsAsync(GetAvailableRoomsInput input)
        {
            await Task.Delay(100);
            
            return new BookingResult
            {
                Success = true,
                Message = "Chambres disponibles récupérées",
                AvailableRooms = new
                {
                    Rooms = new[]
                    {
                        new { Id = 1, Numero = "101", Type = "Simple", Tarif = 80.00m },
                        new { Id = 2, Numero = "102", Type = "Double", Tarif = 120.00m },
                        new { Id = 3, Numero = "201", Type = "Suite", Tarif = 200.00m }
                    },
                    DateDebut = input.DateDebut,
                    DateFin = input.DateFin,
                    NombreNuits = (input.DateFin - input.DateDebut).Days
                }
            };
        }

        public async Task<BookingResult> HandleCreateBookingAsync(BookingInput input)
        {
            await Task.Delay(100);
            
            var reservationId = new Random().Next(1000, 9999);
            
            return new BookingResult
            {
                Success = true,
                Message = "Réservation créée avec succès",
                ReservationId = reservationId,
                Reservation = new
                {
                    Id = reservationId,
                    ClientId = input.ClientId,
                    DateDebut = input.DateDebut,
                    DateFin = input.DateFin,
                    ChambresIds = input.ChambresIds,
                    Statut = "Confirmee",
                    MontantTotal = 150.00m
                }
            };
        }

        public async Task<BookingResult> HandleGetBookingAsync(int id)
        {
            await Task.Delay(100);
            
            return new BookingResult
            {
                Success = true,
                Message = "Réservation trouvée",
                Reservation = new
                {
                    Id = id,
                    ClientId = 1,
                    DateDebut = DateTime.Today.AddDays(7),
                    DateFin = DateTime.Today.AddDays(10),
                    Statut = "Confirmee",
                    MontantTotal = 150.00m
                }
            };
        }

        public async Task<BookingResult> HandleUpdateBookingAsync(int id, BookingInput input)
        {
            await Task.Delay(100);
            
            return new BookingResult
            {
                Success = true,
                Message = "Réservation mise à jour",
                Reservation = new
                {
                    Id = id,
                    ClientId = input.ClientId,
                    DateDebut = input.DateDebut,
                    DateFin = input.DateFin,
                    Statut = "Modifiee"
                }
            };
        }

        public async Task<BookingResult> HandleCancelBookingAsync(int id, CancellationInput input)
        {
            await Task.Delay(100);
            
            // Simulation règle 48h
            var hoursUntil = 72; // Simulation
            var canRefund = hoursUntil >= 48 || input.ForceRemboursement;
            
            return new BookingResult
            {
                Success = true,
                Message = canRefund ? "Annulation avec remboursement" : "Annulation sans remboursement",
                CancellationResult = new
                {
                    Success = true,
                    RemboursementApplique = canRefund,
                    MontantRembourse = canRefund ? 150.00m : 0,
                    FraisAppliques = canRefund ? 0 : 50.00m,
                    Raison = input.Raison
                }
            };
        }

        public async Task<BookingResult> HandlePaymentAsync(int reservationId, PaymentInput input)
        {
            await Task.Delay(100);
            
            // Simulation paiement 95% succès
            var success = new Random().Next(100) >= 5;
            
            return new BookingResult
            {
                Success = success,
                Message = success ? "Paiement réussi" : "Paiement échoué",
                PaymentResult = new
                {
                    Success = success,
                    TransactionId = success ? $"TXN_{DateTime.UtcNow:yyyyMMddHHmmss}" : null,
                    Montant = input.Montant,
                    MethodePaiement = input.MethodePaiement,
                    ErrorMessage = success ? null : "Fonds insuffisants"
                }
            };
        }
    }
}
