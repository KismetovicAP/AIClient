using AIClient.Domain.Enums;

namespace AIClient.Application.Models;

public class ExplanationLogViewModel
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int InputLength { get; set; }
    public int OutputLength { get; set; }
    public long LatencyMs { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public string ModelUsed { get; set; } = string.Empty;
    public string Status => IsSuccess ? "Success" : "Error";
    public string StatusCssClass => IsSuccess ? "text-success" : "text-danger";
}
