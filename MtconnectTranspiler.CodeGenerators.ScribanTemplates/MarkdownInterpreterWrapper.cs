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

            // Expose different Interpret methods with unique names in the script context

            /// <summary>
            /// Interprets a string using the <see cref="MarkdownInterpreter.Interpret(string)"/> method.
            /// </summary>
            /// <remarks>
            /// In Scriban templates, this method can be accessed as <c>interpret_string</c>.
            /// </remarks>
            this.Import("interpret_string", new Func<string, string>(_interpreter.Interpret));

            /// <summary>
            /// Interprets a single <see cref="OwnedComment"/> object using the <see cref="MarkdownInterpreter.Interpret(OwnedComment)"/> method.
            /// </summary>
            /// <remarks>
            /// In Scriban templates, this method can be accessed as <c>interpret_comment</c>.
            /// </remarks>
            this.Import("interpret_comment", new Func<Xmi.OwnedComment, string>(_interpreter.Interpret));

            /// <summary>
            /// Interprets an array of <see cref="OwnedComment"/> objects using the <see cref="MarkdownInterpreter.Interpret(OwnedComment[])"/> method.
            /// </summary>
            /// <remarks>
            /// In Scriban templates, this method can be accessed as <c>interpret_comments_array</c>.
            /// </remarks>
            this.Import("interpret_comments_array", new Func<Xmi.OwnedComment[], string>(_interpreter.Interpret));

            //// Use reflection to import all public instance methods
            //foreach (var method in _interpreter.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance))
            //{
            //    if (method.ReturnType == typeof(string))
            //    {
            //        var parameters = method.GetParameters();

            //        // Create an array of Type representing the parameter types
            //        Type[] paramTypes = parameters.Select(p => p.ParameterType).ToArray();

            //        // Create the delegate type based on the method's parameter types
            //        Type delegateType = Expression.GetFuncType(paramTypes.Concat(new[] { typeof(string) }).ToArray());

            //        // Create a delegate for the method
            //        var del = Delegate.CreateDelegate(delegateType, _interpreter, method);

            //        // Import the method into the script object using the method's name
            //        this.Import(method.Name, del);
            //    }
            //}
        }
    }
}
