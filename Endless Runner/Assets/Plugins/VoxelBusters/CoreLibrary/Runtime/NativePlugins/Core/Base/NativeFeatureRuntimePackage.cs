using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    [System.Serializable]
    public class NativeFeatureRuntimePackage
    {
        #region Fields

        private readonly    RuntimePlatform[]   m_platforms;

        private readonly    string              m_custom;

        #endregion

        #region Properties

        public string Assembly { get; private set; }

        public string Namespace { get; private set; }

        public string NativeInterfaceType { get; private set; }

        public string[] BindingTypes { get; private set; }

        #endregion

        #region Constructors

        private NativeFeatureRuntimePackage(string assembly, string ns,
            string nativeInterfaceType, string[] bindingTypes = null,
            string custom = null, params RuntimePlatform[] platforms)
        {
            // Set properties
            m_platforms         = platforms;
            m_custom            = custom;
            Assembly            = assembly;
            Namespace           = ns;
            NativeInterfaceType = GetTypeFullName(ns, nativeInterfaceType);
            BindingTypes        = (bindingTypes != null)
                ? System.Array.ConvertAll(bindingTypes, (type) => GetTypeFullName(ns, type))
                : new string[0];
        }

        #endregion

        #region Static methods

        public static NativeFeatureRuntimePackage Generic(string assembly, string ns,
            string nativeInterfaceType, string[] bindingTypes = null)
        {
            return new NativeFeatureRuntimePackage(
                assembly: assembly,
                ns: ns,
                nativeInterfaceType: nativeInterfaceType,
                bindingTypes: bindingTypes);
        }

        public static NativeFeatureRuntimePackage Android(string assembly, string ns,
            string nativeInterfaceType, string[] bindingTypes = null)
        {
            return new NativeFeatureRuntimePackage(
                assembly: assembly,
                ns: ns,
                nativeInterfaceType: nativeInterfaceType,
                bindingTypes: bindingTypes,
                platforms: RuntimePlatform.Android);
        }

        public static NativeFeatureRuntimePackage IPhonePlayer(string assembly, string ns,
            string nativeInterfaceType, string[] bindingTypes = null)
        {
            return new NativeFeatureRuntimePackage(
                assembly: assembly,
                ns: ns,
                nativeInterfaceType: nativeInterfaceType,
                bindingTypes: bindingTypes,
                platforms: RuntimePlatform.IPhonePlayer);
        }

        public static NativeFeatureRuntimePackage TvOS(string assembly, string ns,
            string nativeInterfaceType, string[] bindingTypes = null)
        {
            return new NativeFeatureRuntimePackage(
                assembly: assembly,
                ns: ns,
                nativeInterfaceType: nativeInterfaceType,
                bindingTypes: bindingTypes,
                platforms: RuntimePlatform.tvOS);
        }

        public static NativeFeatureRuntimePackage iOS(string assembly, string ns,
            string nativeInterfaceType, string[] bindingTypes = null)
        {
            return new NativeFeatureRuntimePackage(
                assembly: assembly,
                ns: ns,
                nativeInterfaceType: nativeInterfaceType,
                bindingTypes: bindingTypes,
                platforms: new RuntimePlatform[] { RuntimePlatform.IPhonePlayer, RuntimePlatform.tvOS });
        }

        public static NativeFeatureRuntimePackage Custom(string custom, string assembly,
            string ns, string nativeInterfaceType,
            string[] bindingTypes = null)
        {
            return new NativeFeatureRuntimePackage(
                assembly: assembly,
                ns: ns,
                nativeInterfaceType: nativeInterfaceType,
                bindingTypes: bindingTypes,
                custom: custom);
        }

        private static string GetTypeFullName(string ns, string type) => $"{ns}.{type}";

        #endregion

        #region Public methods

        public System.Type[] GetBindingTypeReferences()
        {
            var     assembly    = ReflectionUtility.FindAssemblyWithName(Assembly);
            return System.Array.ConvertAll(BindingTypes, (item) => assembly.GetType(item));
        }

        public bool IsMatch(RuntimePlatform platform, string custom)
        {
            if ((custom != null) && string.Equals(m_custom, custom))
            {
                return true;
            }
            return SupportsPlatform(platform);
        }

        public bool SupportsPlatform(RuntimePlatform platform)
        {
            return (m_platforms == null) || System.Array.Exists(m_platforms, (value) => (value == platform));
        }

        #endregion
    }
}