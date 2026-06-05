namespace AIClient.Infrastructure.Configuration;

public class ClaudeApiSettings
{
    public const string SectionName = "ClaudeApi";

    public string ApiKey { get; set; } = string.Empty;
    public string DefaultModel { get; set; } = string.Empty;
    public int MaxTokens { get; set; }
    public int MaxInputLength { get; set; }
}
