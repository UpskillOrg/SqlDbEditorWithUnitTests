namespace SqlDbEditor.Behaviors
{
    /// <summary>
    /// Represents the input modes for a text box.
    /// </summary>
    public enum TextBoxInputMode
    {
        /// <summary>
        /// No specific input mode is specified.
        /// </summary>
        None,

        /// <summary>
        /// Allows only decimal input.
        /// </summary>
        DecimalInput,

        /// <summary>
        /// Allows only digit input.
        /// </summary>
        DigitInput
    }
}