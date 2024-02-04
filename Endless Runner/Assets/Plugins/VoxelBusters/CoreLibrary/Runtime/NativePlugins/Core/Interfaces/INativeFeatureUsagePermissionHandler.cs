using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    public interface INativeFeatureUsagePermissionHandler
    {
        void ShowPrepermissionDialog(string permissionType, Callback onAllowCallback, Callback onDenyCallback);
    }
}