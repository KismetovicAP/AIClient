using Anthropic.SDK.Constants;

namespace AIClient.Domain.Enums;

/// <summary>
/// The Claude model to use when submitting a code-explanation request.
/// </summary>
public enum ClaudeModel
{
    /// <summary>Claude 4.5 Haiku — fastest and most cost-effective.</summary>
    Haiku,

    /// <summary>Claude 4.5 Sonnet — best balance of intelligence and speed.</summary>
    Sonnet,

    /// <summary>Claude 4.5 Opus — most powerful, suited for deep analysis.</summary>
    Opus
}

/// <summary>
/// Extension methods that map <see cref="ClaudeModel"/> values to Anthropic SDK identifiers and UI strings.
/// </summary>
public static class ClaudeModelExtensions
{
    /// <summary>
    /// Returns the Anthropic SDK model identifier string for the given <see cref="ClaudeModel"/>.
    /// </summary>
    /// <param name="model">The model enum value to convert.</param>
    /// <returns>The Anthropic model ID string (e.g. <c>claude-haiku-4-5</c>).</returns>
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

    /// <summary>
    /// Returns a user-friendly display name for the given <see cref="ClaudeModel"/>.
    /// </summary>
    /// <param name="model">The model enum value.</param>
    /// <returns>A short label suitable for UI dropdowns and log entries.</returns>
    public static string GetDisplayName(this ClaudeModel model)
    {
        return model switch
        {
            ClaudeModel.Haiku => "Claude 4.5 Haiku (Fastest & Cheapest)",
            ClaudeModel.Sonnet => "Claude 4.5 Sonnet (Most Intelligent)",
            ClaudeModel.Opus => "Claude 4.5 Opus (Most Powerful)",
            _ => "Claude 4.5 Haiku"
        };
    }

    /// <summary>
    /// Returns a one-line description of the model's capabilities, suitable for hint text.
    /// </summary>
    /// <param name="model">The model enum value.</param>
    /// <returns>A short capability description string.</returns>
    public static string GetDescription(this ClaudeModel model)
    {
        return model switch
        {
            ClaudeModel.Haiku => "Fast and cost-effective, great for simple explanations",
            ClaudeModel.Sonnet => "Most intelligent model, best for complex reasoning",
            ClaudeModel.Opus => "Most powerful model, best for deep analysis",
            _ => ""
        };
    }
}
