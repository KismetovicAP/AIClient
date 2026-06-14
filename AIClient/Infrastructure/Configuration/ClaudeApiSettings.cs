namespace AIClient.Infrastructure.Configuration;

/// <summary>
/// Strongly-typed configuration bound from the <c>ClaudeApi</c> section of <c>appsettings.json</c>.
/// </summary>
public class ClaudeApiSettings
{
    /// <summary>The configuration section key used when binding this settings class.</summary>
    public const string SectionName = "ClaudeApi";

    /// <summary>Gets or sets the Anthropic API key. Should be supplied via user secrets or an environment variable in production.</summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>Gets or sets the default Anthropic model identifier string (unused at runtime; the <see cref="Domain.Enums.ClaudeModel"/> enum drives model selection).</summary>
    public string DefaultModel { get; set; } = string.Empty;

    /// <summary>Gets or sets the maximum number of tokens allowed in a single Claude response.</summary>
    public int MaxTokens { get; set; }

    /// <summary>Gets or sets the maximum number of characters accepted as code input before an <see cref="Domain.Exceptions.InputTooLargeException"/> is thrown.</summary>
    public int MaxInputLength { get; set; }
}
