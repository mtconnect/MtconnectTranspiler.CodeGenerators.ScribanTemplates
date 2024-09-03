using Scriban.Runtime;
using System;
using System.Reflection;

namespace MtconnectTranspiler.CodeGenerators.ScribanTemplates.Formatters
{
    /// <summary>
    /// A wrapper class for the <see cref="CodeFormatter"/> that exposes its methods to Scriban templates as a <see cref="ScriptObject"/>.
    /// </summary>
    /// <remarks>
    /// This wrapper provides access to the <see cref="CodeFormatter"/> methods within Scriban templates, allowing code formatting operations to be performed in the template context.
    /// The methods are dynamically imported based on their signatures, ensuring that only the appropriate methods are exposed in the template.
    /// </remarks>
    public class CodeFormatterWrapper : ScriptObject
    {
        private readonly CodeFormatter _formatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeFormatterWrapper"/> class.
        /// </summary>
        /// <param name="formatter">The <see cref="CodeFormatter"/> instance to be wrapped and exposed to Scriban templates.</param>
        public CodeFormatterWrapper(CodeFormatter formatter)
        {
            _formatter = formatter;

            // Use reflection to import all public instance methods that match the required signature
            foreach (var method in _formatter.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                // Ensure that we only import methods that return a string and have one string parameter
                if (method.ReturnType == typeof(string) && method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == typeof(string))
                {
                    // Create a delegate for the method
                    var delegateMethod = (Func<string, string>)Delegate.CreateDelegate(typeof(Func<string, string>), _formatter, method);

                    // Import the method into the script object using the method's name
                    this.Import(method.Name, delegateMethod);
                }
            }
        }
    }
}
