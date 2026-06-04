namespace AIClient.Domain.Entities;

public class CodeExplanationResponse
{
    public string Explanation { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public int OutputLength => Explanation.Length;
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
}
