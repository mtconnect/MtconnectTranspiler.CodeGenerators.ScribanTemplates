using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MtconnectTranspiler.Sinks.ScribanTemplates
{
    /// <summary>
    /// An interpreter class for translating the Markdown defined in the XMI document's "Comments" into other documentation forms.
    /// </summary>
    public abstract class MarkdownInterpreter
    {
        /// <summary>
        /// An internal collection of Regex-based interpreters.
        /// </summary>
        protected readonly Dictionary<Regex, Func<string, string>> interpreters = new Dictionary<Regex, Func<string, string>>();

        /// <summary>
        /// Constructs a new instance of a Markdown interpreter
        /// </summary>
        public MarkdownInterpreter() { }

        /// <summary>
        /// Adds a new interpreter for string processing. Note: The default string replacement processes <c>block</c> and <c>contents</c> groupings by replacing instances of <c>block</c> with <c>contents</c>.
        /// </summary>
        /// <param name="regex">The regex used to determine if this interpreter applies to the input string.</param>
        /// <param name="interpreter">A function that will translate the input text into the expected format.</param>
        public void AddInterpreter(Regex regex, Func<string, string> interpreter)
        {
            interpreters.Add(regex, interpreter);
        }
        /// <summary>
        /// <inheritdoc cref="AddInterpreter(Regex, Func{string, string})" path="/summary"/>
        /// </summary>
        /// <param name="expression"><inheritdoc cref="AddInterpreter(Regex, Func{string, string})" path="/param[@name='regex']"/></param>
        /// <param name="interpreter"><inheritdoc cref="AddInterpreter(Regex, Func{string, string})" path="/param[@name='interpreter']"/></param>
        public void AddInterpreter(string expression, Func<string, string> interpreter)
            => AddInterpreter(new Regex(expression), interpreter);

        /// <summary>
        /// Evaluates the input text against all interpreters and performs text replacement when matches are found.
        /// </summary>
        /// <param name="input">The markdown from the XMI comments.</param>
        /// <returns>Interpreted string</returns>
        public virtual string Interpret(string input)
        {
            StringBuilder output = new StringBuilder(input);
            foreach (var interpreter in interpreters)
            {
                var regex = interpreter.Key;
                var matches = regex.Matches(input);
                foreach (Match match in matches)
                {
                    string block = match.Groups["block"].Value;
                    string contents = match.Groups["contents"].Value;
                    output = output.Replace(block, contents);
                }
            }
            return output.ToString();
        }
    }
}
