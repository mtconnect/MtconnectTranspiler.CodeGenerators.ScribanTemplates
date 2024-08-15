using Microsoft.Extensions.Logging;
using Scriban.Runtime;
using Scriban;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace MtconnectTranspiler.CodeGenerators.ScribanTemplates
{
    /// <summary>
    /// A service for generating files from <c>.scriban</c> templates.
    /// </summary>
    public class ScribanTemplateGenerator : IScribanTemplateGenerator
    {
        private readonly ILogger<ScribanTemplateGenerator> _logger;
        private readonly ITemplateLoaderService _templateLoaderService;
        private readonly ScribanGeneratorOptions _options;

        /// <inheritdoc />
        public string OutputPath { get; }

        private string _templatesPath;
        public string TemplatesPath
        {
            get => _templatesPath ?? (_templatesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates"));
            set
            {
                _templatesPath = value;
                _templateLoaderService.TemplatesPath = value;
            }
        }

        /// <inheritdoc />
        public TemplateContext TemplateContext { get; }
        /// <inheritdoc />
        public ScriptObject Model { get; private set; }

        public ScribanTemplateGenerator(ITemplateLoaderService templateLoaderService, IOptions<ScribanGeneratorOptions> options, ILogger<ScribanTemplateGenerator> logger = null)
        {
            _logger = logger;
            _templateLoaderService = templateLoaderService;
            _options = options.Value;

            OutputPath = _options.OutputPath;

            TemplateContext = new TemplateContext
            {
                TemplateLoader = templateLoaderService as ITemplateLoader
            };

            InitializeHelpers();
            InitializeModel();
        }

        private void InitializeHelpers()
        {
            var helperFunctions = new ScribanHelperMethods();
            TemplateContext.PushGlobal(helperFunctions);

            var mtconnectFunctions = new MTConnectHelperMethods();
            TemplateContext.PushGlobal(mtconnectFunctions);
        }

        private void InitializeModel()
        {
            Model = new ScriptObject();
            Model.SetValue("version", Assembly.GetExecutingAssembly().GetName().Version?.ToString(), true);
            TemplateContext.PushGlobal(Model);
        }

        private readonly Dictionary<string, Template> _templateCache = new Dictionary<string, Template>();

        protected Template GetTemplate(string templateName)
        {
            if (_templateCache.TryGetValue(templateName, out Template template))
                return template;

            string templateContent = _templateLoaderService.Load(templateName);
            template = Template.Parse(templateContent);

            if (template != null)
            {
                _logger?.LogInformation("Registering Template: {TemplateName}", templateName);
                _templateCache[templateName] = template;
                return template;
            }

            throw new InvalidOperationException($"Failed to parse template: {templateName}");
        }

        /// <inheritdoc />
        public void UpdateModel(string member, object value)
        {
            if (value == null) return;
            Model.SetValue(member, value, true);
        }

        protected string RenderTemplateWithModel(string member, object value, Template template)
        {
            if (value == null) return string.Empty;

            UpdateModel(member, value);
            string output = template.Render(TemplateContext);
            Model.Remove(member);

            return output;
        }

        /// <inheritdoc />
        public void ProcessTemplate<T>(IEnumerable<T> items, string folderPath, bool overwriteExisting = false) where T : IFileSource
        {
            if (items == null || !items.Any()) return;

            foreach (var item in items)
                ProcessTemplate(item, folderPath, overwriteExisting);
        }

        /// <inheritdoc />
        public void ProcessTemplate<T>(T item, string folderPath, bool overwriteExisting = false) where T : IFileSource
        {
            if (item == null) return;

            var type = typeof(T);
            var attr = type.GetCustomAttribute<ScribanTemplateAttribute>()
                ?? throw new NotImplementedException($"The type {typeof(T).Name} must be decorated with the ScribanTemplateAttribute");

            Template template = GetTemplate(attr.Filename);

            string filepath = Path.Combine(folderPath, item.Filename);

            string output;
            try
            {
                output = RenderTemplateWithModel("source", item, template);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to render file");
                throw;
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
                    throw;
                }
            }
            else
            {
                _logger?.LogWarning("Cannot write an empty file: {Filepath}", filepath);
            }
        }
    }
}
