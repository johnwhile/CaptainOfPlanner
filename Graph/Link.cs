using System;
using System.Collections.Generic;
using System.Xml;

namespace CaptainOfPlanner
{
    public enum LinkType
    {
        Undefined,
        Input,
        Output
    }

    /// <summary>
    /// A link can be an input or output source for one resource
    /// </summary>
    public class Link
    {
        /// <summary>
        /// unique integer to define class when esport to xml
        /// </summary>
        internal int xml_id = -1;
        internal int xml_linked_id = -1;

        /// <summary>
        /// flow entering to be processed
        /// </summary>
        public float Entering;
        /// <summary>
        /// flow not processed calcuated as <see cref="Entering"/> - <see cref="Forward"/>
        /// </summary>
        public float Backward => Entering - Forward;
        /// <summary>
        /// flow that continue to the link
        /// </summary>
        public float Forward;

        public int Quantity;
        public Resource Resource { get; internal set; }
        public Node Owner { get; private set; }
        public LinkType Type { get; }
        public Link Linked { get; set; }
        public ILinkable Controller { get; set; }
        public bool Priority { get; set; }
        public bool IsLinked => Linked != null;

        public Link(Node owner, LinkType type, Resource resource, int resourceCount = 0)
        {
            Owner = owner;
            Type = type;
            Resource = resource;
            Quantity = resourceCount;
            Priority = false;
            Forward = 0;
            Entering = 0;
        }
        public void DoLink(Link other)
        {
            if (!IsLinkable(other)) return;
            if (Linked != other) UnLink();
            if (other.Linked != this) other.UnLink();

            other.Linked = this;
            Linked = other;

            Controller?.DoLink(other);
        }

        public void UnLink()
        {
            Controller?.UnLink();
            if (IsLinked)
            {
                Linked.Controller?.UnLink();
                Linked.Linked = null;
            }
            Linked = null;
        }

        /// <summary>
        /// must be same resource and must be opposite of Input or Output type
        /// </summary>
        public bool IsLinkable(Link other) =>
            Resource.IsCompatible(other.Resource) &&
            Type != other.Type &&
            Owner != other.Owner;

        /// <summary>
        /// Save only linked link
        /// </summary>
        public XmlElement SaveXml(XmlElement parent)
        {
            if (IsLinked)
            {
                var link = parent.OwnerDocument.CreateElement(Type.ToString());
                link.SetAttribute("id", xml_id.ToString());
                link.SetAttribute("linkid", Linked.xml_id.ToString());
                link.SetAttribute("resource", Resource);
                if (Priority) link.SetAttribute("priority", "true");
                parent.AppendChild(link);
                return link;
            }
            return null;
        }

        public static Link LoadXml(Node node, XmlElement element, Dictionary<int, Link> ToResolve = null)
        {
            if (!Enum.TryParse(element.Name, out LinkType type)) type = LinkType.Undefined;
            if (!ResourcesManager.TryGetResource(element.GetAttribute("resource"), out Resource resource)) resource = Resource.Undefined;
            var link = new Link(node, type, resource);

            if (!int.TryParse(element.GetAttribute("id"), out link.xml_id)) link.xml_id = -1;
            if (!int.TryParse(element.GetAttribute("linkid"), out link.xml_linked_id)) link.xml_linked_id = -1;
            if (!bool.TryParse(element.GetAttribute("priority"), out bool priority)) priority = false;
            link.Priority = priority;

            if (ToResolve != null && link.xml_id > -1) ToResolve.Add(link.xml_id, link);

            return link;
        }

        public override string ToString()
        {
            return $"{Type} {Resource.ToString()}";
        }
    }
}

