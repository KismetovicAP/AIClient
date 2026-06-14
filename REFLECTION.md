# Reflection

## 3 things AI helped me do faster

1. **Scaffolding the Clean Architecture skeleton.**
   Setting up the four-layer structure (Domain → Application → Infrastructure → Presentation) with correct dependency directions, DI registrations, and interface abstractions would have taken significant manual effort. AI generated the entire project skeleton — interfaces, entities, exceptions, settings binding, and middleware — in one pass without any trial-and-error.

2. **Mapping Anthropic SDK errors to domain exceptions.**
   The error-handling surface of the Anthropic SDK is wide: HTTP 429, HTTP 402, body-level quota strings (`insufficient_quota`, `quota_exceeded`, `rate_limit`), empty content arrays, and `StopReason == "max_tokens"`. Writing comprehensive, correct `catch` chains against an SDK I hadn't used before would have required multiple rounds of reading docs and hitting the API. AI produced them all at once.

3. **Building the Blazor component UI with real-time state.**
   The `CodeExplainer` component combines model selection, a character-counting textarea with live validation, loading states, markdown output, and a sortable request-history table — all wired up with correct Blazor binding and `StateHasChanged` calls. Building this incrementally by hand would have taken hours of UI iteration.

---

## 2 things AI got wrong

1. **Model display names were stale and inconsistent with the SDK constants.**
   `GetDisplayName()` returned `"Claude 3.5 Haiku"` while `ToAnthropicModel()` mapped to `AnthropicModels.Claude45Haiku` (Claude 4.5). The labels shown to users did not match the models being called. This is a silent user-facing lie that required manual review to catch; AI did not notice the drift between the two switch expressions it generated in the same file.

2. **`MaxTokens` defaulted to `40` in `appsettings.json`.**
   A limit of 40 tokens guarantees a truncated response for any non-trivial code explanation — the API will always return `StopReason == "max_tokens"` and the service will throw. The AI generated a syntactically valid configuration that was semantically broken. A realistic default for this use case is 1 000 – 4 000 tokens. The bug is invisible until the app is run, because nothing in the type system or build pipeline can catch a misconfigured integer.

---

## 1 thing I would architect differently

**Move request history out of the Blazor component and into an injected service.**

The current `List<ExplanationLogViewModel> logs` lives as a field on the `CodeExplainer` component. This means the history is lost on every page refresh, is invisible across browser tabs, and is untestable in isolation. Knowing what I know now, I would define an `IExplanationHistoryStore` scoped service (or singleton with a fixed ring-buffer) that the component reads from and writes to. The UI would call `historyStore.Add(entry)` and bind to `historyStore.Entries`. This keeps persistence concerns out of the Razor file, makes the store independently testable, and opens the door to durable storage (localStorage via JS interop, or a lightweight in-process DB) without touching the component at all.
