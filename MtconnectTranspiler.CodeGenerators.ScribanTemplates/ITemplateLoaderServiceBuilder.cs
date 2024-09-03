using Microsoft.Extensions.DependencyInjection;
using MtconnectTranspiler.CodeGenerators.ScribanTemplates;
using MtconnectTranspiler.CodeGenerators.ScribanTemplates.Formatters;
using MtconnectTranspiler.Interpreters;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Scriban.Runtime;

namespace MtconnectTranspiler.Extensions
{
    /// <summary>
    /// A builder class for configuring and setting up the <see cref="ITemplateLoaderService"/> within the Dependency Injection (DI) container.
    /// This class allows for adding custom Markdown interpreters, code formatters, and configuring the template loading behavior.
    /// </summary>
    public class ITemplateLoaderServiceBuilder
    {
        private readonly IServiceCollection _services;
        private readonly IncludeSharedTemplates _templateLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="ITemplateLoaderServiceBuilder"/> class.
        /// </summary>
        /// <param name="services">The service collection to which the <see cref="ITemplateLoaderService"/> will be added.</param>
        public ITemplateLoaderServiceBuilder(IServiceCollection services)
        {
            _services = services;
            _templateLoader = new IncludeSharedTemplates(services.BuildServiceProvider().GetRequiredService<ILogger<ITemplateLoader>>());
        }

        /// <summary>
        /// Adds a custom <see cref="MarkdownInterpreter"/> to the template loader service.
        /// </summary>
        /// <param name="name">The name used to identify the Markdown interpreter in the templates.</param>
        /// <param name="interpreter">The <see cref="MarkdownInterpreter"/> implementation to be added.</param>
        /// <returns>The current <see cref="ITemplateLoaderServiceBuilder"/> instance for method chaining.</returns>
        public ITemplateLoaderServiceBuilder AddMarkdownInterpreter(string name, MarkdownInterpreter interpreter)
        {
            _templateLoader.MarkdownInterpreters[name] = interpreter;
            return this;
        }

        /// <summary>
        /// Adds a custom <see cref="CodeFormatter"/> to the template loader service.
        /// </summary>
        /// <param name="name">The name used to identify the code formatter in the templates.</param>
        /// <param name="formatter">The <see cref="CodeFormatter"/> implementation to be added.</param>
        /// <returns>The current <see cref="ITemplateLoaderServiceBuilder"/> instance for method chaining.</returns>
        public ITemplateLoaderServiceBuilder AddCodeFormatter(string name, CodeFormatter formatter)
        {
            _templateLoader.CodeFormatters[name] = formatter;
            return this;
        }

        /// <summary>
        /// Sets the path to the directory containing the Scriban templates.
        /// </summary>
        /// <param name="templatesPath">The file path to the templates directory.</param>
        /// <returns>The current <see cref="ITemplateLoaderServiceBuilder"/> instance for method chaining.</returns>
        public ITemplateLoaderServiceBuilder UseTemplatesPath(string templatesPath)
        {
            _templateLoader.TemplatesPath = templatesPath;
            return this;
        }

        /// <summary>
        /// Configures the resource assembly and namespace for loading embedded templates.
        /// </summary>
        /// <param name="assembly">The assembly containing the embedded templates.</param>
        /// <param name="resourceNamespace">The namespace within the assembly where the embedded templates are located.</param>
        /// <returns>The current <see cref="ITemplateLoaderServiceBuilder"/> instance for method chaining.</returns>
        public ITemplateLoaderServiceBuilder UseResourceAssembly(Assembly assembly, string resourceNamespace)
        {
            _templateLoader.ResourceAssembly = assembly;
            _templateLoader.ResourceNamespace = resourceNamespace;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="ITemplateLoaderService"/> and registers it with the service collection as a singleton.
        /// </summary>
        public void Build()
        {
            _services.AddSingleton<ITemplateLoaderService>(_templateLoader);
        }
    }
}
