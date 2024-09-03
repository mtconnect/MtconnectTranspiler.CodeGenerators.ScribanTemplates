using System.Collections.Generic;
using System.Linq;

namespace MtconnectTranspiler.CodeGenerators.ScribanTemplates.Formatters
{
    /// <summary>
    /// A code formatter implementation for Ruby.
    /// </summary>
    public class RubyCodeFormatter : CodeFormatter
    {
        /// <inheritdoc />
        protected override HashSet<char> InvalidCharacters => base.InvalidCharacters;
        // You can extend or override invalid characters specific to Ruby here if needed

        /// <summary>
        /// Replaces invalid characters and converts to PascalCase.<br/>
        /// <b>Example:</b> <c>ClassName</c>
        /// </summary>
        /// <param name="className">Class name</param>
        /// <returns>PascalCase string with no invalid characters.</returns>
        public override string FormatClassName(string className)
            => ReplaceInvalidCharacters(ToPascalCase(className));

        /// <summary>
        /// Replaces invalid characters and converts to snake_case.<br/>
        /// <b>Example:</b> <c>property_name</c>
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>snake_case string with no invalid characters.</returns>
        public override string FormatPublicPropertyName(string propertyName)
            => ReplaceInvalidCharacters(ToSnakeCase(propertyName));

        /// <summary>
        /// Replaces invalid characters and converts to snake_case with an '@' prefix.<br/>
        /// <b>Example:</b> <c>@property_name</c>
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>snake_case string with '@' prefix and no invalid characters.</returns>
        public override string FormatPrivatePropertyName(string propertyName)
            => ReplaceInvalidCharacters($"@{ToSnakeCase(propertyName)}");

        /// <summary>
        /// Replaces invalid characters and converts to snake_case.<br/>
        /// <b>Example:</b> <c>method_name</c>
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <returns>snake_case string with no invalid characters.</returns>
        public override string FormatPublicMethodName(string methodName)
            => ReplaceInvalidCharacters(ToSnakeCase(methodName));

        /// <summary>
        /// Replaces invalid characters and converts to snake_case with an '@' prefix.<br/>
        /// <b>Example:</b> <c>@method_name</c>
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <returns>snake_case string with '@' prefix and no invalid characters.</returns>
        public override string FormatPrivateMethodName(string methodName)
            => ReplaceInvalidCharacters($"@{ToSnakeCase(methodName)}");

        /// <summary>
        /// Replaces invalid characters and converts to snake_case.<br/>
        /// <b>Example:</b> <c>variable_name</c>
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <returns>snake_case string with no invalid characters.</returns>
        public override string FormatPublicFieldName(string fieldName)
            => ReplaceInvalidCharacters(ToSnakeCase(fieldName));

        /// <summary>
        /// Replaces invalid characters and converts to snake_case with an '@' prefix.<br/>
        /// <b>Example:</b> <c>@variable_name</c>
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <returns>snake_case string with '@' prefix and no invalid characters.</returns>
        public override string FormatPrivateFieldName(string fieldName)
            => ReplaceInvalidCharacters($"@{ToSnakeCase(fieldName)}");

        /// <summary>
        /// Replaces invalid characters and converts to UPPER_SNAKE_CASE.<br/>
        /// <b>Example:</b> <c>CONSTANT_NAME</c>
        /// </summary>
        /// <param name="constantName">Constant name</param>
        /// <returns>UPPER_SNAKE_CASE string with no invalid characters.</returns>
        public override string FormatConstantName(string constantName)
            => ReplaceInvalidCharacters(ToUpperSnakeCase(constantName));

        /// <summary>
        /// Replaces invalid characters and converts to PascalCase.<br/>
        /// <b>Example:</b> <c>ModuleName</c>
        /// </summary>
        /// <param name="interfaceName">Interface name (treated like a module name in Ruby)</param>
        /// <returns>PascalCase string with no invalid characters.</returns>
        public override string FormatInterfaceName(string interfaceName)
            => ReplaceInvalidCharacters(ToPascalCase(interfaceName));

        /// <summary>
        /// Replaces invalid characters and converts to PascalCase.<br/>
        /// <b>Example:</b> <c>EnumName</c>
        /// </summary>
        /// <param name="enumName">Enum name</param>
        /// <returns>PascalCase string with no invalid characters.</returns>
        public override string FormatEnumName(string enumName)
            => ReplaceInvalidCharacters(ToPascalCase(enumName));

        /// <summary>
        /// Replaces invalid characters and converts to PascalCase.<br/>
        /// <b>Example:</b> <c>EnumMemberName</c>
        /// </summary>
        /// <param name="enumMemberName">Enum member name</param>
        /// <returns>PascalCase string with no invalid characters.</returns>
        public override string FormatEnumMemberName(string enumMemberName)
            => ReplaceInvalidCharacters(ToPascalCase(enumMemberName));
    }
}
