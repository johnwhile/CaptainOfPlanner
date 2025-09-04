using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace CaptainOfPlanner.NewControls
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
        public abstract NodeType Type { get; }

        /// <summary>
        /// optional, location of control
        /// </summary>
        public Vector2i Position { get; set; }
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
        public LinkCollection Inputs;
        /// <summary>
        /// A node can contains more than one output
        /// </summary>
        public LinkCollection Outputs;


        protected Node(Plant plant, string name)
        {
            Name = name ?? "node";
            Plant = plant;

            Inputs = new LinkCollection(this, LinkType.Input);
            Outputs = new LinkCollection(this, LinkType.Output);
        }



        /// <summary>
        /// after node type is resolved, load xml data with derived implementation
        /// </summary>
        protected abstract void LoadDerivedXml(XmlElement node);
        protected abstract void SaveDerivedXml(XmlElement node);
        public virtual XmlElement SaveXml(XmlElement plant)
        {
            var node = plant.OwnerDocument.CreateElement(Type.ToString());
            if (!string.IsNullOrEmpty(Name)) node.SetAttribute("name", Name);
            node.SetAttribute("pos", Position.ToString());
            plant.AppendChild(node);

            SaveDerivedXml(node);

            foreach (var link in Inputs) link.SaveXml(node);
            foreach (var link in Outputs) link.SaveXml(node);

            return node;
        }

        /// <summary>
        /// load xml node.
        /// </summary>
        /// <param name="ToResolve">link list to resolve after loading all nodes</param>
        /// <returns></returns>
        public static Node LoadXml(Plant plant, XmlElement element, Dictionary<int, Link> ToResolve)
        {
            Node node = null;

            if (Enum.TryParse(element.Name, out NodeType type))
            {
                string name = element.GetAttribute("name");

                node = plant.GenerateNode(type);

                if (Vector2i.TryParse(element.GetAttribute("pos"), out Vector2i pos))
                    node.Position = pos;

                node.LoadDerivedXml(element);

                foreach (var link in node.Inputs) if (link.xml_id > 0) ToResolve.Add(link.xml_id, link);
                foreach (var link in node.Outputs) if (link.xml_id > 0) ToResolve.Add(link.xml_id, link);

            }
            return node;
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
