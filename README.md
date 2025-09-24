# Json.Masker

> **Documentation coming soon.** This repository hosts the in-progress JSON masking utilities that back both Newtonsoft.Json and System.Text.Json pipelines.

## Packages

| Package | Description |
| --- | --- |
| `Json.Masker.Abstract` | Shared abstractions, primitives, and helpers that define the masking pipeline. |
| `Json.Masker.Newtonsoft` | Newtonsoft.Json implementation of the masking abstractions. |
| `Json.Masker.SystemTextJson` | System.Text.Json implementation of the masking abstractions. |

Each package is versioned together and published to NuGet once changes merge to `main`.

## Building locally

```bash
dotnet restore
dotnet build
dotnet test
```

> Tip: run `./install-dependencies.sh` (or the PowerShell equivalent on Windows) to install consistent tooling such as the matching .NET SDK and the `pre-commit` hooks.

## Releasing

All releases are automated through GitHub Actions:

1. Create a pull request with conventional commits.
2. Merge the PR into `main` (or trigger the **Publish & Release** workflow manually).
3. GitVersion calculates the next semantic version, the workflow packs both NuGet packages (including symbols), updates `CHANGELOG.md`, pushes a GitHub release, and optionally publishes to NuGet when the `NUGET_TOKEN` secret is configured.

Refer to [`CHANGELOG.md`](CHANGELOG.md) for the evolving feature list until fuller documentation is written.
