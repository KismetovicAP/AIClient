using AIClient.Domain.Enums;

namespace AIClient.Application.Models;

/// <summary>
/// UI projection of <see cref="Domain.Entities.ExplanationLog"/> used in the request-history table.
/// Adds computed display properties so views remain logic-free.
/// </summary>
public class ExplanationLogViewModel
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

    /// <summary>Gets or sets a value indicating whether the request succeeded.</summary>
    public bool IsSuccess { get; set; }

    /// <summary>Gets or sets a human-readable error message when <see cref="IsSuccess"/> is <c>false</c>.</summary>
    public string? ErrorMessage { get; set; }

    /// <summary>Gets or sets the display name of the Claude model used for this request.</summary>
    public string ModelUsed { get; set; } = string.Empty;

    /// <summary>Gets <c>"Success"</c> or <c>"Error"</c> based on <see cref="IsSuccess"/>.</summary>
    public string Status => IsSuccess ? "Success" : "Error";

    /// <summary>Gets the Bootstrap text-color CSS class for the status badge.</summary>
    public string StatusCssClass => IsSuccess ? "text-success" : "text-danger";
}
