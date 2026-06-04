namespace AIClient.Infrastructure.Configuration;

public class ClaudeApiSettings
{
    public const string SectionName = "ClaudeApi";

    public string ApiKey { get; set; } = string.Empty;
    public string DefaultModel { get; set; } = "claude-3-5-haiku-20241022";
    public int MaxTokens { get; set; } = 4096;
}
