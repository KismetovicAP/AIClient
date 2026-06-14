using Anthropic.SDK;
using Anthropic.SDK.Messaging;
using Anthropic.SDK.Constants;
using AIClient.Domain.Interfaces;
using AIClient.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using AIClient.Domain.Enums;
using AIClient.Domain.Exceptions;
using System.Net;

namespace AIClient.Infrastructure.ExternalServices;

/// <summary>
/// Anthropic SDK implementation of <see cref="IClaudeApiClient"/>.
/// Wraps HTTP-level errors into domain exceptions so callers remain decoupled from the SDK.
/// </summary>
public class ClaudeApiClient : IClaudeApiClient
{
    private readonly AnthropicClient _client;
    private readonly ClaudeApiSettings _settings;
    private readonly ILogger<ClaudeApiClient> _logger;

    /// <summary>
    /// Initializes a new <see cref="ClaudeApiClient"/> and validates that an API key is present.
    /// </summary>
    /// <param name="settings">Bound configuration containing the API key and token limits.</param>
    /// <param name="logger">Logger for request diagnostics.</param>
    /// <exception cref="InvalidOperationException">Thrown at startup when <c>ClaudeApi:ApiKey</c> is missing.</exception>
    public ClaudeApiClient(
        IOptions<ClaudeApiSettings> settings,
        ILogger<ClaudeApiClient> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        if (string.IsNullOrWhiteSpace(_settings.ApiKey))
            throw new InvalidOperationException("Claude API key is not configured. Please set it in appsettings.json or user secrets.");

        _client = new AnthropicClient(new APIAuthentication(_settings.ApiKey));
    }

    /// <inheritdoc />
    public async Task<string> SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        return await SendMessageAsync(message, ClaudeModel.Haiku, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string> SendMessageAsync(string message, ClaudeModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            var anthropicModel = model.ToAnthropicModel();
            _logger.LogDebug("Sending request to Claude API using model: {Model}", anthropicModel);

            var messages = new List<Message>
            {
                new Message(RoleType.User, message)
            };

            var parameters = new MessageParameters
            {
                Messages = messages,
                Model = anthropicModel,
                MaxTokens = _settings.MaxTokens,
                Stream = false
            };

            var messageResponse = await _client.Messages.GetClaudeMessageAsync(parameters, cancellationToken);

            if (messageResponse?.Content == null || messageResponse.Content.Count == 0)
                throw new InvalidOperationException("Claude API returned an empty response");

            if (messageResponse.StopReason == "max_tokens")
            {
                _logger.LogWarning(
                    "Response truncated due to MaxTokens limit. MaxTokens: {MaxTokens}, Model: {Model}",
                    _settings.MaxTokens,
                    model.GetDisplayName());

                throw new InvalidOperationException(
                    $"Response exceeded the configured token limit of {_settings.MaxTokens}. " +
                    "The explanation was incomplete. Please increase 'MaxTokens' in appsettings.json or reduce the input size.");
            }

            var textContent = string.Join("\n", messageResponse.Content
                .OfType<TextContent>()
                .Select(block => block.Text));

            if (string.IsNullOrEmpty(textContent))
                throw new InvalidOperationException("No text content found in Claude API response");

            _logger.LogDebug("Successfully received response from Claude API. Length: {Length}", textContent.Length);

            return textContent;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.TooManyRequests)
        {
            _logger.LogWarning(ex, "Rate limit or quota exceeded for model: {Model}", model.GetDisplayName());
            throw new InsufficientCreditsException(model.GetDisplayName(), "Rate limit or quota exceeded");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.PaymentRequired)
        {
            _logger.LogWarning(ex, "Insufficient credits for model: {Model}", model.GetDisplayName());
            throw new InsufficientCreditsException(model.GetDisplayName(), "Insufficient credits or payment required");
        }
        catch (Exception ex) when (ex.Message.Contains("insufficient_quota") ||
                                    ex.Message.Contains("quota_exceeded") ||
                                    ex.Message.Contains("rate_limit"))
        {
            _logger.LogWarning(ex, "Quota or credit issue detected for model: {Model}", model.GetDisplayName());
            throw new InsufficientCreditsException(model.GetDisplayName(), ex.Message);
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (InsufficientCreditsException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error communicating with Claude API");
            throw new Exception("Failed to communicate with Claude API. Please check your internet connection and API key.", ex);
        }
    }
}
