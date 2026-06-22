# MtconnectTranspiler.CodeGenerators.ScribanTemplates

`MtconnectTranspiler.CodeGenerators.ScribanTemplates` is a reusable Scriban rendering library for MTConnect Transpiler code generators. It provides the plumbing needed to load `.scriban` templates, populate a Scriban model, render MTConnect-derived objects, and write generated files.

Use this package when you are building a generator that consumes model objects from `MtconnectTranspiler` and emits text artifacts such as source code, markdown, XML, JSON, schemas, or configuration files.

## Install

```sh
dotnet add package MtconnectTranspiler.CodeGenerators.ScribanTemplates
```

Or add the package reference manually:

```xml
<PackageReference Include="MtconnectTranspiler.CodeGenerators.ScribanTemplates" Version="2.7.0" />
```

The package targets `netstandard2.0`.

## Core Concepts

The library is built around four pieces:

- `IScribanTemplateGenerator` renders templates and writes files.
- `IFileSource` marks an object as something that can become a file. The `Filename` property determines the generated file name.
- `ScribanTemplateAttribute` maps an `IFileSource` implementation to the template used to render it.
- `ITemplateLoaderService` loads templates from disk or embedded resources and registers optional helper objects.

During rendering, the current `IFileSource` item is available to the template as `source`.

## Configure Services

The dependency injection extension lives in the `MtconnectTranspiler.Extensions` namespace.

```csharp
using Microsoft.Extensions.DependencyInjection;
using MtconnectTranspiler.CodeGenerators.ScribanTemplates;
using MtconnectTranspiler.CodeGenerators.ScribanTemplates.Formatters;
using MtconnectTranspiler.Extensions;
using MtconnectTranspiler.Interpreters;

var services = new ServiceCollection();

services.AddLogging();

services.AddScribanServices(scriban =>
{
    scriban.ConfigureGenerator(options =>
    {
        options.OutputPath = "generated";
    });

    scriban.ConfigureTemplateLoader(loader =>
    {
        loader.UseTemplatesPath("Templates");
        loader.AddCodeFormatter("csharp", new CSharpCodeFormatter());
        loader.AddMarkdownInterpreter("markdown", new PlainTextInterpreter());
    });
});

using var provider = services.BuildServiceProvider();
var generator = provider.GetRequiredService<IScribanTemplateGenerator>();
```

`ConfigureTemplateLoader` is required because it registers the default `ITemplateLoaderService`. Call `services.AddLogging()` before configuring Scriban services so the loader builder can resolve its logger.

## Define a File Source

Decorate each generated file type with `ScribanTemplateAttribute`. The attribute value is the template filename or relative template path.

```csharp
using MtconnectTranspiler.CodeGenerators.ScribanTemplates;

[ScribanTemplate("class.scriban")]
public sealed class ClassFile : IFileSource
{
    public string Filename { get; set; }

    public string Name { get; set; }

    public IReadOnlyList<string> Properties { get; set; } = Array.Empty<string>();
}
```

The generator looks for the attribute on the generic type passed to `ProcessTemplate<T>`, so call it with the concrete decorated type.

## Write a Template

Create `Templates/class.scriban`:

```scriban
namespace {{ namespace }}
{
    public sealed class {{ source.name | to_pascal_code }}
    {
{{ for property in source.properties }}
        public string {{ property | to_pascal_code }} { get; set; }
{{ end }}
    }
}
```

The `source` object is the current `ClassFile`. Additional global values can be added with `UpdateModel`.

## Render Files

```csharp
var outputPath = Path.GetFullPath("generated");
Directory.CreateDirectory(outputPath);

generator.UpdateModel("namespace", "Generated.MTConnect");

var files = new[]
{
    new ClassFile
    {
        Filename = "Availability.cs",
        Name = "availability",
        Properties = new[] { "data item id", "timestamp" }
    }
};

generator.ProcessTemplate(files, outputPath, overwriteExisting: true);
```

`ProcessTemplate` has overloads for a single item and for an enumerable. It ignores `null` items and empty collections. It writes a file only when the rendered template output is not empty.

## Template Loading

`IncludeSharedTemplates` is the default loader.

It resolves templates in this order:

1. The template path exactly as requested, when it exists on disk.
2. The template path relative to `TemplatesPath`.
3. An embedded resource in `ResourceAssembly` under `ResourceNamespace`.

By default, `TemplatesPath` is `Templates` under the application base directory. You can override it:

```csharp
scriban.ConfigureTemplateLoader(loader =>
{
    loader.UseTemplatesPath(Path.Combine(AppContext.BaseDirectory, "Templates"));
});
```

To load embedded templates, configure the assembly and resource namespace:

```csharp
scriban.ConfigureTemplateLoader(loader =>
{
    loader.UseResourceAssembly(
        typeof(Program).Assembly,
        "MyGenerator.EmbeddedTemplates");
});
```

For an embedded resource named `MyGenerator.EmbeddedTemplates.class.scriban`, use `[ScribanTemplate("class.scriban")]`.

## Built-In Template Globals

The generator initializes a root Scriban model and pushes it as the global context. It includes:

- `source`: the current item being rendered, available during `ProcessTemplate`.
- `version`: the version of the `MtconnectTranspiler.CodeGenerators.ScribanTemplates` assembly.
- Any values you add with `UpdateModel(member, value)`.
- Built-in helper functions from `ScribanHelperMethods`.
- MTConnect helper functions from `MTConnectHelperMethods`.
- Registered markdown interpreters and code formatters.

## Built-In Helper Functions

The helper methods are imported into Scriban using Scriban's normal member naming, so they are available in templates as snake_case functions.

Identifier and case helpers:

```scriban
{{ "MTConnect asset changed" | to_code_safe }}
{{ "MTConnect asset changed" | to_pascal_case }}
{{ "MTConnect asset changed" | to_pascal_code }}
{{ "MTConnect asset changed" | to_camel_case }}
{{ "MTConnect asset changed" | to_camel_code }}
{{ "MTConnectAssetChanged" | to_snake_case }}
{{ "MTConnectAssetChanged" | to_snake_code }}
{{ "MTConnectAssetChanged" | to_upper_snake_code }}
{{ "MTConnectAssetChanged" | to_lower_snake_code }}
{{ "MTConnect asset changed" | to_kebab_case }}
{{ "MTConnect asset changed" | to_kebab_code }}
{{ "MTConnect asset changed" | to_upper_kebab_code }}
{{ "MTConnect asset changed" | to_lower_kebab_code }}
{{ "MTConnect asset changed" | to_train_case }}
{{ "MTConnect asset changed" | to_train_code }}
{{ "MTConnect asset changed" | to_upper_train_code }}
{{ "MTConnect asset changed" | to_lower_train_code }}
{{ "MTConnect asset changed" | to_upper_case }}
{{ "MTConnect asset changed" | to_lower_case }}
```

Markdown helper:

```scriban
{{ source.description | to_summary }}
```

MTConnect helpers:

```scriban
{{ normative = lookup_normative model source.id }}
{{ deprecated = lookup_deprecated model source.id }}
{{ version_name = lookup_mtconnect_versions "2.6" }}
```

Pass the full MTConnect `XmiDocument` or other shared model values into the template context with `UpdateModel`:

```csharp
generator.UpdateModel("model", xmiDocument);
```

## Code Formatters

Register a formatter under a template name:

```csharp
loader.AddCodeFormatter("csharp", new CSharpCodeFormatter());
loader.AddCodeFormatter("python", new PythonCodeFormatter());
loader.AddCodeFormatter("cpp", new CppCodeFormatter());
loader.AddCodeFormatter("javascript", new ES6JavaScriptFormatter());
loader.AddCodeFormatter("ruby", new RubyCodeFormatter());
```

The registered formatter exposes its public one-string methods in templates:

```scriban
{{ csharp.FormatClassName source.name }}
{{ csharp.FormatPublicPropertyName "sample property" }}
{{ python.FormatPrivateFieldName "sample field" }}
```

Built-in formatter methods include:

- `FormatClassName`
- `FormatPublicPropertyName`
- `FormatPrivatePropertyName`
- `FormatPublicMethodName`
- `FormatPrivateMethodName`
- `FormatPublicFieldName`
- `FormatPrivateFieldName`
- `FormatConstantName`
- `FormatInterfaceName`
- `FormatEnumName`
- `FormatEnumMemberName`

Create a custom formatter by deriving from `CodeFormatter`, implementing the abstract methods, and registering it with `AddCodeFormatter`.

## Markdown Interpreters

Register a markdown interpreter under a template name:

```csharp
loader.AddMarkdownInterpreter("markdown", new PlainTextInterpreter());
```

The wrapper exposes explicit methods for the supported input shapes:

```scriban
{{ markdown.interpret_string source.summary }}
{{ markdown.interpret_comment source.owned_comment }}
{{ markdown.interpret_comments_array source.owned_comments }}
```

Use this when templates need to convert MTConnect comments or other markdown-like text into the target language's documentation style.

## Overwrite Behavior

`ProcessTemplate` does not overwrite existing files unless `overwriteExisting` is `true`:

```csharp
generator.ProcessTemplate(file, outputPath, overwriteExisting: true);
```

Leave this as `false` when a generator should preserve manually edited output files.

## Common Patterns

Use `UpdateModel` for values shared by many templates:

```csharp
generator.UpdateModel("namespace", "Generated.MTConnect");
generator.UpdateModel("model", xmiDocument);
generator.UpdateModel("generated_at", DateTimeOffset.UtcNow);
```

Group generated model projections by file type:

```csharp
generator.ProcessTemplate(classes, Path.Combine(outputPath, "Models"), true);
generator.ProcessTemplate(enums, Path.Combine(outputPath, "Enums"), true);
generator.ProcessTemplate(docs, Path.Combine(outputPath, "Docs"), true);
```

Keep templates simple by preparing generator-specific view models before calling `ProcessTemplate`. The package is intentionally focused on rendering; downstream generator projects should own model traversal, filtering, sorting, and target-language decisions.

## Development

The source repository is hosted at:

https://github.com/mtconnect/MtconnectTranspiler.CodeGenerators.ScribanTemplates

Build locally:

```sh
dotnet build MtconnectTranspiler.CodeGenerators.ScribanTemplates.sln
```

Pack locally:

```sh
dotnet pack MtconnectTranspiler.CodeGenerators.ScribanTemplates/MtconnectTranspiler.CodeGenerators.ScribanTemplates.csproj -c Release -o artifacts
```

## License

This project is licensed under the Apache License 2.0.
