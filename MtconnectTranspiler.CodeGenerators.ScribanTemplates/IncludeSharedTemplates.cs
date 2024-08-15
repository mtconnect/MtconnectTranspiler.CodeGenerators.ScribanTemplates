using Scriban.Parsing;
using Scriban.Runtime;
using Scriban;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using MtconnectTranspiler.CodeGenerators.ScribanTemplates.Formatters;
using MtconnectTranspiler.Interpreters;
using Microsoft.Extensions.Logging;

namespace MtconnectTranspiler.CodeGenerators.ScribanTemplates
{
    /// <summary>
    /// Used as the <see cref="ITemplateLoader"/>.
    /// </summary>
    public sealed class IncludeSharedTemplates : ITemplateLoaderService
    {
        private readonly ILogger<ITemplateLoader> _logger;

        /// <summary>
        /// Reference to the directory containing all Scriban template files.<br/>
        /// <b>Default:</b> <c>Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates")</c>; aka "Templates" in the root of the executable.
        /// </summary>
        public string TemplatesPath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");

        /// <summary>
        /// A collection of named <see cref="MarkdownInterpreter"/> implementations that can be used within the templates.
        /// </summary>
        public Dictionary<string, MarkdownInterpreter> MarkdownInterpreters { get; } = new Dictionary<string, MarkdownInterpreter>();

        /// <summary>
        /// A collection of named <see cref="CodeFormatter"/> implementations that can be used within the templates.
        /// </summary>
        public Dictionary<string, CodeFormatter> CodeFormatters { get; } = new Dictionary<string, CodeFormatter>();

        /// <summary>
        /// The assembly to search for embedded resources. For example, your Visual Studio project.<br/>
        /// <b>Default: </b> <c>Assembly.GetExecutingAssembly()</c>
        /// </summary>
        public Assembly ResourceAssembly { get; set; } = Assembly.GetExecutingAssembly();

        /// <summary>
        /// The namespace within the assembly where the embedded resources are located. For example, the name of your Visual Studio project.<br/>
        /// <b>Default: </b> <c>MtconnectTranspiler.CodeGenerators.ScribanTemplates.EmbeddedTemplates</c>
        /// </summary>
        public string ResourceNamespace { get; set; } = "MtconnectTranspiler.CodeGenerators.ScribanTemplates.EmbeddedTemplates";

        public IncludeSharedTemplates(ILogger<ITemplateLoader> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public string GetPath(TemplateContext context, SourceSpan callerSpan, string templateName)
        {
            return Path.Combine(TemplatesPath, templateName);
        }

        /// <inheritdoc />
        public string Load(TemplateContext context, SourceSpan callerSpan, string templatePath)
            => Load(templatePath);

        public string Load(string templatePath)
        {
            _logger?.LogDebug("Loading template from path {templatePath}", templatePath);
            if (!File.Exists(templatePath) && File.Exists(Path.Combine(TemplatesPath, templatePath)))
            {
                templatePath = Path.Combine(TemplatesPath, templatePath);
            }

            if (File.Exists(templatePath))
            {
                _logger?.LogDebug("Found template file.");
                // Load template from file
                return File.ReadAllText(templatePath);
            }

            _logger?.LogDebug("Searching embedded resources for template.");
            // Attempt to load the template from embedded resources
            string resourcePath = $"{ResourceNamespace}.{templatePath.Replace(Path.DirectorySeparatorChar, '.').Replace(Path.AltDirectorySeparatorChar, '.')}";
            using (Stream stream = ResourceAssembly.GetManifestResourceStream(resourcePath))
            {
                if (stream != null)
                {
                    _logger?.LogDebug("Found template resource.");
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }

            throw new FileNotFoundException("Could not find template file or embedded resource", templatePath);
        }

        /// <inheritdoc />
        public async ValueTask<string> LoadAsync(TemplateContext context, SourceSpan callerSpan, string templatePath)
            => await Task.FromResult(Load(templatePath));

        /// <summary>
        /// Adds custom helper methods (MarkdownInterpreters, CodeFormatters, etc.) to the <see cref="TemplateContext"/>.
        /// </summary>
        /// <param name="context">The template context to which helpers will be added.</param>
        public void InitializeLoader(IScribanTemplateGenerator generator)
        {
            var scriptObject = new ScriptObject();

            // Add MTConnectHelperMethods to the context
            var mtconnectFunctions = new MTConnectHelperMethods();
            scriptObject.Import(mtconnectFunctions);

            // Add all registered MarkdownInterpreterWrappers to the context with their respective names
            foreach (var kvp in MarkdownInterpreters)
            {
                scriptObject.Add(kvp.Key, kvp.Value);
            }

            // Add all registered CodeFormatterWrappers to the context with their respective names
            foreach (var kvp in CodeFormatters)
            {
                scriptObject.Add(kvp.Key, kvp.Value);
            }

            generator.TemplateContext.PushGlobal(scriptObject);
        }
    }
}
