
using CaseExtensions;
using System.Collections.Generic;
using System.Linq;

namespace MtconnectTranspiler.CodeGenerators.ScribanTemplates.Formatters
{
    /// <summary>
    /// An abstract class that outlines methods for formatting code elements according to best practices.
    /// Includes handling of invalid characters and case conversion.
    /// </summary>
    public abstract class CodeFormatter
    {
        /// <summary>
        /// A collection of invalid characters for the specific language.
        /// </summary>
        protected virtual HashSet<char> InvalidCharacters { get; } = new HashSet<char> { ' ', '-', '.', ',', ';', ':', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '+', '=', '{', '}', '[', ']', '|', '\\', '<', '>', '/', '?', '`', '~' };

        /// <summary>
        /// Formats a class name according to the best practices of the specific language.
        /// </summary>
        /// <param name="className">The class name to format.</param>
        /// <returns>The formatted class name.</returns>
        public abstract string FormatClassName(string className);

        /// <summary>
        /// Formats a public property name according to the best practices of the specific language.
        /// </summary>
        /// <param name="propertyName">The property name to format.</param>
        /// <returns>The formatted property name.</returns>
        public abstract string FormatPublicPropertyName(string propertyName);

        /// <summary>
        /// Formats a private property name according to the best practices of the specific language.
        /// </summary>
        /// <param name="propertyName">The property name to format.</param>
        /// <returns>The formatted property name.</returns>
        public abstract string FormatPrivatePropertyName(string propertyName);

        /// <summary>
        /// Formats a public method name according to the best practices of the specific language.
        /// </summary>
        /// <param name="methodName">The method name to format.</param>
        /// <returns>The formatted method name.</returns>
        public abstract string FormatPublicMethodName(string methodName);

        /// <summary>
        /// Formats a private method name according to the best practices of the specific language.
        /// </summary>
        /// <param name="methodName">The method name to format.</param>
        /// <returns>The formatted method name.</returns>
        public abstract string FormatPrivateMethodName(string methodName);

        /// <summary>
        /// Formats a public field name according to the best practices of the specific language.
        /// </summary>
        /// <param name="fieldName">The field name to format.</param>
        /// <returns>The formatted field name.</returns>
        public abstract string FormatPublicFieldName(string fieldName);

        /// <summary>
        /// Formats a private field name according to the best practices of the specific language.
        /// </summary>
        /// <param name="fieldName">The field name to format.</param>
        /// <returns>The formatted field name.</returns>
        public abstract string FormatPrivateFieldName(string fieldName);

        /// <summary>
        /// Formats a constant name according to the best practices of the specific language.
        /// </summary>
        /// <param name="constantName">The constant name to format.</param>
        /// <returns>The formatted constant name.</returns>
        public abstract string FormatConstantName(string constantName);

        /// <summary>
        /// Formats an interface name according to the best practices of the specific language.
        /// </summary>
        /// <param name="interfaceName">The interface name to format.</param>
        /// <returns>The formatted interface name.</returns>
        public abstract string FormatInterfaceName(string interfaceName);

        /// <summary>
        /// Formats an enumeration name according to the best practices of the specific language.
        /// </summary>
        /// <param name="enumName">The enumeration name to format.</param>
        /// <returns>The formatted enumeration name.</returns>
        public abstract string FormatEnumName(string enumName);

        /// <summary>
        /// Formats an enumeration member name according to the best practices of the specific language.
        /// </summary>
        /// <param name="enumMemberName">The enumeration member name to format.</param>
        /// <returns>The formatted enumeration member name.</returns>
        public abstract string FormatEnumMemberName(string enumMemberName);

        /// <summary>
        /// Replaces invalid characters in the string with safe characters.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="replacement">The character to replace invalid characters with.</param>
        /// <returns>The string with invalid characters replaced.</returns>
        protected virtual string ReplaceInvalidCharacters(string input, char replacement = '_')
        {
            return new string(input.Select(c => InvalidCharacters.Contains(c) ? replacement : c).ToArray());
        }

        /// <summary>
        /// Converts a string to PascalCase.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The PascalCase formatted string.</returns>
        protected string ToPascalCase(string input)
        {
            return input.ToPascalCase();
        }

        /// <summary>
        /// Converts a string to camelCase.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The camelCase formatted string.</returns>
        protected string ToCamelCase(string input)
        {
            return input.ToCamelCase();
        }

        /// <summary>
        /// Converts a string to snake_case.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The snake_case formatted string.</returns>
        protected string ToSnakeCase(string input)
        {
            return input.ToSnakeCase();
        }

        /// <summary>
        /// Converts a string to UPPER_SNAKE_CASE.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The UPPER_SNAKE_CASE formatted string.</returns>
        protected string ToUpperSnakeCase(string input)
        {
            return input.ToSnakeCase().ToUpperInvariant();
        }

        /// <summary>
        /// Converts a string to train-case.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The train-case formatted string.</returns>
        protected string ToTrainCase(string input)
        {
            return input.ToTrainCase();
        }

        /// <summary>
        /// Converts a string to lower train-case.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The lower train-case formatted string.</returns>
        protected string ToLowerTrainCase(string input)
        {
            return input.ToTrainCase().ToLowerInvariant();
        }
    }
}
