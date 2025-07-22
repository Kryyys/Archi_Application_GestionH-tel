using System;

namespace GestionHotel.Core.Exceptions
{
    public class ReservationException : BusinessException
    {
        public ReservationException(string message) : base(message)
        {
        }

        public ReservationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ReservationException(string message, string errorCode) : base(message, errorCode)
        {
        }

        public ReservationException(string message, string errorCode, object details) : base(message, errorCode, details)
        {
        }
    }
}