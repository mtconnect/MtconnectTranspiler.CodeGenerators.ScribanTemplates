using Microsoft.Extensions.DependencyInjection;
using MtconnectTranspiler.CodeGenerators.ScribanTemplates;
using System;

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
            // Create a builder to configure services
            var builder = new ScribanServiceBuilder(services);
            configure?.Invoke(builder);

            // Add IScribanTemplateGenerator to the service collection
            services.AddScoped<IScribanTemplateGenerator, ScribanTemplateGenerator>();

            return services;
        }
    }

    /// <summary>
    /// A builder class to configure Scriban services using a fluent API.
    /// This class allows configuring the template loader and generator options, and adding custom interpreters and formatters.
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
        public ScribanServiceBuilder ConfigureTemplateLoader(Action<ITemplateLoaderServiceBuilder> configure)
        {
            var templateLoaderBuilder = new ITemplateLoaderServiceBuilder(_services);
            configure(templateLoaderBuilder);
            templateLoaderBuilder.Build();
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
    }
}
