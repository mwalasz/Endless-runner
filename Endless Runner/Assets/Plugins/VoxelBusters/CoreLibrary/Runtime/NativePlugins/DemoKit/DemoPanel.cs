using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins.DemoKit
{
    public abstract class DemoPanel : MonoBehaviour
    {
        #region Methods

        public abstract void Rebuild();

        #endregion

    }
}