namespace AIClient.Domain.Exceptions;

public class InputTooLargeException : Exception
{
    public int InputLength { get; }
    public int MaxAllowedLength { get; }

    public InputTooLargeException(int inputLength, int maxAllowedLength)
        : base($"Input code is too large. Length: {inputLength} characters, Maximum allowed: {maxAllowedLength} characters.")
    {
        InputLength = inputLength;
        MaxAllowedLength = maxAllowedLength;
    }
}
