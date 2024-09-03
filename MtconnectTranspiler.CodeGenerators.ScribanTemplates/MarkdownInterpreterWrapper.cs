using MtconnectTranspiler.Interpreters;
using Scriban.Runtime;
using System;

namespace MtconnectTranspiler.CodeGenerators.ScribanTemplates
{
    /// <summary>
    /// A wrapper class for the <see cref="MarkdownInterpreter"/> that exposes its methods to Scriban templates as a <see cref="ScriptObject"/>.
    /// </summary>
    /// <remarks>
    /// This wrapper provides access to the <see cref="MarkdownInterpreter"/> methods within Scriban templates, enabling the use of markdown interpretation features.
    /// Specifically, this wrapper handles different method signatures for the <c>Interpret</c> method and exposes them with unique names to avoid method overloading issues in Scriban.
    /// </remarks>
    public class MarkdownInterpreterWrapper : ScriptObject
    {
        private readonly MarkdownInterpreter _interpreter;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownInterpreterWrapper"/> class.
        /// </summary>
        /// <param name="interpreter">The <see cref="MarkdownInterpreter"/> instance to be wrapped and exposed to Scriban templates.</param>
        public MarkdownInterpreterWrapper(MarkdownInterpreter interpreter)
        {
            _interpreter = interpreter;

            // Expose different Interpret methods with unique names in the script context

            // Interprets a string using the <see cref="MarkdownInterpreter.Interpret(string)"/> method.
            // In Scriban templates, this method can be accessed as <c>interpret_string</c>.
            this.Import("interpret_string", new Func<string, string>(_interpreter.Interpret));

            // Interprets a single <see cref="OwnedComment"/> object using the <see cref="MarkdownInterpreter.Interpret(OwnedComment)"/> method.
            // In Scriban templates, this method can be accessed as <c>interpret_comment</c>.
            this.Import("interpret_comment", new Func<Xmi.OwnedComment, string>(_interpreter.Interpret));

            // Interprets an array of <see cref="OwnedComment"/> objects using the <see cref="MarkdownInterpreter.Interpret(OwnedComment[])"/> method.
            // In Scriban templates, this method can be accessed as <c>interpret_comments_array</c>.
            this.Import("interpret_comments_array", new Func<Xmi.OwnedComment[], string>(_interpreter.Interpret));
        }
    }
}
