namespace SqlDbEditor.ViewModels
{
    /// <summary>
    /// Represents the result of an operation, such as an action or validation, indicating whether it passed, failed,
    /// or validation-specific failures occurred.
    /// </summary>
    public enum OkResult
    {
        /// <summary>
        /// Indicates that the operation passed successfully without any issues.
        /// </summary>
        Passed,

        /// <summary>
        /// Indicates that the operation failed due to an error or unexpected condition.
        /// </summary>
        Failed,

        /// <summary>
        /// Indicates that the operation failed due to validation-specific issues or constraints not being met.
        /// </summary>
        ValidationFailed
    }
}
