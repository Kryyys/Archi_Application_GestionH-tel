using System;

namespace GestionHotel.Core.Exceptions
{
    public class BusinessException : Exception
    {
        public string ErrorCode { get; }
        public object Details { get; }

        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BusinessException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public BusinessException(string message, string errorCode, object details) : base(message)
        {
            ErrorCode = errorCode;
            Details = details;
        }
    }
}