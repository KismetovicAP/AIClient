namespace AIClient.Domain.Entities;

/// <summary>
/// Represents the result of a code explanation request, including the generated explanation and status.
/// </summary>
public class CodeExplanationResponse
{
    /// <summary>Gets or sets the AI-generated explanation text.</summary>
    public string Explanation { get; set; } = string.Empty;

    /// <summary>Gets or sets the UTC timestamp when the response was produced.</summary>
    public DateTime Timestamp { get; set; }

    /// <summary>Gets the character length of <see cref="Explanation"/>.</summary>
    public int OutputLength => Explanation.Length;

    /// <summary>Gets or sets a value indicating whether the explanation was generated successfully.</summary>
    public bool IsSuccess { get; set; }

    /// <summary>Gets or sets a human-readable error message when <see cref="IsSuccess"/> is <c>false</c>.</summary>
    public string? ErrorMessage { get; set; }
}
