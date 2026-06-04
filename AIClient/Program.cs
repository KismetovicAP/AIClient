using AIClient.Components;
using AIClient.Application.Services;
using AIClient.Domain.Interfaces;
using AIClient.Infrastructure.Configuration;
using AIClient.Infrastructure.ExternalServices;
using AIClient.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure Claude API settings
builder.Services.Configure<ClaudeApiSettings>(
    builder.Configuration.GetSection(ClaudeApiSettings.SectionName));

// Register Claude API client (uses official Anthropic SDK)
builder.Services.AddScoped<IClaudeApiClient, ClaudeApiClient>();

// Register application services
builder.Services.AddScoped<ICodeExplanationService, CodeExplanationService>();

var app = builder.Build();

// Add global exception handling middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
