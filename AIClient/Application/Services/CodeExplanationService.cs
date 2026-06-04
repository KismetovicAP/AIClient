using System.Diagnostics;
using AIClient.Domain.Entities;
using AIClient.Domain.Interfaces;
using AIClient.Domain.Enums;

namespace AIClient.Application.Services;

public class CodeExplanationService : ICodeExplanationService
{
    private readonly IClaudeApiClient _claudeApiClient;
    private readonly ILogger<CodeExplanationService> _logger;

    public CodeExplanationService(
        IClaudeApiClient claudeApiClient,
        ILogger<CodeExplanationService> logger)
    {
        _claudeApiClient = claudeApiClient;
        _logger = logger;
    }

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
            {
                throw new ArgumentException("Code cannot be empty.", nameof(request));
            }

            var prompt = $"Please explain the following code in detail, including its purpose, functionality, and any important implementation details:\n\n```\n{request.Code}\n```";

            _logger.LogInformation("Sending code explanation request using model: {Model}. Input length: {InputLength}", 
                request.Model.GetDisplayName(), request.InputLength);

            var explanation = await _claudeApiClient.SendMessageAsync(prompt, request.Model, cancellationToken);

            response.Explanation = explanation;
            response.IsSuccess = true;

            _logger.LogInformation("Code explanation completed successfully. Output length: {OutputLength}, Latency: {LatencyMs}ms",
                response.OutputLength, stopwatch.ElapsedMilliseconds);
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
