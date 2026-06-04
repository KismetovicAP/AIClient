using AIClient.Domain.Entities;

namespace AIClient.Domain.Interfaces;

public interface ICodeExplanationService
{
    Task<CodeExplanationResponse> ExplainCodeAsync(CodeExplanationRequest request, CancellationToken cancellationToken = default);
}
