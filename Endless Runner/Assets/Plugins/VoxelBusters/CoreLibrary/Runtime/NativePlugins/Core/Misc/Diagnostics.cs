using System;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    public static class Diagnostics
    {
        #region Constants

        public  static  readonly    Error   kFeatureNotSupported            = new Error(description: "The requested operation could not be completed because this feature is not supported on current platform.");

        public  const   string              kCreateNativeObjectError        = "Failed to create native object.";

        #endregion

        #region Exception methods

        public static VBException PluginNotConfiguredException(string name = "Native")
        {
            return new VBException($"Please configure {name} plugin, before you start using it in your project.");
        }

        #endregion

        #region Log methods

        public static void LogNotSupportedInEditor(string featureName = "This")
        {
            DebugLogger.LogWarning(CoreLibraryDomain.NativePlugins, $"{featureName} feature is not supported by simulator.");
        }

        public static void LogNotSupported(string featureName = "This")
        {
            DebugLogger.LogWarning(CoreLibraryDomain.NativePlugins, $"{featureName} feature is not supported.");
        }

        #endregion
    }
}