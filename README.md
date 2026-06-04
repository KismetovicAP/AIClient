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
2. Paste your code in the input textarea
3. Click "Explain Code"
4. View the AI-generated explanation in the output panel
5. Check the logs table below for request metrics:
   - Timestamp
   - Input length (characters)
   - Output length (characters)
   - Latency (milliseconds)
   - Success/Error status
   - Error messages (if any)

## Error Handling

The application includes comprehensive error handling:

- **Input Validation**: Empty code input is prevented
- **API Errors**: HTTP errors are caught and displayed to users
- **Network Issues**: Connection problems are handled gracefully
- **JSON Parsing**: Invalid API responses are handled
- **Configuration**: Missing API key warnings
- **Logging**: All errors are logged with context

## Technologies

- **Frontend**: Blazor Server, Bootstrap 5, Bootstrap Icons
- **Backend**: ASP.NET Core 10
- **AI**: Anthropic Claude API (Claude 3.5 Sonnet)
- **Architecture**: Clean Architecture
- **Logging**: Microsoft.Extensions.Logging

## API Configuration

Default settings in `appsettings.json`:
```json
{
  "ClaudeApi": {
	"ApiUrl": "https://api.anthropic.com/v1/messages",
	"Model": "claude-3-5-sonnet-20241022",
	"MaxTokens": 4096
  }
}
```

## License

This project is provided as-is for educational and commercial use.
