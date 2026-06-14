namespace AIClient.Domain.Exceptions;

/// <summary>
/// Thrown when submitted code exceeds the configured maximum input character limit.
/// </summary>
public class InputTooLargeException : Exception
{
    /// <summary>Gets the actual character count of the submitted input.</summary>
    public int InputLength { get; }

    /// <summary>Gets the maximum permitted character count as configured in <c>ClaudeApiSettings.MaxInputLength</c>.</summary>
    public int MaxAllowedLength { get; }

    /// <summary>
    /// Initializes a new <see cref="InputTooLargeException"/> with the actual and maximum lengths.
    /// </summary>
    /// <param name="inputLength">The character count of the submitted code.</param>
    /// <param name="maxAllowedLength">The configured upper limit.</param>
    public InputTooLargeException(int inputLength, int maxAllowedLength)
        : base($"Input code is too large. Length: {inputLength} characters, Maximum allowed: {maxAllowedLength} characters.")
    {
        InputLength = inputLength;
        MaxAllowedLength = maxAllowedLength;
    }
}
