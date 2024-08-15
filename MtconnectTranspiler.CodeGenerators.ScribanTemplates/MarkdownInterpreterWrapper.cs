using MtconnectTranspiler.Interpreters;
using Scriban.Runtime;
using System;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;

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

            // Use reflection to import all public instance methods
            foreach (var method in _interpreter.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (method.ReturnType == typeof(string))
                {
                    var parameters = method.GetParameters();

                    // Create an array of Type representing the parameter types
                    Type[] paramTypes = parameters.Select(p => p.ParameterType).ToArray();

                    // Create the delegate type based on the method's parameter types
                    Type delegateType = Expression.GetFuncType(paramTypes.Concat(new[] { typeof(string) }).ToArray());

                    // Create a delegate for the method
                    var del = Delegate.CreateDelegate(delegateType, _interpreter, method);

                    // Import the method into the script object using the method's name
                    this.Import(method.Name, del);
                }
            }
        }
    }
}
