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

public class ClaudeApiClient : IClaudeApiClient
{
    private readonly AnthropicClient _client;
    private readonly ClaudeApiSettings _settings;
    private readonly ILogger<ClaudeApiClient> _logger;

    public ClaudeApiClient(
        IOptions<ClaudeApiSettings> settings,
        ILogger<ClaudeApiClient> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        if (string.IsNullOrWhiteSpace(_settings.ApiKey))
        {
            throw new InvalidOperationException("Claude API key is not configured. Please set it in appsettings.json or user secrets.");
        }

        // Initialize client following official documentation
        _client = new AnthropicClient(new APIAuthentication(_settings.ApiKey));
    }

    public async Task<string> SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        return await SendMessageAsync(message, ClaudeModel.Haiku, cancellationToken);
    }

    public async Task<string> SendMessageAsync(string message, ClaudeModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            var anthropicModel = model.ToAnthropicModel();
            _logger.LogDebug("Sending request to Claude API using model: {Model}", anthropicModel);

            // Create message following official documentation pattern
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

            // Send request to Claude API
            var messageResponse = await _client.Messages.GetClaudeMessageAsync(parameters, cancellationToken);

            if (messageResponse?.Content == null || messageResponse.Content.Count == 0)
            {
                throw new InvalidOperationException("Claude API returned an empty response");
            }

            // Extract text content from response
            var textContent = string.Join("\n", messageResponse.Content
                .OfType<TextContent>()
                .Select(block => block.Text));

            if (string.IsNullOrEmpty(textContent))
            {
                throw new InvalidOperationException("No text content found in Claude API response");
            }

            _logger.LogDebug("Successfully received response from Claude API. Length: {Length}", textContent.Length);

            return textContent;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.TooManyRequests)
        {
            // 429 Too Many Requests - Rate limit or quota exceeded
            _logger.LogWarning(ex, "Rate limit or quota exceeded for model: {Model}", model.GetDisplayName());
            throw new InsufficientCreditsException(model.GetDisplayName(), "Rate limit or quota exceeded");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.PaymentRequired)
        {
            // 402 Payment Required - Insufficient credits
            _logger.LogWarning(ex, "Insufficient credits for model: {Model}", model.GetDisplayName());
            throw new InsufficientCreditsException(model.GetDisplayName(), "Insufficient credits or payment required");
        }
        catch (Exception ex) when (ex.Message.Contains("insufficient_quota") || 
                                     ex.Message.Contains("quota_exceeded") ||
                                     ex.Message.Contains("rate_limit"))
        {
            // Check error message for quota/credit issues
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
