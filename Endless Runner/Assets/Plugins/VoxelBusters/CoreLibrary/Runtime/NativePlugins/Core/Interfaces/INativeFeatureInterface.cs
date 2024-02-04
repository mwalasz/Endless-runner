using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    public interface INativeFeatureInterface : INativeObject
    {
        #region Properties

        bool IsAvailable { get; }

        #endregion
    }
}