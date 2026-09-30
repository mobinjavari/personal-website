# Contributing

We'd be happy to have you interested in contributing to the development of this repository!

## Setup Workflow

Requires the .NET SDK matching the project's target framework.

| Command | Description |
|---|---|
| `dotnet restore` | Fetch dependencies |
| `dotnet run` | Launch the dev server |
| `dotnet build` | Compile the project |
| `dotnet format` | Apply code style fixes |
| `docker build -t personal-website .` | Build a container image |

## Schema Workflow

- `Pages` holds the Razor Pages UI and page models, organized by feature (`Account`, `Auth`, `Tools`) with shared layouts under `Pages/Shared`
- `Models` holds the data models and the site's content/configuration
- `Data` holds the EF Core database context
- `Endpoints` holds minimal API endpoints (e.g. SEO-related routes)
- `Utils` holds shared helper logic
- `wwwroot` holds static assets (CSS, JS, images)

## Contribution Workflow

Commits follow Conventional Commits, and changes are proposed through pull requests against `main`.
