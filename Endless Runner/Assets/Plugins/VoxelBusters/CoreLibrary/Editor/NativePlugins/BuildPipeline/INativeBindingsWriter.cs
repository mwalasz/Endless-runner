using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    public interface INativeBindingsWriter
    {
        #region Methods

        void WriteStart(string product = null,
                        string author = null,
                        string copyrights = null);

        void WriteCustomTypeDeclarations(System.Type[] customTypes);

        void WriteMethod(MethodInfo method);

        void WriteEnd(out string[] files);

        #endregion
    }
}