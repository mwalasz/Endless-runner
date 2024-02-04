using System;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    /// <summary>
    /// The enum is used to indicate the platform application is running.
    /// </summary>
    [Flags]
    public enum NativePlatform
    {
        /// <summary> The runtime platform could not be determined.</summary>
        Unknown     = 0,

        /// <summary> The runtime platform is iOS.</summary>
        iOS         = 1 << 0,

        /// <summary> The runtime platform is tvOS.</summary>
        tvOS        = 1 << 1,

        /// <summary> The runtime platform is Android.</summary>
        Android     = 1 << 2,

        All         = iOS | tvOS | Android,
    }
}