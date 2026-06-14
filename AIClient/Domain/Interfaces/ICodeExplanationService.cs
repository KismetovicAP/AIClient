using AIClient.Domain.Entities;

namespace AIClient.Domain.Interfaces;

/// <summary>
/// Orchestrates validation, API communication, and logging for code explanation requests.
/// </summary>
public interface ICodeExplanationService
{
    /// <summary>
    /// Validates <paramref name="request"/>, calls the Claude API, and returns a structured response.
    /// This method never throws; errors are encoded in <see cref="CodeExplanationResponse.IsSuccess"/>
    /// and <see cref="CodeExplanationResponse.ErrorMessage"/>.
    /// </summary>
    /// <param name="request">The code snippet and model selection to explain.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="CodeExplanationResponse"/> where <see cref="CodeExplanationResponse.IsSuccess"/>
    /// is <c>true</c> on success and <c>false</c> on any validation or API error.
    /// </returns>
    Task<CodeExplanationResponse> ExplainCodeAsync(CodeExplanationRequest request, CancellationToken cancellationToken = default);
}
