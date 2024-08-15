using Microsoft.Extensions.DependencyInjection;
using MtconnectTranspiler.CodeGenerators.ScribanTemplates;
using MtconnectTranspiler.CodeGenerators.ScribanTemplates.Formatters;
using MtconnectTranspiler.Interpreters;
using System;
using System.Reflection;

namespace MtconnectTranspiler.Extensions
{
    /// <summary>
    /// Extension methods for configuring Scriban services in the Dependency Injection (DI) container.
    /// </summary>
    public static class ScribanServicesExtensions
    {
        /// <summary>
        /// Adds Scriban-related services to the Dependency Injection (DI) container.
        /// </summary>
        /// <param name="services">The service collection to which the Scriban services will be added.</param>
        /// <param name="configure">An optional configuration action to customize the Scriban services.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddScribanServices(this IServiceCollection services, Action<ScribanServiceBuilder> configure = null)
        {
            // Add default services
            services.AddSingleton<ITemplateLoaderService, IncludeSharedTemplates>();
            services.AddScoped<IScribanTemplateGenerator, ScribanTemplateGenerator>();

            // Create a builder to configure services
            var builder = new ScribanServiceBuilder(services);
            configure?.Invoke(builder);

            return services;
        }
    }

    /// <summary>
    /// A builder class to configure Scriban services using a fluent API.
    /// </summary>
    public class ScribanServiceBuilder
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScribanServiceBuilder"/> class.
        /// </summary>
        /// <param name="services">The service collection being configured.</param>
        public ScribanServiceBuilder(IServiceCollection services)
        {
            _services = services;
        }

        /// <summary>
        /// Configures the template loader by applying the specified configuration action.
        /// </summary>
        /// <param name="configure">An action to configure the template loader.</param>
        /// <returns>The <see cref="ScribanServiceBuilder"/> for chaining method calls.</returns>
        public ScribanServiceBuilder ConfigureTemplateLoader(Action<IncludeSharedTemplates> configure)
        {
            _services.Configure<IncludeSharedTemplates>(options => configure(options));
            return this;
        }

        /// <summary>
        /// Configures the Scriban template generator by applying the specified configuration action.
        /// </summary>
        /// <param name="configure">An action to configure the Scriban template generator with options.</param>
        /// <returns>The <see cref="ScribanServiceBuilder"/> for chaining method calls.</returns>
        public ScribanServiceBuilder ConfigureGenerator(Action<ScribanGeneratorOptions> configure)
        {
            _services.Configure(configure);
            return this;
        }

        /// <summary>
        /// Adds a custom Markdown interpreter to the service collection.
        /// </summary>
        /// <param name="name">The name used to identify the Markdown interpreter in the template.</param>
        /// <param name="interpreter">The Markdown interpreter implementation to be added.</param>
        /// <returns>The <see cref="ScribanServiceBuilder"/> for chaining method calls.</returns>
        public ScribanServiceBuilder AddMarkdownInterpreter(string name, MarkdownInterpreter interpreter)
        {
            _services.Configure<IncludeSharedTemplates>(options =>
            {
                options.MarkdownInterpreters[name] = interpreter;
            });
            return this;
        }

        /// <summary>
        /// Adds a custom code formatter to the service collection.
        /// </summary>
        /// <param name="name">The name used to identify the code formatter in the template.</param>
        /// <param name="formatter">The code formatter implementation to be added.</param>
        /// <returns>The <see cref="ScribanServiceBuilder"/> for chaining method calls.</returns>
        public ScribanServiceBuilder AddCodeFormatter(string name, CodeFormatter formatter)
        {
            _services.Configure<IncludeSharedTemplates>(options =>
            {
                options.CodeFormatters[name] = formatter;
            });
            return this;
        }

        /// <summary>
        /// Sets the path to the directory containing the Scriban templates.
        /// </summary>
        /// <param name="templatesPath">The file path to the templates directory.</param>
        /// <returns>The <see cref="ScribanServiceBuilder"/> for chaining method calls.</returns>
        public ScribanServiceBuilder UseTemplatesPath(string templatesPath)
        {
            _services.Configure<IncludeSharedTemplates>(options => options.TemplatesPath = templatesPath);
            return this;
        }

        /// <summary>
        /// Configures the resource assembly and namespace for loading embedded templates.
        /// </summary>
        /// <param name="assembly">The assembly containing the embedded templates.</param>
        /// <param name="resourceNamespace">The namespace within the assembly where the embedded templates are located.</param>
        /// <returns>The <see cref="ScribanServiceBuilder"/> for chaining method calls.</returns>
        public ScribanServiceBuilder UseResourceAssembly(Assembly assembly, string resourceNamespace)
        {
            _services.Configure<IncludeSharedTemplates>(options =>
            {
                options.ResourceAssembly = assembly;
                options.ResourceNamespace = resourceNamespace;
            });
            return this;
        }
    }
}
