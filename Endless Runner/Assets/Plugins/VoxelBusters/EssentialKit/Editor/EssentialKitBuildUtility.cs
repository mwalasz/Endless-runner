using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor;
using VoxelBusters.EssentialKit;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.Build;

namespace VoxelBusters.EssentialKit.Editor
{
    public static class EssentialKitBuildUtility 
    {
        #region Static methods

        public static bool IsReleaseBuild()
        {
            var     firstPackage    = ImplementationSchema.AddressBook.GetPackageForPlatform(RuntimePlatform.OSXEditor);
            return !(firstPackage == null || ReflectionUtility.FindAssemblyWithName(firstPackage.Assembly) == null);
        }

        #endregion
    }
}