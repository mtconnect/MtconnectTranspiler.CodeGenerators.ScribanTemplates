using System.Collections.Generic;

namespace MtconnectTranspiler.CodeGenerators.ScribanTemplates.Formatters
{
    /// <summary>
    /// A code formatter implementation for C#.
    /// </summary>
    public class CSharpCodeFormatter : CodeFormatter
    {
        protected override HashSet<char> InvalidCharacters => base.InvalidCharacters;
        // Extend or override invalid characters specific to C#
        // For example: '@', which might be allowed in other languages but not in C#

        /// <summary>
        /// Replaces invalid characters and converts to PascalCase.<br/>
        /// <b>Example:</b> <c>ClassName</c>
        /// </summary>
        /// <param name="className">Class name</param>
        /// <returns>PascalCase string with no invalid characters.</returns>
        public override string FormatClassName(string className)
            => ReplaceInvalidCharacters(ToPascalCase(className));

        /// <summary>
        /// Replaces invalid characters and converts to PascalCase.<br/>
        /// <b>Example:</b> <c>PropertyName</c>
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>PascalCase string with no invalid characters.</returns>
        public override string FormatPublicPropertyName(string propertyName)
            => ReplaceInvalidCharacters(ToPascalCase(propertyName));

        /// <summary>
        /// Replaces invalid characters and converts to PascalCase.<br/>
        /// <b>Example:</b> <c>PropertyName</c>
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>PascalCase string with no invalid characters.</returns>
        public override string FormatPrivatePropertyName(string propertyName)
            => ReplaceInvalidCharacters(ToPascalCase(propertyName));

        /// <summary>
        /// Replaces invalid characters and converts to PascalCase.<br/>
        /// <b>Example:</b> <c>MethodName</c>
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <returns>PascalCase string with no invalid characters.</returns>
        public override string FormatPublicMethodName(string methodName)
            => ReplaceInvalidCharacters(ToPascalCase(methodName));

        /// <summary>
        /// Replaces invalid characters and converts to camelCase.<br/>
        /// <b>Example:</b> <c>methodName</c>
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <returns>camelCase string with no invalid characters.</returns>
        public override string FormatPrivateMethodName(string methodName)
            => ReplaceInvalidCharacters(ToCamelCase(methodName));

        /// <summary>
        /// Replaces invalid characters and converts to PascalCase.<br/>
        /// <b>Example:</b> <c>FieldName</c>
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <returns>PascalCase string with no invalid characters.</returns>
        public override string FormatPublicFieldName(string fieldName)
            => ReplaceInvalidCharacters(ToPascalCase(fieldName));

        /// <summary>
        /// Replaces invalid characters and converts to camelCase.<br/>
        /// <b>Example:</b> <c>_fieldName</c>
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <returns>camelCase string with no invalid characters.</returns>
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
        /// <b>Example:</b> <c>IInterfaceName</c>
        /// </summary>
        /// <param name="interfaceName">Interface name</param>
        /// <returns>PascalCase string with no invalid characters.</returns>
        public override string FormatInterfaceName(string interfaceName)
            => ReplaceInvalidCharacters($"I{ToPascalCase(interfaceName)}");

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
