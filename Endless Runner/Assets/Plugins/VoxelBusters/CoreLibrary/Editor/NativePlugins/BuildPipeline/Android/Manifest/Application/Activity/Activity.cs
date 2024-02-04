using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Android
{
    public class Activity : AppComponent
    {
        public Layout Layout
        {
            get;
            set;
        }

        protected override string GetName()
        {
            return "activity";
        }
    }
}

