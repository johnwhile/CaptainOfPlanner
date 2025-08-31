using CaptainOfPlanner.Controls;
using System;
using System.Collections.Generic;
using System.Xml;
using static System.Windows.Forms.LinkLabel;

namespace CaptainOfPlanner
{
    public enum LinkType
    {
        Input,
        Output
    }

    public class Link : IDisposable
    {
        /// <summary>
        /// unique integer to define class when esport to xml
        /// </summary>
        internal int xml_id = -1;
        internal int xml_linked_id = -1;

        public ResourceCount ResourceCount { get; }
        public Node Owner { get; private set; }
        public LinkControl Control { get; set; }
        public LinkType Type { get; }
        public Link Linked { get; set; }

        public Link(Node owner, LinkType type, ResourceCount resource)
        {
            Owner = owner;
            Type = type;
            ResourceCount = resource;
            Control = new LinkControl();
            Control.Node = this;
        }


        /// <summary>
        /// must be same resource and must be opposite of Input or Output type
        /// </summary>
        public bool IsLinkable(Link other) =>
            ResourceCount.Resource.IsCompatible(other.ResourceCount.Resource) && 
            Type != other.Type;

        public override string ToString()
        {
            return $"{Type} {ResourceCount.ToString()}";
        }

        /// <summary>
        /// Save only linked link
        /// </summary>
        public void SaveXml(XmlElement node)
        {
            if (Linked != null)
            {
                var link = node.OwnerDocument.CreateElement(Type.ToString());
                link.SetAttribute("id", xml_id.ToString());
                link.SetAttribute("linkid", Linked.xml_id.ToString());
                link.SetAttribute("res", ResourceCount.Resource.Name);
                node.AppendChild(link);
            }
        }


        public void Dispose()
        {
            Owner = null;
            if (Linked != null)
            {
                Linked.Linked = null;
                Linked.Control.Invalidate();
            }
            Linked = null;
        }
    }
}

