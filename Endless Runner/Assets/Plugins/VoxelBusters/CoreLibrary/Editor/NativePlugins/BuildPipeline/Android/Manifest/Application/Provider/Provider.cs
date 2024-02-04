using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Android
{
    public class Provider : Element
    {
        public void Add(MetaData element)
        {
            base.Add(element);
        }

        protected override string GetName()
        {
            return "provider";
        }
    }
}
