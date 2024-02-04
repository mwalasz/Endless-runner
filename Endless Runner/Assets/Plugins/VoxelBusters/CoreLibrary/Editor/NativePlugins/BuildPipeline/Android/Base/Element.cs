using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Android
{
    public abstract class Element
    {
        private readonly string DefaultNamespace    = "http://schemas.android.com/apk/res/android";
        private readonly string ToolsNamespace      = "http://schemas.android.com/tools";

        private List<Attribute>   m_attributes;
        private List<Element>     m_children;

        public void AddAttribute(string key, string value)
        {
            if (m_attributes == null)
                m_attributes = new List<Attribute>();

            m_attributes.Add(new Attribute(key, value));
        }

        protected virtual XmlElement ToXml(XmlDocument xmlDocument)
        {
            XmlElement element = xmlDocument.CreateElement(GetName());
            if(m_attributes != null)
            {
                foreach(Attribute attribute in m_attributes)
                {
                    string[] components = attribute.Key.Split(':');
                    
                    if (attribute.Key.Contains("xmlns") || components.Length == 1)
                    {
                        element.SetAttribute(attribute.Key, attribute.Value);
                    }
                    else
                    {
                        XmlAttribute newAttribute = xmlDocument.CreateAttribute(components[0], components[1], GetNamespace(components[0]));
                        newAttribute.Value = attribute.Value;
                        element.Attributes.Append(newAttribute);
                    }
                }
            }

            if (m_children != null)
            {
                foreach(Element eachChild in m_children)
                {
                    XmlElement xmlElement = eachChild.ToXml(xmlDocument);
                    element.AppendChild(xmlElement);
                }
            }

            return element;
        }

        protected virtual void Add(Element element)
        {
            if(m_children == null)
                m_children = new List<Element>();

            m_children.Add(element);
        }

        protected abstract string GetName();

        private string GetNamespace(string key)
        {
            switch(key)
            {
                case "android":
                    return DefaultNamespace;

                case "tools":
                    return ToolsNamespace;

                default:
                    return DefaultNamespace;
            }
        }
    }
}
