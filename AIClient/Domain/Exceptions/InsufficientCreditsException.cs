namespace AIClient.Domain.Exceptions;

public class InsufficientCreditsException : Exception
{
    public string ModelName { get; }

    public InsufficientCreditsException(string modelName)
        : base($"Insufficient credits or quota exceeded for model '{modelName}'. Please check your Anthropic account balance or upgrade your plan.")
    {
        ModelName = modelName;
    }

    public InsufficientCreditsException(string modelName, string apiErrorMessage)
        : base($"Insufficient credits or quota exceeded for model '{modelName}'. API Error: {apiErrorMessage}")
    {
        ModelName = modelName;
    }
}
