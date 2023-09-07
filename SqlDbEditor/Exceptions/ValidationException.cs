using System;

namespace SqlDbEditor.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a validation fails.
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Gets the name of the parameter that failed validation.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class with the specified parameter name and error message.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that failed validation.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ValidationException(string parameterName, string message) : base(message)
        {
            ParameterName = parameterName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class with the specified parameter name, error message, and inner exception.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that failed validation.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ValidationException(string parameterName, string message, Exception innerException) : base(message, innerException)
        {
            ParameterName = parameterName;
        }
    }
}
