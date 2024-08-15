using Scriban;
using Scriban.Runtime;

namespace MtconnectTranspiler.CodeGenerators.ScribanTemplates
{
    public interface ITemplateLoaderService : ITemplateLoader
    {
        string TemplatesPath { get; set; }

        void InitializeLoader(IScribanTemplateGenerator generator);

        string Load(string templatePath);
    }
}
