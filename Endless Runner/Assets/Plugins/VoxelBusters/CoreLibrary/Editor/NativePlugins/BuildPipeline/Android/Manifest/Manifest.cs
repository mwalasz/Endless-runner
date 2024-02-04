using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Android
{
    public class Manifest : Element
    {
        public void Add(Application element)
        {
            base.Add(element);
        }

        public void Add(Permission element)
        {
            base.Add(element);
        }

        public void Add(Feature element)
        {
            base.Add(element);
        }

        public void Add(Configuration element)
        {
            base.Add(element);
        }

        public void Add(SupportedGLTexture element)
        {
            base.Add(element);
        }

        public void Add(PermissionGroup element)
        {
            base.Add(element);
        }

        public void Add(PermissionTree element)
        {
            base.Add(element);
        }

        public void Add(Instrumentation element)
        {
            base.Add(element);
        }

        public void Add(CompatibleScreens element)
        {
            base.Add(element);
        }

        public void Add(SupportedScreens element)
        {
            base.Add(element);
        }

        public void Add(SDK element)
        {
            base.Add(element);
        }

        public void Add(Queries queries)
        {
            base.Add(queries);
        }

        public XmlElement GenerateXml(XmlDocument xmlDocument)
        {
            return base.ToXml(xmlDocument);
        }

        protected override string GetName()
        {
            return "manifest";
        }
    }
}