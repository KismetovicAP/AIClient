namespace AIClient.Domain.Exceptions;

/// <summary>
/// Thrown when an Anthropic API call fails due to insufficient account credits,
/// rate-limiting (HTTP 429), or payment issues (HTTP 402).
/// </summary>
public class InsufficientCreditsException : Exception
{
    /// <summary>Gets the display name of the Claude model that was being requested.</summary>
    public string ModelName { get; }

    /// <summary>
    /// Initializes a new <see cref="InsufficientCreditsException"/> with a generic credits message.
    /// </summary>
    /// <param name="modelName">The display name of the model that triggered the error.</param>
    public InsufficientCreditsException(string modelName)
        : base($"Insufficient credits or quota exceeded for model '{modelName}'. Please check your Anthropic account balance or upgrade your plan.")
    {
        ModelName = modelName;
    }

    /// <summary>
    /// Initializes a new <see cref="InsufficientCreditsException"/> including the raw API error detail.
    /// </summary>
    /// <param name="modelName">The display name of the model that triggered the error.</param>
    /// <param name="apiErrorMessage">The error message returned by the Anthropic API.</param>
    public InsufficientCreditsException(string modelName, string apiErrorMessage)
        : base($"Insufficient credits or quota exceeded for model '{modelName}'. API Error: {apiErrorMessage}")
    {
        ModelName = modelName;
    }
}
