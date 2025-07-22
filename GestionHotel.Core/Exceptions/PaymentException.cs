using System;

namespace GestionHotel.Core.Exceptions
{
    public class PaymentException : BusinessException
    {
        public PaymentException(string message) : base(message)
        {
        }

        public PaymentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PaymentException(string message, string errorCode) : base(message, errorCode)
        {
        }

        public PaymentException(string message, string errorCode, object details) : base(message, errorCode, details)
        {
        }
    }
}