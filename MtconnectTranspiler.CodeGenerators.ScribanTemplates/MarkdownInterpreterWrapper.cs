using MtconnectTranspiler.Interpreters;
using Scriban;
using Scriban.Runtime;
using System;

namespace MtconnectTranspiler.CodeGenerators.ScribanTemplates
{
    public class MarkdownInterpreterWrapper : ScriptObject
    {
        private readonly MarkdownInterpreter _interpreter;

        public MarkdownInterpreterWrapper(MarkdownInterpreter interpreter)
        {
            _interpreter = interpreter;

            // Import all public methods from the interpreter to the wrapper
            this.Import("interpret", new Func<string,string>(_interpreter.Interpret));
        }

    }
}
