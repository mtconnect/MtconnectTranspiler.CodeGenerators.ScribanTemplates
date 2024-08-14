using Scriban.Runtime;

namespace MtconnectTranspiler.CodeGenerators.ScribanTemplates
{
    public interface ITemplateLoaderService : ITemplateLoader
    {
        string TemplatesPath { get; set; }

        string Load(string templatePath);
    }
}
