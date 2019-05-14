using System;

namespace DevOpen.Domain.Exceptions
{
    public class ValidationException : ArgumentException
    {
        public string Value { get; }

        public Type Type { get; }

        public ValidationException(Type type, string message, string value) : base(message)
        {
            Value = value;
            Type = type;
        }

        public ValidationException(Type type, string message, string value, string paramName) : base(message, paramName)
        {
            Value = value;
            Type = type;
        }

        public ValidationException(Type type, string message, Exception innerException) : base(message, innerException)
        {
            Type = type;
        }

        public ValidationException(Type type, string message) : base(message)
        {
            Type = type;
        }
    }
}