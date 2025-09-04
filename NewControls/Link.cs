using System.Xml;

namespace CaptainOfPlanner.NewControls
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

        public ResourceCount ResourceCount { get; }
        public Node Owner { get; private set; }
        public LinkType Type { get; }
        public Link Linked { get; set; }
        public ILinkable Controller { get; set; }

        public Link(Node owner, LinkType type, ResourceCount resource)
        {
            Owner = owner;
            Type = type;
            ResourceCount = resource;
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
            if (Linked != null) Linked.Linked = null;
            Linked = null;
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
        public XmlElement SaveXml(XmlElement parent)
        {
            if (Linked != null)
            {
                var link = parent.OwnerDocument.CreateElement(Type.ToString());
                link.SetAttribute("id", xml_id.ToString());
                link.SetAttribute("linkid", Linked.xml_id.ToString());
                link.SetAttribute("res", ResourceCount.Resource.Name);
                parent.AppendChild(link);
                return link;
            }
            return null;
        }

        public bool LoadXml(XmlElement node)
        {
            return false;
        }
    }
}

