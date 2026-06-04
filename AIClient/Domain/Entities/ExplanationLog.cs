namespace AIClient.Domain.Entities;

public class ExplanationLog
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int InputLength { get; set; }
    public int OutputLength { get; set; }
    public long LatencyMs { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
}
