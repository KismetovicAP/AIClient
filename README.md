# AI Code Explainer

A Blazor Server application built with Clean Architecture that provides AI-powered code explanations using Anthropic's Claude API.

## Project Structure

```
AIClient/
├── Domain/                          # Core domain layer
│   ├── Entities/                    # Domain entities
│   │   ├── CodeExplanationRequest.cs
│   │   ├── CodeExplanationResponse.cs
│   │   └── ExplanationLog.cs
│   └── Interfaces/                  # Domain interfaces
│       ├── IClaudeApiClient.cs
│       └── ICodeExplanationService.cs
├── Application/                     # Application layer
│   ├── Models/                      # View models
│   │   └── ExplanationLogViewModel.cs
│   └── Services/                    # Business logic
│       └── CodeExplanationService.cs
├── Infrastructure/                  # Infrastructure layer
│   ├── Configuration/               # Configuration models
│   │   └── ClaudeApiSettings.cs
│   └── ExternalServices/            # External API clients
│       └── ClaudeApiClient.cs
└── Components/                      # Presentation layer
	├── Pages/                       # Razor pages
	│   ├── CodeExplainer.razor      # Main code explanation UI
	│   └── Home.razor               # Home page with setup instructions
	└── Layout/                      # Layout components
		├── MainLayout.razor
		└── NavMenu.razor
```

## Clean Architecture Layers

### 1. Domain Layer
- **Purpose**: Contains core business entities and interfaces
- **Dependencies**: None (innermost layer)
- **Components**:
  - `CodeExplanationRequest`: Represents a request to explain code
  - `CodeExplanationResponse`: Represents the explanation result
  - `ExplanationLog`: Represents a log entry
  - `ICodeExplanationService`: Service interface for code explanation
  - `IClaudeApiClient`: Interface for Claude API integration

### 2. Application Layer
- **Purpose**: Contains business logic and application services
- **Dependencies**: Domain layer only
- **Components**:
  - `CodeExplanationService`: Implements business logic for code explanation
  - `ExplanationLogViewModel`: View model for displaying logs

### 3. Infrastructure Layer
- **Purpose**: Implements external concerns (APIs, databases, etc.)
- **Dependencies**: Domain and Application layers
- **Components**:
  - `ClaudeApiClient`: HTTP client for Claude API
  - `ClaudeApiSettings`: Configuration model for API settings

### 4. Presentation Layer
- **Purpose**: User interface and interaction
- **Dependencies**: All other layers
- **Components**:
  - `CodeExplainer.razor`: Main UI for code explanation
  - `Home.razor`: Welcome page with setup instructions

## Features

✅ **Clean Architecture** - Separation of concerns with clear layer boundaries
✅ **Error Handling** - Comprehensive exception handling at all layers
✅ **Logging** - Detailed logging using ILogger
✅ **Request Tracking** - Real-time logs with timestamps, input/output lengths, and latency
✅ **Model Selection** - Choose between Claude Haiku, Sonnet, and Opus models
✅ **Input Validation** - Configurable character limits with real-time feedback
✅ **Quota Management** - Graceful handling of API rate limits and credit exhaustion
✅ **Markdown Rendering** - Beautiful formatted output with syntax highlighting
✅ **Responsive UI** - Modern Bootstrap-based interface
✅ **Interactive** - Blazor Server for real-time updates
✅ **Security** - Supports user secrets and environment variables for API keys

## Setup

### Prerequisites
- .NET 10 SDK
- Claude API key from [Anthropic](https://console.anthropic.com/)

### Configuration

#### Option 1: User Secrets (Recommended for Development)
```bash
cd AIClient
dotnet user-secrets init
dotnet user-secrets set "ClaudeApi:ApiKey" "your-api-key-here"
```

#### Option 2: Environment Variable (Recommended for Production)
```bash
# Windows (PowerShell)
$env:ClaudeApi__ApiKey="your-api-key-here"

# Linux/macOS
export ClaudeApi__ApiKey="your-api-key-here"
```

#### Option 3: appsettings.json (Not Recommended)
Edit `appsettings.json`:
```json
{
  "ClaudeApi": {
	"ApiKey": "your-api-key-here"
  }
}
```
⚠️ **Warning**: Never commit API keys to source control!

### Run the Application

```bash
cd AIClient
dotnet run
```

Navigate to `https://localhost:5001` (or the URL shown in the console).

## Usage

1. Navigate to the "Code Explainer" page
2. Select your preferred Claude model (Haiku, Sonnet, or Opus) from the dropdown
3. Paste your code in the input textarea (respects the configured character limit)
4. Click "Explain Code" button
5. View the beautifully formatted AI-generated explanation with:
   - Markdown rendering
   - Syntax highlighting
   - Code blocks and tables
   - Structured sections
6. Check the logs table below for request metrics:
   - Timestamp
   - Model used
   - Input length (characters)
   - Output length (characters)
   - Latency (milliseconds)
   - Success/Failed status
   - Error messages (if any)

### Real-time Input Validation

The UI provides immediate feedback:
- Character counter showing current length vs. maximum allowed
- Line counter for quick reference
- Red warning badge when limit is exceeded
- Submit button is automatically disabled for invalid input
- Alert message explaining the limit

## Error Handling

The application includes comprehensive error handling with custom exceptions:

### Input Validation
- **Empty Input**: Submit button is disabled when textarea is empty
- **Input Too Large**: `InputTooLargeException` thrown when code exceeds `MaxInputLength`
  - Validated on client-side (real-time UI feedback)
  - Validated on server-side (service layer)
  - Error message shows actual length vs. configured maximum

### API Quota & Rate Limits
- **Insufficient Credits**: `InsufficientCreditsException` thrown when:
  - API returns 429 (Too Many Requests)
  - API returns 402 (Payment Required)
  - Response contains quota/rate limit error messages
- **Network Issues**: Connection problems are handled gracefully
- **Invalid Responses**: JSON parsing errors are caught and logged

### Error Logging
- All errors are logged with full context
- Request logs show "Failed" status for any error
- Error messages are displayed in the UI
- Detailed exception information in server logs

### User-Friendly Error Messages
- "Input too large! Please reduce to X characters or less."
- "Insufficient credits or quota exceeded for model: [Model Name]"
- "Failed to communicate with Claude API. Please check your internet connection and API key."

## Technologies

- **Frontend**: Blazor Server, Bootstrap 5, Bootstrap Icons
- **Backend**: ASP.NET Core 10
- **AI**: Anthropic Claude API 3.5 (Haiku, Sonnet, Opus)
- **SDK**: Anthropic.SDK 5.10.0
- **Markdown**: Markdig for beautiful output rendering
- **Architecture**: Clean Architecture with dependency injection
- **Logging**: Microsoft.Extensions.Logging
- **Configuration**: Microsoft.Extensions.Options pattern

## API Configuration

The application uses configurable limits and settings to control API usage and input validation. All configuration is centralized in `appsettings.json` with **no hardcoded values** in the codebase.

### Configuration Settings

Edit `appsettings.json` to customize the application behavior:

```json
{
  "ClaudeApi": {
	"ApiKey": "your-api-key-here",
	"DefaultModel": "claude-3-5-haiku-20240307",
	"MaxTokens": 4096,
	"MaxInputLength": 10000
  }
}
```

### Configuration Properties

| Property | Description | Default | Notes |
|----------|-------------|---------|-------|
| `ApiKey` | Your Anthropic API key | (required) | Use user secrets or environment variables for security |
| `DefaultModel` | The Claude model to use by default | `claude-3-5-haiku-20240307` | Available models: Haiku (fastest/cheapest), Sonnet (balanced), Opus (most capable) |
| `MaxTokens` | Maximum tokens in API response | `4096` | Controls output length; higher values cost more |
| `MaxInputLength` | Maximum characters allowed in code input | `10000` | Prevents oversized requests; validated on both client and server |

### Available Claude Models

The application supports three Claude 3.5 models with different performance and cost characteristics:

| Model | Model ID | Speed | Cost | Best For |
|-------|----------|-------|------|----------|
| **Haiku** | `claude-3-5-haiku-20240307` | ⚡ Fastest | 💰 Cheapest | Quick explanations, simple code |
| **Sonnet** | `claude-3-5-sonnet-20241022` | ⚡ Fast | 💰💰 Moderate | Complex code, detailed analysis |
| **Opus** | `claude-3-opus-20240229` | ⚡ Slower | 💰💰💰 Most expensive | Advanced reasoning, critical code |

### Adjusting Limits

#### Increase Input Character Limit

To allow larger code snippets (e.g., 50,000 characters):

```json
{
  "ClaudeApi": {
	"MaxInputLength": 50000
  }
}
```

⚠️ **Note**: Larger inputs consume more tokens and may increase API costs and latency.

#### Increase Output Token Limit

To get longer explanations:

```json
{
  "ClaudeApi": {
	"MaxTokens": 8192
  }
}
```

⚠️ **Note**: Higher token limits increase API costs. Claude pricing is per-token.

#### Switch Default Model

To use a more powerful model by default:

```json
{
  "ClaudeApi": {
	"DefaultModel": "claude-3-5-sonnet-20241022"
  }
}
```

Users can still select any model from the dropdown in the UI.

### Environment-Specific Configuration

For different environments (development, staging, production), use:

- `appsettings.Development.json` - Development overrides
- `appsettings.Production.json` - Production settings
- Environment variables - Override any setting (e.g., `ClaudeApi__MaxTokens=8192`)
- User secrets - Store sensitive values like API keys

Example environment variable override:
```bash
# Windows (PowerShell)
$env:ClaudeApi__MaxInputLength=20000

# Linux/macOS
export ClaudeApi__MaxInputLength=20000
```

### Cost Optimization Tips

1. **Use Haiku for simple code** - 50x cheaper than Opus
2. **Limit MaxTokens** - Only request what you need
3. **Set appropriate MaxInputLength** - Prevents accidentally processing huge files
4. **Monitor the logs** - Track input/output lengths and latency per request

## License

This project is provided as-is for educational and commercial use.
