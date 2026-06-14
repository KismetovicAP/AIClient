using AIClient.Domain.Enums;

namespace AIClient.Domain.Entities;

/// <summary>
/// Represents a request to explain a code snippet using the Claude AI API.
/// </summary>
public class CodeExplanationRequest
{
    /// <summary>Gets or sets the source code to be explained.</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Gets or sets the UTC timestamp when the request was created.</summary>
    public DateTime Timestamp { get; set; }

    /// <summary>Gets or sets the Claude model to use for the explanation.</summary>
    public ClaudeModel Model { get; set; } = ClaudeModel.Haiku;

    /// <summary>Gets the character length of <see cref="Code"/>.</summary>
    public int InputLength => Code.Length;
}
