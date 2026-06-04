using Anthropic.SDK.Constants;

namespace AIClient.Domain.Enums;

public enum ClaudeModel
{
    Haiku,
    Sonnet,
    Opus
}

public static class ClaudeModelExtensions
{
    public static string ToAnthropicModel(this ClaudeModel model)
    {
        return model switch
        {
            ClaudeModel.Haiku => AnthropicModels.Claude45Haiku,
            ClaudeModel.Sonnet => AnthropicModels.Claude45Sonnet,
            ClaudeModel.Opus => AnthropicModels.Claude45Opus,
            _ => AnthropicModels.Claude45Haiku
        };
    }

    public static string GetDisplayName(this ClaudeModel model)
    {
        return model switch
        {
            ClaudeModel.Haiku => "Claude 3.5 Haiku (Fastest & Cheapest)",
            ClaudeModel.Sonnet => "Claude 3.5 Sonnet (Most Intelligent)",
            ClaudeModel.Opus => "Claude 3 Opus (Previous Generation)",
            _ => "Claude 3.5 Haiku"
        };
    }

    public static string GetDescription(this ClaudeModel model)
    {
        return model switch
        {
            ClaudeModel.Haiku => "Fast and cost-effective, great for simple explanations",
            ClaudeModel.Sonnet => "Most intelligent model, best for complex reasoning",
            ClaudeModel.Opus => "Previous generation, powerful but slower",
            _ => ""
        };
    }
}
