using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    [System.Flags]
    public enum PBXTargetMembership
    {
        UnityIphone = 1 << 0,

        UnityIphoneTests = 1 << 1,

        UnityFramework = 1 << 2,
    }
}