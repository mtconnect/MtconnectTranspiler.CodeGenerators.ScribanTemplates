[![Publish to NuGet.org](https://github.com/mtconnect/MtconnectTranspiler.CodeGenerators.ScribanTemplates/actions/workflows/nuget-packages.yml/badge.svg)](https://github.com/mtconnect/MtconnectTranspiler.CodeGenerators.ScribanTemplates/actions/workflows/nuget-packages.yml)
[![NuGet](https://img.shields.io/nuget/v/MtconnectTranspiler.CodeGenerators.ScribanTemplates.svg)](https://www.nuget.org/packages/MtconnectTranspiler.CodeGenerators.ScribanTemplates/)

# MtconnectTranspiler.CodeGenerators.ScribanTemplates

This repository contains the source for the `MtconnectTranspiler.CodeGenerators.ScribanTemplates` NuGet package. The library provides a reusable Scriban-based rendering layer for MTConnect Transpiler code generators. It helps generator projects turn MTConnect SysML/XMI-derived model objects into source files, documentation, configuration, or other text artifacts.

The package is not a complete command-line generator by itself. It supplies the template loading, dependency injection registration, rendering context, helper functions, and formatter extension points that downstream MTConnect generator projects can build on.

## Package Documentation

Full library usage is documented in the package README:

- [MtconnectTranspiler.CodeGenerators.ScribanTemplates/README.md](MtconnectTranspiler.CodeGenerators.ScribanTemplates/README.md)

That README is included in the NuGet package and appears on the package page.

## What This Library Provides

- Scriban template rendering through `IScribanTemplateGenerator`.
- File generation for objects that implement `IFileSource`.
- A `ScribanTemplateAttribute` for mapping model/output classes to `.scriban` templates.
- Template loading from a filesystem `Templates` directory or embedded resources.
- Dependency injection setup through `AddScribanServices`.
- Built-in template helpers for case conversion, code-safe identifiers, MTConnect lookups, and MTConnect version aliases.
- Pluggable markdown interpreters and code formatters.
- Formatter implementations for C#, C++, ES6 JavaScript, Python, and Ruby naming conventions.

## Installation

Install the package from NuGet.org:

```sh
dotnet add package MtconnectTranspiler.CodeGenerators.ScribanTemplates
```

Or add a package reference manually:

```xml
<PackageReference Include="MtconnectTranspiler.CodeGenerators.ScribanTemplates" Version="2.7.0" />
```

The package targets `netstandard2.0` and depends on `MtconnectTranspiler`, `Scriban`, `CaseExtensions`, and Microsoft Extensions dependency injection/options packages.

## Publishing

NuGet publishing is handled by [.github/workflows/nuget-packages.yml](.github/workflows/nuget-packages.yml).

The workflow:

1. Runs when a GitHub release is published or when manually started with `workflow_dispatch`.
2. Checks out the repository on `windows-latest`.
3. Installs the .NET SDK configured by the workflow.
4. Builds `MtconnectTranspiler.CodeGenerators.ScribanTemplates/MtconnectTranspiler.CodeGenerators.ScribanTemplates.csproj` in `Release`.
5. Packs the project into the `artifacts` directory.
6. Uploads the `.nupkg` as a workflow artifact.
7. Pushes `artifacts/*.nupkg` to `https://nuget.org/` using the `NUGET_TOKEN` repository secret.

To publish an official package version, update the package metadata in the project file, merge the change, and publish a GitHub release. Use the manual workflow only when a maintainer intentionally wants to republish from the selected branch/ref.

## Local Development

Restore and build the solution:

```sh
dotnet restore MtconnectTranspiler.CodeGenerators.ScribanTemplates.sln
dotnet build MtconnectTranspiler.CodeGenerators.ScribanTemplates.sln
```

Create a local NuGet package:

```sh
dotnet pack MtconnectTranspiler.CodeGenerators.ScribanTemplates/MtconnectTranspiler.CodeGenerators.ScribanTemplates.csproj -c Release -o artifacts
```

## Repository Layout

```text
.
|-- .github/workflows/nuget-packages.yml
|-- MtconnectTranspiler.CodeGenerators.ScribanTemplates.sln
|-- MtconnectTranspiler.CodeGenerators.ScribanTemplates/
|   |-- MtconnectTranspiler.CodeGenerators.ScribanTemplates.csproj
|   |-- README.md
|   |-- ScribanTemplateGenerator.cs
|   |-- IncludeSharedTemplates.cs
|   |-- ScribanServiceBuilder.cs
|   |-- Formatters/
|   `-- Attributes/
|-- CONTRIBUTING.md
|-- CODE_OF_CONDUCT.md
|-- SECURITY.md
`-- LICENSE.txt
```

## Contribution

This repository is hosted under the MTConnect Institute organization on GitHub and is primarily maintained by the package maintainer. Contributions are welcome when they support MTConnect Transpiler code-generation workflows and keep the library broadly reusable.

For contributions:

1. Open an issue or discussion first for large API changes, release/process changes, or behavior that affects downstream generator packages.
2. Keep pull requests focused and describe the generator scenario the change enables.
3. Update `MtconnectTranspiler.CodeGenerators.ScribanTemplates/README.md` when changing public APIs, template behavior, helpers, formatters, or installation guidance.
4. Include tests or a clear manual verification note for behavior changes.
5. Follow the repository [Code of Conduct](CODE_OF_CONDUCT.md) and report security concerns through the process in [SECURITY.md](SECURITY.md).

## License

This project is licensed under the Apache License 2.0. See [LICENSE.txt](LICENSE.txt) for details.
