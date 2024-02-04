using System.Collections;
using System.Collections.Generic;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Android
{
    public class CompatibleScreens : Element
    {
        public void Add(Screen element)
        {
            base.Add(element);
        }
        protected override string GetName()
        {
            return "compatible-screens";
        }

        public class Screen : Element
        {
            protected override string GetName()
            {
                return "screen";
            }
        }
    }
}
