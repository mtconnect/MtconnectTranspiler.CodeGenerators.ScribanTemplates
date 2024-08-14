using System.Collections.Generic;
using System.Linq;

namespace MtconnectTranspiler.CodeGenerators.ScribanTemplates.Formatters
{
    /// <summary>
    /// A code formatter implementation for C++.
    /// </summary>
    public class CppCodeFormatter : CodeFormatter
    {
        protected override HashSet<char> InvalidCharacters => base.InvalidCharacters;
        // You can extend or override invalid characters specific to C++ here if needed

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
        /// Replaces invalid characters, converts to PascalCase, and adds "m_" prefix.<br/>
        /// <b>Example:</b> <c>m_PropertyName</c>
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>PascalCase string with "m_" prefix and no invalid characters.</returns>
        public override string FormatPrivatePropertyName(string propertyName)
            => ReplaceInvalidCharacters($"m_{ToPascalCase(propertyName)}");

        /// <summary>
        /// Replaces invalid characters and converts to PascalCase.<br/>
        /// <b>Example:</b> <c>MethodName</c>
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <returns>PascalCase string with no invalid characters.</returns>
        public override string FormatPublicMethodName(string methodName)
            => ReplaceInvalidCharacters(ToPascalCase(methodName));

        /// <summary>
        /// Replaces invalid characters and converts to PascalCase.<br/>
        /// <b>Example:</b> <c>MethodName</c>
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <returns>PascalCase string with no invalid characters.</returns>
        public override string FormatPrivateMethodName(string methodName)
            => ReplaceInvalidCharacters(ToPascalCase(methodName));

        /// <summary>
        /// Replaces invalid characters and converts to UPPER_SNAKE_CASE.<br/>
        /// <b>Example:</b> <c>FIELD_NAME</c>
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <returns>UPPER_SNAKE_CASE string with no invalid characters.</returns>
        public override string FormatPublicFieldName(string fieldName)
            => ReplaceInvalidCharacters(ToUpperSnakeCase(fieldName));

        /// <summary>
        /// Replaces invalid characters, converts to camelCase, and adds "m_" prefix.<br/>
        /// <b>Example:</b> <c>m_fieldName</c>
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <returns>camelCase string with "m_" prefix and no invalid characters.</returns>
        public override string FormatPrivateFieldName(string fieldName)
            => ReplaceInvalidCharacters($"m_{ToCamelCase(fieldName)}");

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
