using System;

namespace MtconnectTranspiler.Sinks.ScribanTemplates
{
    /// <summary>
    /// An attribute used to relate a <c>class</c> with a Scriban-formatted template contained in the "Templates" folder of the project.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ScribanTemplateAttribute : Attribute
    {
        /// <summary>
        /// Reference to the filename within the "Templates" folder to pull the Scriban template.
        /// </summary>
        public string Filename { get; }

        /// <summary>
        /// Constructs a new <see cref="ScribanTemplateAttribute"/> to reference a Scriban template relative to an entity.
        /// </summary>
        /// <param name="filename"><inheritdoc cref="Filename" path="/summary"/></param>
        public ScribanTemplateAttribute(string filename)
        {
            Filename = filename;
        }
    }
}
