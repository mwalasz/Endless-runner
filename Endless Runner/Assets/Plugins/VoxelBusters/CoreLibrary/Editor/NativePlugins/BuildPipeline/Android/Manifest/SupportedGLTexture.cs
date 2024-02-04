using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Android
{
    public class SupportedGLTexture : Element
    {
        protected override string GetName()
        {
            return "supports-gl-texture";
        }
    }
}