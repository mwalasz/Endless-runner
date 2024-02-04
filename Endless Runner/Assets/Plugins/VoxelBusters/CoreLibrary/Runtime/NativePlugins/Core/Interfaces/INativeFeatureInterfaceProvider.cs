using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    public interface INativeFeatureInterfaceProvider
    {
        INativeFeatureInterface CreateInterface(Type interfaceType, RuntimePlatform platform);
    }
}