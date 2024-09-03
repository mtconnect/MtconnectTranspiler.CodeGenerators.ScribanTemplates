using Scriban.Runtime;
using Scriban;
using System.Collections.Generic;

namespace MtconnectTranspiler.CodeGenerators.ScribanTemplates
{
    /// <summary>
    /// Interface for generating files using Scriban templates.
    /// </summary>
    public interface IScribanTemplateGenerator
    {
        /// <summary>
        /// Gets the directory that generated files are saved into.
        /// </summary>
        string OutputPath { get; }

        /// <summary>
        /// Gets the template context that manages the state of the template rendering, including global variables and functions.
        /// </summary>
        TemplateContext TemplateContext { get; }

        /// <summary>
        /// Gets the root model object that holds the global variables for the template rendering process.
        /// </summary>
        ScriptObject Model { get; }

        /// <summary>
        /// Processes a collection of objects, decorated with the <see cref="ScribanTemplateAttribute"/>, into files in the specified folder.
        /// </summary>
        /// <typeparam name="T">An implementation of <see cref="IFileSource"/>.</typeparam>
        /// <param name="items">The collection of objects to process, each decorated with <see cref="ScribanTemplateAttribute"/>.</param>
        /// <param name="folderPath">The folder path where the output files will be saved.</param>
        /// <param name="overwriteExisting">Flag to indicate whether to overwrite existing files. Defaults to <c>false</c>.</param>
        void ProcessTemplate<T>(IEnumerable<T> items, string folderPath, bool overwriteExisting = false) where T : IFileSource;

        /// <summary>
        /// Processes a single object, decorated with the <see cref="ScribanTemplateAttribute"/>, into a file in the specified folder.
        /// </summary>
        /// <typeparam name="T">An implementation of <see cref="IFileSource"/>.</typeparam>
        /// <param name="item">The object to process, decorated with <see cref="ScribanTemplateAttribute"/>.</param>
        /// <param name="folderPath">The folder path where the output file will be saved.</param>
        /// <param name="overwriteExisting">Flag to indicate whether to overwrite an existing file. Defaults to <c>false</c>.</param>
        void ProcessTemplate<T>(T item, string folderPath, bool overwriteExisting = false) where T : IFileSource;

        /// <summary>
        /// Updates the global model with a new member variable for use in templates.
        /// </summary>
        /// <param name="member">The name of the member to add to the model.</param>
        /// <param name="value">The value to assign to the member in the model.</param>
        void UpdateModel(string member, object value);
    }
}
