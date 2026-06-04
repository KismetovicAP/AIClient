using AIClient.Domain.Enums;

namespace AIClient.Domain.Interfaces;

public interface IClaudeApiClient
{
    Task<string> SendMessageAsync(string message, CancellationToken cancellationToken = default);
    Task<string> SendMessageAsync(string message, ClaudeModel model, CancellationToken cancellationToken = default);
}
