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

        protected Node(Plant plant, string name)
        {
            Name = name ?? "node";
            Plant = plant;
        }

        public virtual XmlElement SaveXml(XmlElement plant)
        {
            var node = plant.OwnerDocument.CreateElement(Type.ToString());
            if (!string.IsNullOrEmpty(Name)) node.SetAttribute("name", Name);
            node.SetAttribute("pos", Position.ToString());
            plant.AppendChild(node);

            //Inputs.SaveXml(node);
            //Outputs.SaveXml(node);

            return node;
        }

        /// <summary>
        /// after node type is resolved, load xml data with derived implementation
        /// </summary>
        /// <param name="node"></param>
        /// <param name="ToResolve"></param>
        protected abstract void LoadXml(XmlElement node, Dictionary<int, Link> ToResolve);


        public static Node LoadXml(Plant plant, XmlElement element, Dictionary<int, Link> ToResolve)
        {
            Node node = null;

            if (Enum.TryParse(element.Name, out NodeType type))
            {
                string name = element.GetAttribute("name");
                node = plant.CreateNode(type, name);

                if (Vector2i.TryParse(element.GetAttribute("pos"), out Vector2i pos))
                    node.Position = pos;

                node.LoadXml(element, ToResolve);
            }
            return node;
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
