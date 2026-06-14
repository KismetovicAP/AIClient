using AIClient.Components;
using AIClient.Application.Services;
using AIClient.Domain.Interfaces;
using AIClient.Infrastructure.Configuration;
using AIClient.Infrastructure.ExternalServices;
using AIClient.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<ClaudeApiSettings>(
    builder.Configuration.GetSection(ClaudeApiSettings.SectionName));

builder.Services.AddScoped<IClaudeApiClient, ClaudeApiClient>();
builder.Services.AddScoped<ICodeExplanationService, CodeExplanationService>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
