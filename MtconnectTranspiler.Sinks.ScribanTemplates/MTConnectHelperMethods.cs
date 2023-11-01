using MtconnectTranspiler.Xmi;
using MtconnectTranspiler.Xmi.Profile;
using Scriban.Runtime;
using System.Collections.Generic;

namespace MtconnectTranspiler.Sinks.ScribanTemplates
{
    /// <summary>
    /// Helper methods related to processing MTConnect references.
    /// </summary>
    public partial class MTConnectHelperMethods : ScriptObject
    {
        /// <summary>
        /// Finds the first <see cref="Normative"/> definition for the type with the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="model">Reference to the whole <see cref="XmiDocument"/> in order to find all <see cref="Normative"/> references</param>
        /// <param name="id">Reference to the <c>xmi:id</c> which is used to compare against the <see cref="Normative.BaseElement"/></param>
        /// <returns>First find of the <see cref="Normative"/></returns>
        public static Normative LookupNormative(XmiDocument model, string id) => MtconnectTranspiler.Contracts.MTConnectHelper.LookupNormative(model, id);
        /// <summary>
        /// Finds the first <see cref="Deprecated"/> definition for the type with the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="model">Reference to the whole <see cref="XmiDocument"/> in order to find all <see cref="Normative"/> references</param>
        /// <param name="id">Reference to the <c>xmi:id</c> which is used to compare against the <see cref="Deprecated.BaseElement"/></param>
        /// <returns>First find of the <see cref="Deprecated"/></returns>
        public static Deprecated LookupDeprecated(XmiDocument model, string id) => MtconnectTranspiler.Contracts.MTConnectHelper.LookupDeprecated(model, id);

        // TODO: Remove these references to MtconnectVersions and leave that up to MtconnectCore
        /// <summary>
        /// An overridable collection of alternative version numbers, keyed by the version numbers defined within the <see cref="XmiDocument"/>.
        /// </summary>
        public static Dictionary<string, string> VersionEnumLookup { get; set; } = new Dictionary<string, string>()
        {
            { "1.0", "MtconnectVersions.V_1_0_1" },
            { "1.1", "MtconnectVersions.V_1_1_0" },
            { "1.2", "MtconnectVersions.V_1_2_0" },
            { "1.3", "MtconnectVersions.V_1_3_0" },
            { "1.4", "MtconnectVersions.V_1_4_0" },
            { "1.5", "MtconnectVersions.V_1_5_0" },
            { "1.6", "MtconnectVersions.V_1_6_0" },
            { "1.7", "MtconnectVersions.V_1_7_0" },
            { "1.8", "MtconnectVersions.V_1_8_0" },
            { "2.0", "MtconnectVersions.V_2_0_0" },
            { "2.1", "MtconnectVersions.V_2_1_0" },
            { "2.2", "MtconnectVersions.V_2_2_0" },
            { "2.3", "MtconnectVersions.V_2_3_0" }
        };
        /// <summary>
        /// References <see cref="VersionEnumLookup"/> to lookup an alternative to the version number referred to within the <see cref="XmiDocument"/>.
        /// </summary>
        /// <param name="version">Version number as defined within the <see cref="XmiDocument"/></param>
        /// <returns>Alternative version string, see <see cref="VersionEnumLookup"/>. Returns <c>null</c> if not found.</returns>
        public static string LookupMtconnectVersions(string version)
        {
            if (version == null) return null;
            if (!VersionEnumLookup.TryGetValue(version, out string versionEnum)) return null;
            return versionEnum;
        }
    }
}
