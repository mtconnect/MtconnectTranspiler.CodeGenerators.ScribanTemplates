using Scriban.Runtime;

namespace MtconnectTranspiler.CodeGenerators.ScribanTemplates
{
    /// <summary>
    /// An interface that defines the contract for a template loader service used with Scriban templates.
    /// </summary>
    /// <remarks>
    /// The <see cref="ITemplateLoaderService"/> interface extends the Scriban <see cref="ITemplateLoader"/> interface, providing additional functionality for loading templates from various sources, such as files or embedded resources. It also supports initialization with a <see cref="IScribanTemplateGenerator"/> to configure context-specific behaviors.
    /// </remarks>
    public interface ITemplateLoaderService : ITemplateLoader
    {
        /// <summary>
        /// Gets or sets the path to the directory containing the Scriban templates.
        /// </summary>
        /// <remarks>
        /// This property allows you to specify the root directory from which templates will be loaded when a template file is requested.
        /// If the file is not found in the specified path, the service will attempt to load the template from embedded resources.
        /// </remarks>
        string TemplatesPath { get; set; }

        /// <summary>
        /// Initializes the template loader with the provided <see cref="IScribanTemplateGenerator"/> instance.
        /// </summary>
        /// <param name="generator">The <see cref="IScribanTemplateGenerator"/> that will be used to configure the template context, including helper methods and other custom behaviors.</param>
        /// <remarks>
        /// This method allows the template loader to access and initialize any context-specific settings, such as global functions or variables, required for rendering templates.
        /// </remarks>
        void InitializeLoader(IScribanTemplateGenerator generator);

        /// <summary>
        /// Loads a template from the specified path as a string.
        /// </summary>
        /// <param name="templatePath">The file path or resource name of the template to load.</param>
        /// <returns>The content of the template as a string.</returns>
        /// <remarks>
        /// This method attempts to load a template from the specified path. If the file is not found, it will try to load the template from embedded resources.
        /// </remarks>
        string Load(string templatePath);
    }
}
