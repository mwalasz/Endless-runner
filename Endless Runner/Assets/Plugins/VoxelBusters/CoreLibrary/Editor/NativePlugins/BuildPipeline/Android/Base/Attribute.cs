using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Android
{
    public class Attribute
    {
        public string Key
        {
            get;
            private set;
        }

        public string Value
        {
            get;
            private set;
        }

        public Attribute(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
