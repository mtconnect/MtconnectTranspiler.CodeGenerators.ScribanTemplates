using MtconnectTranspiler.Interpreters;
using Scriban.Runtime;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MtconnectTranspiler.CodeGenerators.ScribanTemplates
{
    /// <summary>
    /// Collection of helper methods that can be used by the Scriban template engine.
    /// </summary>
    public partial class ScribanHelperMethods : ScriptObject
    {
        /// <summary>
        /// The default replacement character for converting invalid code characters.
        /// </summary>
        public const string DEFAULT_CODE_SAFE_REPLACEMENT = "_";

        /// <summary>
        /// The default <see cref="MarkdownInterpreter"/> used to convert markdown from XMI comments. Default is the <see cref="VisualStudioSummaryInterpreter"/>.
        /// </summary>
        public static MarkdownInterpreter DefaultMarkdownInterpreter { get; set; } = new PlainTextInterpreter();

        /// <summary>
        /// Converts Markdown formatted text using the <see cref="DefaultMarkdownInterpreter"/>.
        /// </summary>
        /// <param name="markdown">Markdown text</param>
        /// <returns><c>&lt;summary /&gt; formatted text.</c></returns>
        public static string ToSummary(string markdown)
        {
            if (string.IsNullOrEmpty(markdown)) return markdown;

            return DefaultMarkdownInterpreter.Interpret(markdown);
        }

        /// <summary>
        /// Array of invalid characters for C# types. Initialized with those returned from <see cref="System.IO.Path.GetInvalidFileNameChars"/>.
        /// </summary>
        public static char[] InvalidCharacters { get; set; } = System.IO.Path
            .GetInvalidFileNameChars()
            .Concat(new char[] { ' ' })
            .ToArray();

        /// <summary>
        /// Regular expression to replace the <see cref="InvalidCharacters"/>
        /// </summary>
        public static Regex ReplaceInvalidChars { get; set; } = new Regex(@"\" + String.Join(@"|\", InvalidCharacters), RegexOptions.Compiled);

        /// <summary>
        /// Replaces invalid filename characters with the <paramref name="replaceBy"/> character
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="replaceBy">Character to replace invalid characters</param>
        /// <returns>String considered to be code-safe</returns>
        public static string ToCodeSafe(string input, string replaceBy = DEFAULT_CODE_SAFE_REPLACEMENT) => ReplaceInvalidChars.Replace(input, replaceBy);

        /// <inheritdoc cref="string.ToUpper()"/>
        public static string ToUpperCase(string input) => input.ToUpper();

        /// <inheritdoc cref="string.ToLower()"/>
        public static string ToLowerCase(string input) => input.ToLower();

        /// <summary></summary>
        /// <param name="input"></param>
        /// <returns><c>"The Quick Brown Fox"</c> => <c>"TheQuickBrownFox"</c></returns>
        public static string ToPascalCase(string input) => CaseExtensions.StringExtensions.ToPascalCase(input);

        /// <inheritdoc cref="ToPascalCase(string)" />
        /// <remarks><inheritdoc cref="ToCodeSafe(string, string)" path="/summary"/></remarks>
        public static string ToPascalCode(string input, string replaceBy = DEFAULT_CODE_SAFE_REPLACEMENT) => ToCodeSafe(ToPascalCase(input), replaceBy);

        /// <summary></summary>
        /// <param name="input"></param>
        /// <returns><c>"The Quick Brown Fox"</c> => <c>"theQuickBrownFox"</c></returns>
        public static string ToCamelCase(string input) => CaseExtensions.StringExtensions.ToCamelCase(input);

        /// <inheritdoc cref="ToCamelCase(string)" />
        /// <remarks><inheritdoc cref="ToCodeSafe(string, string)" path="/summary"/></remarks>
        public static string ToCamelCode(string input, string replaceBy = DEFAULT_CODE_SAFE_REPLACEMENT) => ToCodeSafe(ToCamelCase(input), replaceBy);

        /// <summary></summary>
        /// <param name="input"></param>
        /// <returns><c>"The Quick Brown Fox"</c> => <c>"the_quick_brown_fox"</c></returns>
        public static string ToSnakeCase(string input)
        {
            const string MTConnect = "MTConnect";
            if (string.IsNullOrEmpty(input)) return input;

            var sb = new StringBuilder();

            int startIndex = 1;
            if (input.StartsWith(MTConnect, StringComparison.OrdinalIgnoreCase))
            {
                startIndex = MTConnect.Length;
                sb.Append(MTConnect);
            }
            else
            {
                sb.Append(char.ToLower(input[0]));
            }

            for (var i = startIndex; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                {
                    if (i > 1 && !char.IsUpper(input[i - 1]))
                    {
                        sb.Append("_");
                    }
                    else if (i < input.Length - 1 && !char.IsUpper(input[i + 1]))
                    {
                        sb.Append("_");
                    }
                }
                sb.Append(char.ToLower(input[i]));
            }

            return sb.ToString();
        }

        /// <inheritdoc cref="ToSnakeCase(string)" />
        /// <remarks><inheritdoc cref="ToCodeSafe(string, string)" path="/summary"/></remarks>
        public static string ToSnakeCode(string input, string replaceBy = DEFAULT_CODE_SAFE_REPLACEMENT) => ToCodeSafe(ToSnakeCase(input), replaceBy);

        /// <inheritdoc cref="ToSnakeCode(string, string)" />
        /// <remarks><inheritdoc cref="ToCodeSafe(string, string)" path="/summary"/> and <inheritdoc cref="ToUpperCase(string)" path="/summary"/> </remarks>
        public static string ToUpperSnakeCode(string input, string replaceBy = DEFAULT_CODE_SAFE_REPLACEMENT) => ToSnakeCode(input, replaceBy).ToUpper();

        /// <inheritdoc cref="ToSnakeCode(string, string)" />
        /// <remarks><inheritdoc cref="ToCodeSafe(string, string)" path="/summary"/> and <inheritdoc cref="ToLowerCase(string)" path="/summary"/> </remarks>
        public static string ToLowerSnakeCode(string input, string replaceBy = DEFAULT_CODE_SAFE_REPLACEMENT) => ToSnakeCode(input, replaceBy).ToLower();

        /// <summary></summary>
        /// <param name="input"></param>
        /// <returns><c>"The Quick Brown Fox"</c> => <c>"the-quick-brown-fox"</c></returns>
        public static string ToKebabCase(string input) => CaseExtensions.StringExtensions.ToKebabCase(input);

        /// <inheritdoc cref="ToKebabCase(string)" />
        /// <remarks><inheritdoc cref="ToCodeSafe(string, string)" path="/summary"/></remarks>
        public static string ToKebabCode(string input, string replaceBy = DEFAULT_CODE_SAFE_REPLACEMENT) => ToCodeSafe(ToKebabCase(input), replaceBy);

        /// <inheritdoc cref="ToKebabCode(string, string)" />
        /// <remarks><inheritdoc cref="ToCodeSafe(string, string)" path="/summary"/> and <inheritdoc cref="ToUpperCase(string)" path="/summary"/> </remarks>
        public static string ToUpperKebabCode(string input, string replaceBy = DEFAULT_CODE_SAFE_REPLACEMENT) => ToKebabCode(input, replaceBy).ToUpper();

        /// <inheritdoc cref="ToKebabCode(string, string)" />
        /// <remarks><inheritdoc cref="ToCodeSafe(string, string)" path="/summary"/> and <inheritdoc cref="ToLowerCase(string)" path="/summary"/> </remarks>
        public static string ToLowerKebabCode(string input, string replaceBy = DEFAULT_CODE_SAFE_REPLACEMENT) => ToKebabCode(input, replaceBy).ToLower();

        /// <summary></summary>
        /// <param name="input"></param>
        /// <returns><c>"The Quick Brown Fox"</c> => <c>"The-Quick-Brown-Fox"</c></returns>
        public static string ToTrainCase(string input) => CaseExtensions.StringExtensions.ToTrainCase(input);

        /// <inheritdoc cref="ToTrainCase(string)" />
        /// <remarks><inheritdoc cref="ToCodeSafe(string, string)" path="/summary"/></remarks>
        public static string ToTrainCode(string input, string replaceBy = DEFAULT_CODE_SAFE_REPLACEMENT) => ToCodeSafe(ToTrainCase(input), replaceBy);

        /// <inheritdoc cref="ToTrainCode(string, string)" />
        /// <remarks><inheritdoc cref="ToCodeSafe(string, string)" path="/summary"/> and <inheritdoc cref="ToUpperCase(string)" path="/summary"/> </remarks>
        public static string ToUpperTrainCode(string input, string replaceBy = DEFAULT_CODE_SAFE_REPLACEMENT) => ToTrainCode(input, replaceBy).ToUpper();

        /// <inheritdoc cref="ToTrainCode(string, string)" />
        /// <remarks><inheritdoc cref="ToCodeSafe(string, string)" path="/summary"/> and <inheritdoc cref="ToLowerCase(string)" path="/summary"/> </remarks>
        public static string ToLowerTrainCode(string input, string replaceBy = DEFAULT_CODE_SAFE_REPLACEMENT) => ToTrainCode(input, replaceBy).ToLower();

    }
}
