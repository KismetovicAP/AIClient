using System.Diagnostics;
using AIClient.Domain.Entities;
using AIClient.Domain.Interfaces;
using AIClient.Domain.Enums;
using AIClient.Domain.Exceptions;
using AIClient.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace AIClient.Application.Services;

/// <summary>
/// Default implementation of <see cref="ICodeExplanationService"/> that validates input,
/// forwards the request to <see cref="IClaudeApiClient"/>, and captures structured outcomes.
/// </summary>
public class CodeExplanationService : ICodeExplanationService
{
    private readonly IClaudeApiClient _claudeApiClient;
    private readonly ILogger<CodeExplanationService> _logger;
    private readonly ClaudeApiSettings _settings;

    /// <summary>
    /// Initializes a new instance of <see cref="CodeExplanationService"/>.
    /// </summary>
    /// <param name="claudeApiClient">The Claude API client used to submit prompts.</param>
    /// <param name="logger">Logger for request diagnostics.</param>
    /// <param name="settings">Bound configuration containing token and input limits.</param>
    public CodeExplanationService(
        IClaudeApiClient claudeApiClient,
        ILogger<CodeExplanationService> logger,
        IOptions<ClaudeApiSettings> settings)
    {
        _claudeApiClient = claudeApiClient;
        _logger = logger;
        _settings = settings.Value;
    }

    /// <inheritdoc />
    public async Task<CodeExplanationResponse> ExplainCodeAsync(
        CodeExplanationRequest request,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = new CodeExplanationResponse
        {
            Timestamp = DateTime.UtcNow
        };

        try
        {
            if (string.IsNullOrWhiteSpace(request.Code))
                throw new ArgumentException("Code cannot be empty.", nameof(request));

            if (request.InputLength > _settings.MaxInputLength)
            {
                _logger.LogWarning(
                    "Input code exceeds maximum length. Input: {InputLength}, Max: {MaxLength}",
                    request.InputLength,
                    _settings.MaxInputLength);

                throw new InputTooLargeException(request.InputLength, _settings.MaxInputLength);
            }

            var prompt = $"Please explain the following code in detail, including its purpose, functionality, and any important implementation details:\n\n```\n{request.Code}\n```";

            _logger.LogInformation(
                "Sending code explanation request using model: {Model}. Input length: {InputLength}",
                request.Model.GetDisplayName(), request.InputLength);

            var explanation = await _claudeApiClient.SendMessageAsync(prompt, request.Model, cancellationToken);

            response.Explanation = explanation;
            response.IsSuccess = true;

            _logger.LogInformation(
                "Code explanation completed successfully. Output length: {OutputLength}, Latency: {LatencyMs}ms",
                response.OutputLength, stopwatch.ElapsedMilliseconds);
        }
        catch (InputTooLargeException ex)
        {
            response.IsSuccess = false;
            response.ErrorMessage = ex.Message;
            _logger.LogWarning(ex, "Input validation failed: code too large");
        }
        catch (InsufficientCreditsException ex)
        {
            response.IsSuccess = false;
            response.ErrorMessage = ex.Message;
            _logger.LogWarning(ex, "API quota exceeded for model: {Model}", request.Model.GetDisplayName());
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.ErrorMessage = ex.Message;
            _logger.LogError(ex, "Error occurred while explaining code");
        }
        finally
        {
            stopwatch.Stop();
        }

        return response;
    }
}
