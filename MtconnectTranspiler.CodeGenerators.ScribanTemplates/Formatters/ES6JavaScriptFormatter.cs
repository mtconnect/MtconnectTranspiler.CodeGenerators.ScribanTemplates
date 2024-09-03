using System.Collections.Generic;
using System.Linq;

namespace MtconnectTranspiler.CodeGenerators.ScribanTemplates.Formatters
{
    /// <summary>
    /// A code formatter implementation for ES6 JavaScript.
    /// </summary>
    public class ES6JavaScriptFormatter : CodeFormatter
    {
        /// <inheritdoc/>
        protected override HashSet<char> InvalidCharacters => base.InvalidCharacters;
        // You can extend or override invalid characters specific to ES6 JavaScript here if needed

        /// <summary>
        /// Replaces invalid characters and converts to PascalCase.<br/>
        /// <b>Example:</b> <c>ClassName</c>
        /// </summary>
        /// <param name="className">Class name</param>
        /// <returns>PascalCase string with no invalid characters.</returns>
        public override string FormatClassName(string className)
            => ReplaceInvalidCharacters(ToPascalCase(className));

        /// <summary>
        /// Replaces invalid characters and converts to camelCase.<br/>
        /// <b>Example:</b> <c>propertyName</c>
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>camelCase string with no invalid characters.</returns>
        public override string FormatPublicPropertyName(string propertyName)
            => ReplaceInvalidCharacters(ToCamelCase(propertyName));

        /// <summary>
        /// Replaces invalid characters and converts to camelCase.<br/>
        /// <b>Example:</b> <c>_propertyName</c>
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>camelCase string with leading underscore and no invalid characters.</returns>
        public override string FormatPrivatePropertyName(string propertyName)
            => ReplaceInvalidCharacters($"_{ToCamelCase(propertyName)}");

        /// <summary>
        /// Replaces invalid characters and converts to camelCase.<br/>
        /// <b>Example:</b> <c>methodName</c>
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <returns>camelCase string with no invalid characters.</returns>
        public override string FormatPublicMethodName(string methodName)
            => ReplaceInvalidCharacters(ToCamelCase(methodName));

        /// <summary>
        /// Replaces invalid characters and converts to camelCase.<br/>
        /// <b>Example:</b> <c>_methodName</c>
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <returns>camelCase string with leading underscore and no invalid characters.</returns>
        public override string FormatPrivateMethodName(string methodName)
            => ReplaceInvalidCharacters($"_{ToCamelCase(methodName)}");

        /// <summary>
        /// Replaces invalid characters and converts to camelCase.<br/>
        /// <b>Example:</b> <c>fieldName</c>
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <returns>camelCase string with no invalid characters.</returns>
        public override string FormatPublicFieldName(string fieldName)
            => ReplaceInvalidCharacters(ToCamelCase(fieldName));

        /// <summary>
        /// Replaces invalid characters and converts to camelCase.<br/>
        /// <b>Example:</b> <c>_fieldName</c>
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <returns>camelCase string with leading underscore and no invalid characters.</returns>
        public override string FormatPrivateFieldName(string fieldName)
            => ReplaceInvalidCharacters($"_{ToCamelCase(fieldName)}");

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
        /// <b>Example:</b> <c>InterfaceName</c>
        /// </summary>
        /// <param name="interfaceName">Interface name</param>
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
        /// Replaces invalid characters and converts to UPPER_SNAKE_CASE.<br/>
        /// <b>Example:</b> <c>ENUM_MEMBER_NAME</c>
        /// </summary>
        /// <param name="enumMemberName">Enum member name</param>
        /// <returns>UPPER_SNAKE_CASE string with no invalid characters.</returns>
        public override string FormatEnumMemberName(string enumMemberName)
            => ReplaceInvalidCharacters(ToUpperSnakeCase(enumMemberName));
    }
}
