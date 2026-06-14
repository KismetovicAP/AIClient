using AIClient.Domain.Enums;

namespace AIClient.Domain.Interfaces;

/// <summary>
/// Abstraction over the Anthropic Claude messaging API.
/// </summary>
public interface IClaudeApiClient
{
    /// <summary>
    /// Sends a plain-text message to Claude using the default model and returns the response text.
    /// </summary>
    /// <param name="message">The prompt to send.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>The text content of the Claude response.</returns>
    /// <exception cref="InsufficientCreditsException">Thrown when the API returns 429 or 402.</exception>
    Task<string> SendMessageAsync(string message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a plain-text message to Claude using the specified model and returns the response text.
    /// </summary>
    /// <param name="message">The prompt to send.</param>
    /// <param name="model">The Claude model to use for this request.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>The text content of the Claude response.</returns>
    /// <exception cref="InsufficientCreditsException">Thrown when the API returns 429 or 402.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the response is empty or was truncated by the token limit.</exception>
    Task<string> SendMessageAsync(string message, ClaudeModel model, CancellationToken cancellationToken = default);
}
