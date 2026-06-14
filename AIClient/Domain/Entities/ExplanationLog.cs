namespace AIClient.Domain.Entities;

/// <summary>
/// An immutable audit record for a single code-explanation API call.
/// </summary>
public class ExplanationLog
{
    /// <summary>Gets or sets the unique identifier for this log entry.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the UTC timestamp when the request was initiated.</summary>
    public DateTime Timestamp { get; set; }

    /// <summary>Gets or sets the character count of the submitted code.</summary>
    public int InputLength { get; set; }

    /// <summary>Gets or sets the character count of the returned explanation.</summary>
    public int OutputLength { get; set; }

    /// <summary>Gets or sets the total round-trip latency in milliseconds.</summary>
    public long LatencyMs { get; set; }

    /// <summary>Gets or sets a value indicating whether the API call succeeded.</summary>
    public bool IsSuccess { get; set; }

    /// <summary>Gets or sets a human-readable error message when <see cref="IsSuccess"/> is <c>false</c>.</summary>
    public string? ErrorMessage { get; set; }
}
