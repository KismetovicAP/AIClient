using AIClient.Domain.Enums;

namespace AIClient.Domain.Entities;

public class CodeExplanationRequest
{
    public string Code { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public ClaudeModel Model { get; set; } = ClaudeModel.Haiku;
    public int InputLength => Code.Length;
}
