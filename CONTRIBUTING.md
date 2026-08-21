# Contributing

We'd be happy to have you interested in contributing to the development of this repository!

## Setup Workflow

Requires the .NET SDK matching the project's target framework. Run `dotnet restore` to fetch dependencies, `dotnet run` to launch the dev server, `dotnet build` to compile, and `dotnet format` to apply code style fixes. A Dockerfile is also available for a containerized run.

Site identity and contact info are read from configuration (`appsettings.json`, overridable via environment variables): `SITE_URL`, `TWITTER_USERNAME`, `TELEGRAM_USERNAME`, `GITHUB_USERNAME`, `CONTACT_EMAIL`.

## Schema Workflow

- `Pages` holds the Razor Pages UI and page models, organized by feature (`Account`, `Auth`, `Tools`) with shared layouts under `Pages/Shared`
- `Models` holds the data models and the site's content/configuration
- `Data` holds the EF Core database context
- `Utils` holds shared helper logic
- `wwwroot` holds static assets (CSS, JS, images)

## Contribution Workflow

Commits follow Conventional Commits, and changes are proposed through pull requests against `main`.
