using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace CaptainOfPlanner
{
    public enum NodeType
    {
        Processor,
        Balancer,
        Storage
    }

    [DebuggerDisplay("{Name}")]
    /// <summary>
    /// A node rapresent a single resource's function.
    /// </summary>
    public abstract class Node
    {
        protected static int instance_counter = 0;

        /// <summary>
        /// used for enumerator's method
        /// </summary>
        public int IteratorId = 0;
        /// <summary>
        /// optional: only valid when saving or loading xml
        /// </summary>
        public bool Mirrored = false;
        /// <summary>
        /// optional: only valid when saving or loading xml
        /// </summary>
        public Vector2i Position;

        public abstract NodeType Type { get; }
        /// <summary>
        /// optional name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Main owner of all nodes
        /// </summary>
        public Plant Plant { get; }
        /// <summary>
        /// A node can contains more than one input
        /// </summary>
        public LinkCollection InLinks;
        /// <summary>
        /// A node can contains more than one output
        /// </summary>
        public LinkCollection OutLinks;


        protected Node(Plant plant, string name)
        {
            Name = name ?? "node";
            Plant = plant;

            InLinks = new LinkCollection(this, LinkType.Input);
            OutLinks = new LinkCollection(this, LinkType.Output);
        }
        public abstract void UpdateFlowRate();
        public abstract void UpdateOutFlowRate();


        #region Read/White
        /// <summary>
        /// after node type is resolved, load xml data with derived implementation
        /// </summary>
        protected abstract void CompleatReadingXml(XmlElement node);
        protected abstract void CompleteWritingXml(XmlElement node);   
        public virtual XmlElement SaveXml(XmlElement plant)
        {
            var node = plant.OwnerDocument.CreateElement(Type.ToString());

            if (!string.IsNullOrEmpty(Name)) node.SetAttribute("name", Name);
            if (Mirrored) node.SetAttribute("mirror", Mirrored ? "true" : "false");

            node.SetAttribute("pos", Position.ToString());
            plant.AppendChild(node);

            CompleteWritingXml(node);

            foreach (var link in InLinks) link.SaveXml(node);
            foreach (var link in OutLinks) link.SaveXml(node);

            return node;
        }
        /// <summary>
        /// load xml node.
        /// </summary>
        /// <param name="ToResolve">link list to resolve after loading all nodes</param>
        public static Node LoadXml(Plant plant, XmlElement element, Dictionary<int, Link> ToResolve)
        {
            Node node = null;

            if (Enum.TryParse(element.Name, out NodeType type))
            {
                node = plant.GenerateNode(type);
                node.Name = element.GetAttribute("name");
                node.InLinks.Clear();
                node.OutLinks.Clear();

                if (!bool.TryParse(element.GetAttribute("mirror"), out node.Mirrored)) node.Mirrored = false;

                if (Vector2i.TryParse(element.GetAttribute("pos"), out Vector2i pos))
                    node.Position = pos;

                foreach (XmlElement child in element.ChildNodes)
                {
                    var link = Link.LoadXml(node, child, ToResolve);
                    switch (link.Type)
                    {
                        case LinkType.Input: node.InLinks.Add(link); break;
                        case LinkType.Output: node.OutLinks.Add(link); break;
                    }
                }

                node.CompleatReadingXml(element);
            }
            return node;
        }
        #endregion
    }
}
