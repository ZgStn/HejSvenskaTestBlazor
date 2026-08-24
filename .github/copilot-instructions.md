Repository: HejSvenskaTestBlazor — concise guide for Copilot sessions

Purpose
- Provide repository-specific guidance so Copilot agents can act correctly (build, run, surface important files, and respect conventions).

Build, run, test, and lint commands
- Build: dotnet build -v minimal
- Run (development): dotnet run --project HejSvenskaTestBlazor.csproj
  - From repo root (C:\Users\<you>\source\repos\HejSvenskaTestBlazor)
- Run single project explicitly: dotnet run --project ./HejSvenskaTestBlazor.csproj
- No test projects present in the repository. If tests are added, run:
  - Full suite: dotnet test <path-to-test-proj>
  - Single test example: dotnet test <test-proj> --filter "FullyQualifiedName=Namespace.ClassName.TestMethod"
- Lint/format: No project-specific linter configured. Use dotnet format (if installed): dotnet format .

High-level architecture (big picture)
- Type: ASP.NET Core + Razor Components with interactive server render mode (server-side interactive components).
- Entry point: Program.cs
  - Registers Razor Components and interactive server components
  - Adds a singleton ITopicService -> TopicService
  - Maps static assets and the Razor Components App
- UI: Components/ contains layouts and Pages/ (Topics.razor, TopicDetails.razor, etc.). Routes are defined with @page in the components.
- Data & services:
  - Data/topics.json (Data/) is the authoritative sample data used at runtime.
  - TopicService (Services/TopicService.cs) reads Data/topics.json using IWebHostEnvironment.ContentRootPath and System.Text.Json (PropertyNameCaseInsensitive = true).
  - ITopicService exposes GetTopicsAsync and GetTopicByIdAsync used by pages via DI (@inject).
- Static assets live in wwwroot/ (images, audio, bootstrap libs). JSON references to images/audio expect paths under wwwroot (e.g., /images/..., /audio/...).
- Models: Topic and WordItem in Models/ define the JSON shape.

Key repository conventions and notes
- Data is file-backed: TopicService reads Data/topics.json from the app ContentRootPath, not wwwroot. When modifying Data/topics.json, update/add static assets in wwwroot to match the paths in JSON.
- Topic Ids are used as URL segments: /topics/{id}. Ids in Data/topics.json should be URL-safe strings (lowercase, no spaces). The app assumes id is unique and stable.
- JSON files use lowercase property names in the sample (topics.json) but the JsonSerializer is configured case-insensitively — Copilot should prefer property names matching models (Id, Title, Words) when generating C# code.
- Services lifetime: TopicService is registered as singleton. It reads the file each call; avoid heavy synchronous IO or writes from multiple threads. If persisting edits later, convert to a more robust storage or add locking.
- Razor pattern: Pages use OnInitializedAsync / OnParametersSetAsync to call ITopicService and render loading/error states — follow this pattern for new pages.
- Framework target: net10.0 (see HejSvenskaTestBlazor.csproj). Keep generated code compatible with modern minimal hosting and DI patterns.

Files and areas to check when making changes (quick guide)
- Program.cs — startup registration and middleware (Antiforgery, MapStaticAssets, MapRazorComponents)
- Data/topics.json — canonical data source for topics
- Services/TopicService.cs — file I/O and JSON options
- Components/Pages/* — routing and UI for topics
- wwwroot/ — static assets referenced by Data JSON

Other AI assistant configs found
- None of: CLAUDE.md, .cursorrules, AGENTS.md, .windsurfrules, CONVENTIONS.md, AIDER_CONVENTIONS.md, .clinerules are present in this repository.

If you add CI, tests, or other tools
- Add a top-level README/CONTRIBUTING snippets for test commands and CI matrix that Copilot can reuse.

End of file
