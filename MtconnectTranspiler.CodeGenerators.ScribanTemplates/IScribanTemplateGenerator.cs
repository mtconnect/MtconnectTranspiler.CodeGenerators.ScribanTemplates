using System.Collections.Generic;

namespace MtconnectTranspiler.CodeGenerators.ScribanTemplates
{
    public interface IScribanTemplateGenerator
    {
        void ProcessTemplate<T>(IEnumerable<T> items, string folderPath, bool overwriteExisting = false) where T : IFileSource;
        void ProcessTemplate<T>(T item, string folderPath, bool overwriteExisting = false) where T : IFileSource;
    }

}
