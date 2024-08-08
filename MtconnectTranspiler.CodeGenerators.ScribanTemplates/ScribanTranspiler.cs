using Microsoft.Extensions.Logging;
using MtconnectTranspiler.Xmi;
using Scriban.Runtime;
using Scriban;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace MtconnectTranspiler.Sinks.ScribanTemplates
{
    public abstract class ScribanTranspiler : ITranspilerSink
    {
        /// <inheritdoc cref="ILogger"/>
        protected ILogger<ITranspilerSink> _logger;

        /// <summary>
        /// The root output directory for the transpiled code.
        /// </summary>
        public string ProjectPath { get; set; }

        private string _templatesPath { get; set; }
        /// <summary>
        /// Reference to the directory containing all Scriban template files.
        /// </summary>
        public string TemplatesPath
        {
            get
            {
                if (string.IsNullOrEmpty(_templatesPath))
                {
                    _templatesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
                }
                return _templatesPath;
            }
            set
            {
                _templatesPath = value;
                (TemplateContext.TemplateLoader as IncludeSharedTemplates).TemplatesPath = value;
            }
        }

        /// <summary>
        /// Reference to the template rendering context.
        /// </summary>
        protected TemplateContext TemplateContext { get; set; }

        /// <summary>
        /// Reference to the core template rendering model.
        /// </summary>
        protected ScriptObject Model { get; set; }

        /// <summary>
        /// Constructs a new instance of the transpiler that can transpile the model into files.
        /// </summary>
        /// <param name="projectPath"><inheritdoc cref="ProjectPath" path="/summary"/></param>
        /// <param name="logger"><inheritdoc cref="ILogger"/></param>
        public ScribanTranspiler(string projectPath, ILogger<ITranspilerSink> logger = default)
        {
            ProjectPath = projectPath;
            _logger = logger;

            TemplateContext = new TemplateContext
            {
                TemplateLoader = new IncludeSharedTemplates()
            };

            var helperFunctions = new ScribanHelperMethods();
            TemplateContext.PushGlobal(helperFunctions);

            var mtconnectFunctions = new MTConnectHelperMethods();
            TemplateContext.PushGlobal(mtconnectFunctions);


            Model = new ScriptObject();
            Model.SetValue("version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString(), true);
            TemplateContext.PushGlobal(Model);
        }

        /// <summary>
        /// <inheritdoc cref="ITranspilerSink.Transpile(XmiDocument, CancellationToken)" path="/summary"/>
        /// </summary>
        /// <param name="model"><inheritdoc cref="XmiDocument" path="/summary"/></param>
        /// <param name="cancellationToken"><inheritdoc cref="CancellationToken" path="/summary"/></param>
        public abstract void Transpile(XmiDocument model, CancellationToken cancellationToken = default);

        /// <summary>
        /// An internal cache of <see cref="Template"/>s based on the source <c>.scriban</c> file location.
        /// </summary>
        protected Dictionary<string, Template> templateCache = new Dictionary<string, Template>();
        /// <summary>
        /// Retrieves a <see cref="Template"/> from a <c>.scriban</c> file at the given <paramref name="filepath"/>.
        /// </summary>
        /// <param name="filepath">Location of the <c>.scriban</c> file to parse a <see cref="Template"/>.</param>
        /// <returns>Reference to the <see cref="Template"/> parsed from the given <c>.scriban</c> at the <paramref name="filepath"/>.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected Template GetTemplate(string filepath)
        {
            if (templateCache.TryGetValue(filepath, out Template template)) return template;

            if (!File.Exists(filepath)) throw new FileNotFoundException("Could not find template file", filepath);

            string templateContent = File.ReadAllText(filepath);
            template = Template.Parse(templateContent);

            if (template != null)
            {
                if (templateCache.ContainsKey(filepath)) return templateCache[filepath];
                _logger?.LogInformation("Registering Template from file: {Filepath}", filepath);
                templateCache.Add(filepath, template);
                return template;
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Renders the <paramref name="template"/>.
        /// </summary>
        /// <param name="member">The member name used in the <paramref name="template"/> for the provided <paramref name="value"/>.</param>
        /// <param name="value">The value for the provided <paramref name="member"/> reference.</param>
        /// <param name="template">The <see cref="Template"/> to render.</param>
        /// <returns>Rendered output.</returns>
        protected string RenderTemplateWithModel(string member, object value, Template template)
        {
            if (value == null) return String.Empty;
            if (Model.Contains(member))
            {
                Model.Remove(member);
            }
            Model.SetValue(member, value, true);
            string output = template.Render(TemplateContext);

            Model.Remove(member);

            return output;
        }

        /// <summary>
        /// Processes a collection of objects, decorated with the <see cref="ScribanTemplateAttribute"/>, into a file.
        /// </summary>
        /// <typeparam name="T">An implementation of <see cref="IFileSource"/>.</typeparam>
        /// <param name="items">Collection of objects, decorated with <see cref="ScribanTemplateAttribute"/>.</param>
        /// <param name="folderPath">Location to save the files.</param>
        /// <param name="overwriteExisting">Flag for whether or not the output file should be overwritten</param>
        protected void ProcessTemplate<T>(IEnumerable<T> items, string folderPath, bool overwriteExisting = false) where T : IFileSource
        {
            if (items == null || items.Any() == false) return;

            foreach (var item in items) ProcessTemplate(item, folderPath, overwriteExisting);
        }
        /// <summary>
        /// Processes an object, decorated with the <see cref="ScribanTemplateAttribute"/>, into a file.
        /// </summary>
        /// <typeparam name="T">An implementation of <see cref="IFileSource"/>.</typeparam>
        /// <param name="item">An object, decorated with <see cref="ScribanTemplateAttribute"/>.</param>
        /// <param name="folderPath">Location to save the file.</param>
        /// <param name="overwriteExisting">Flag for whether or not the output file should be overwritten</param>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        protected void ProcessTemplate<T>(T item, string folderPath, bool overwriteExisting = false) where T : IFileSource
        {
            if (item == null) return;

            System.Type type = typeof(T);

            ScribanTemplateAttribute attr = type.GetCustomAttribute<ScribanTemplateAttribute>()
                ?? throw new NotImplementedException("The type of " + typeof(T).Name + " must be decorated with the ScribanTemplateAttribute");

            Template template = GetTemplate(Path.Combine(TemplatesPath, attr.Filename));
            if (template == null)
            {
                var templateNotFound = new FileNotFoundException();
                _logger?.LogError(templateNotFound, "Could not find template");
                throw templateNotFound;
            }

            string filepath = Path.Combine(folderPath, item.Filename);

            string output;
            try
            {
                output = RenderTemplateWithModel("source", item, template);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to render file");
                throw ex;
            }

            if (!string.IsNullOrEmpty(output))
            {
                try
                {
                    XmiTranspilerExtensions.WriteToFile(filepath, output, overwriteExisting);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Failed to write to file: {Filepath}", filepath);
                    throw ex;
                }
            }
            else
            {
                _logger?.LogWarning("Cannot write an empty file: {Filepath}", filepath);
            }
        }

    }
}
