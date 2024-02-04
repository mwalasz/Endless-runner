using System;
using System.Collections;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    public class NativeFeatureRuntimeConfiguration
    {
        #region Properties

        public NativeFeatureRuntimePackage[] Packages { get; private set; }

        public NativeFeatureRuntimePackage SimulatorPackage { get; private set; }

        public NativeFeatureRuntimePackage FallbackPackage { get; private set; }

        #endregion

        #region Constructors

        public NativeFeatureRuntimeConfiguration(NativeFeatureRuntimePackage[] packages, NativeFeatureRuntimePackage simulatorPackage = null,
            NativeFeatureRuntimePackage fallbackPackage = null)
        {
            // Set properties
            Packages            = packages;
            SimulatorPackage    = simulatorPackage;
            FallbackPackage     = fallbackPackage;
        }

        #endregion

        #region Public methods

        public NativeFeatureRuntimePackage GetPackageForPlatform(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.LinuxEditor:
                    return SimulatorPackage;

                default:
                    return Array.Find(Packages, (item) => item.SupportsPlatform(platform));
            }
        }

        #endregion
    }
}