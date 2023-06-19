using Scriban.Parsing;
using Scriban.Runtime;
using Scriban;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MtconnectTranspiler.Sinks.ScribanTemplates
{
    /// <summary>
    /// Used as the <see cref="ITemplateLoader"/>.
    /// </summary>
    public sealed class IncludeSharedTemplates : ITemplateLoader
    {
        /// <summary>
        /// Reference to the directory containing all Scriban template files.
        /// </summary>
        public string TemplatesPath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");

        /// <inheritdoc />
        public string GetPath(TemplateContext context, SourceSpan callerSpan, string templateName)
        {
            return Path.Combine(TemplatesPath, templateName);
        }

        /// <inheritdoc />
        public string Load(TemplateContext context, SourceSpan callerSpan, string templatePath)
        {
            if (!File.Exists(templatePath)) templatePath = Path.Combine(TemplatesPath, templatePath);
            if (!File.Exists(templatePath)) throw new FileNotFoundException("Could not find template file", templatePath);

            var mtconnectFunctions = new MTConnectHelperMethods();
            context.PushGlobal(mtconnectFunctions);
            return File.ReadAllText(templatePath);
        }

        /// <inheritdoc />
        public async ValueTask<string> LoadAsync(TemplateContext context, SourceSpan callerSpan, string templatePath)
        {
            if (!File.Exists(templatePath)) templatePath = Path.Combine(TemplatesPath, templatePath);
            if (!File.Exists(templatePath)) throw new FileNotFoundException("Could not find template file", templatePath);

            var mtconnectFunctions = new MTConnectHelperMethods();
            context.PushGlobal(mtconnectFunctions);
            return await Task.FromResult(File.ReadAllText(templatePath));
        }
    }
}
