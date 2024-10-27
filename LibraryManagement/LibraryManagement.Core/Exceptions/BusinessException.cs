using System;

namespace LibraryManagement.Core.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public BusinessException(string operation, string reason)
            : base($"Business rule violation in \"{operation}\": {reason}")
        {
        }
    }
}