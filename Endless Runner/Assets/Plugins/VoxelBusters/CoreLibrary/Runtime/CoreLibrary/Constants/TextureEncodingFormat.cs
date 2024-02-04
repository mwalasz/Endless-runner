using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    /// <summary>
    /// This enum is used to define the texture encoding technique to be used by the plugin.
    /// </summary>
    public enum TextureEncodingFormat
    {
        /// <summary> Encodes the given texture into PNG format.</summary>
        PNG,

        /// <summary> Encodes the given texture into JPEG format.</summary>
        JPG,
    }
}