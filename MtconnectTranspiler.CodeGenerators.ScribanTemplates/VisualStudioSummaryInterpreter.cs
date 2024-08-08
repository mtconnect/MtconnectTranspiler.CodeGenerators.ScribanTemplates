namespace MtconnectTranspiler.Sinks.ScribanTemplates
{
    /// <summary>
    /// An interpreter class for translating the Markdown defined in the XMI document's "Comments" into <c>&lt;summary /&gt;</c> blocks used in Visual Studio.
    /// </summary>
    /// <interpreter>An interpreter function</interpreter>
    /// <interpreter_input><inheritdoc cref="MarkdownInterpreter.Interpret(string)" path="/param[@name='input']"/></interpreter_input>
    public class VisualStudioSummaryInterpreter : MarkdownInterpreter
    {
        /// <inheritdoc />
        public VisualStudioSummaryInterpreter() : base()
        {
            AddInterpreter(@"(.*?)(?<block>&#10;)(.*?)", LineBreakInterpreter);
            AddInterpreter(@"(.*?)(?<block>`(?<contents>.*?)`)(.*?)", InlineCodeInterpreter);
            AddInterpreter(@"(.*?)(?<block>\*\*(?<contents>.*?)\*\*)(.*?)", BoldInterpreter);
            AddInterpreter(@"(.*?)(?<block>\{\{term\((?<contents>.*?)\)\}\})(.*?)", EmphasisInterpreter);
            AddInterpreter(@"(.*?)(?<block>\{\{termplural\((?<contents>.*?)\)\}\})(.*?)", EmphasisPluralInterpreter);
            AddInterpreter(@"(.*?)(?<block>\*(?<contents>.*?)\*)(.*?)", EmphasisPluralInterpreter);
            AddInterpreter(@"(.*?)(?<block>\$\$(?<contents>.*?)\$\$)(.*?)", EmphasisPluralInterpreter);
            AddInterpreter(@"(.*?)(?<block>\{\{newacronym\{(?<acronym1>.*?)\}\{(?<contents>.*?)\}\{(?<definition>.*?)\}\(\)\}\})(.*?)", EmphasisPluralInterpreter);
            AddInterpreter(@"(.*?)(?<block>\{\{block\((?<contents>.*?)\)\}\})(.*?)", CrefInterpreter);
            AddInterpreter(@"(.*?)(?<block>\{\{url\((?<contents>.*?)\)\}\}(.*?))(.*?)", HrefInterpreter);
            AddInterpreter(@"(.*?)(?<block>\{\{def\((?<contents>.*?)\)\}\}(.*?))(.*?)", CrefInterpreter);
            AddInterpreter(@"(.*?)(?<block>\{\{property\((?<contents>.*?)\)\}\}(.*?))(.*?)", (string s) =>
            {
                string[] parts = s.Split(',');
                if (parts.Length > 1)
                    return $"<see cref=\"{s.Replace(",", ".")}\">{parts[parts.Length - 1]} in {parts[0]}</see>";
                else
                    return $"<see cref=\"{s}\" />";
            });
            AddInterpreter(@"(.*?)(?<block>\{\{cite\((?<contents>.*?)\)\}\})(.*?)", HrefAlsoInterpreter);
            AddInterpreter(@"(.*?)(?<block>\{\{sect\((?<contents>.*?)\)\}\})(.*?)", CrefInterpreter);
            AddInterpreter(@"(.*?)(?<block>\{\{package\((?<contents>.*?)\)\}\})(.*?)", CrefInterpreter);

            AddInterpreter(@"(.*?)(?<block>\{\{newpage\(\)\}\}(?<contents>.*?))(.*?)", UnhandledInterpreter);
            AddInterpreter(@"(.*?)(?<block>\{\{appendix\((?<contents>.*?)\)\}\})(.*?)", UnhandledInterpreter);
            AddInterpreter(@"(.*?)(?<block>\{\{section\*\((?<contents>.*?)\)\}\})(.*?)", UnhandledInterpreter);
            AddInterpreter(@"(.*?)(?<block>\{\{addcontentsline(?<contents>.*?)\}\})(.*?)", UnhandledInterpreter);
            AddInterpreter(@"(.*?)(?<block>\{\{input(?<contents>.*?)\}\})(.*?)", UnhandledInterpreter);
            AddInterpreter(@"(.*?)(?<block>\{\{renewcommand(?<contents>.*?)\}\})(.*?)", UnhandledInterpreter);
        }

        /// <summary>
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter"/>
        /// </summary>
        /// <param name="input">
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter_input"/>
        /// </param>
        /// <returns><c>&lt;br /&gt;</c></returns>
        public string LineBreakInterpreter(string input)
            => "<br/>";

        /// <summary>
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter"/>
        /// </summary>
        /// <param name="input">
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter_input"/>
        /// </param>
        /// <returns><c>&lt;c /&gt;</c></returns>
        public string InlineCodeInterpreter(string input)
            => $"<c>{input}</c>";

        /// <summary>
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter"/>
        /// </summary>
        /// <param name="input">
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter_input"/>
        /// </param>
        /// <returns><c>&lt;b /&gt;</c></returns>
        public string BoldInterpreter(string input)
            => $"<b>{input}</b>";

        /// <summary>
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter"/>
        /// </summary>
        /// <param name="input">
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter_input"/>
        /// </param>
        /// <returns><c>&lt;see cref/&gt;</c></returns>
        public string CrefInterpreter(string input)
            => $"<see cref=\"{input}\">{input}</see>";

        /// <summary>
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter"/>
        /// </summary>
        /// <param name="input">
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter_input"/>
        /// </param>
        /// <returns><c>&lt;i /&gt;</c></returns>
        public string EmphasisInterpreter(string input)
            => $"<i>{input}</i>";

        /// <summary>
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter"/>
        /// </summary>
        /// <param name="input">
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter_input"/>
        /// </param>
        /// <returns><c>&lt;i /&gt;</c></returns>
        public string EmphasisPluralInterpreter(string input)
            => $"<i>{input}</i>s";

        /// <summary>
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter"/>
        /// </summary>
        /// <param name="input">
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter_input"/>
        /// </param>
        /// <returns><c>&lt;see href /&gt;</c></returns>
        public string HrefInterpreter(string input)
            => $"<see href=\"{input}\">{input}</see>";

        /// <summary>
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter"/>
        /// </summary>
        /// <param name="input">
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter_input"/>
        /// </param>
        /// <returns><c>&lt;seealso href /&gt;</c></returns>
        public string HrefAlsoInterpreter(string input)
            => $"<seealso href=\"https://www.google.com/search?q={input}&btnI=I\">{input}</seealso>";

        /// <summary>
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter"/>
        /// </summary>
        /// <param name="input">
        /// <inheritdoc cref="VisualStudioSummaryInterpreter" path="/interpreter_input"/>
        /// </param>
        /// <returns><c>&lt;!-- XML comment --&gt;</c></returns>
        public string UnhandledInterpreter(string input)
            => $"<!-- UNHANDLED MARKDOWN: {input} -->";

    }
}
