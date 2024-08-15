using MtconnectTranspiler.CodeGenerators.ScribanTemplates.Formatters;
using Scriban.Runtime;
using System;

namespace MtconnectTranspiler.CodeGenerators.ScribanTemplates
{
    public class CodeFormatterWrapper : ScriptObject
    {
        private readonly CodeFormatter _formatter;

        public CodeFormatterWrapper(CodeFormatter formatter)
        {
            _formatter = formatter;

            // Import all public methods from the formatter to the wrapper
            this.Import("format_class_name", new Func<string, string>(_formatter.FormatClassName));
            this.Import("format_public_property_name", new Func<string, string>(_formatter.FormatPublicPropertyName));
        }

        // If needed, add any custom methods or logic here
    }
}
