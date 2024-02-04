using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Android
{
    public class Application : Element
    {
        public void Add(Activity element)
        {
            base.Add(element);
        }

        public void Add(ActivityAlias element)
        {
            base.Add(element);
        }

        public void Add(MetaData element)
        {
            base.Add(element);
        }
        public void Add(Service element)
        {
            base.Add(element);
        }
        public void Add(Receiver element)
        {
            base.Add(element);
        }

        public void Add(Provider element)
        {
            base.Add(element);
        }

        public void Add(Library element)
        {
            base.Add(element);
        }

        protected override string GetName()
        {
            return "application";
        }
    }
}
