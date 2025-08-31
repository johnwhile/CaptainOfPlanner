using CaptainOfPlanner.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;

namespace CaptainOfPlanner
{
    public enum NodeType
    {
        Generic,
        Processor,
        Balancer,
        Storage
    }

    /// <summary>
    /// A node rapresent a single resource's function.
    /// This base class can be used as generic node
    /// </summary>
    public class Node
    {
        protected static int instance_counter = 0;
        public virtual NodeType Type { get => NodeType.Generic; }

        /// <summary>
        /// optional name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Main owner of all nodes
        /// </summary>
        public Plant Plant { get; }
        /// <summary>
        /// bound size of node in plant space
        /// </summary>
        protected RectangleF aabb;
        /// <summary>
        /// the base controller to interact with
        /// </summary>
        public NodeControl Control { get; protected set; }

        public LinkCollection Inputs;
        public LinkCollection Outputs;


        internal Node(Plant plant, string name = "generic")
        {
            Name = name ?? "generic";
            Plant = plant;

            Control = GenerateControl();

            Inputs = new LinkCollection(this);
            Outputs = new LinkCollection(this);
        }

        protected virtual NodeControl GenerateControl() => new NodeControl(this);

        public virtual XmlElement SaveXml(XmlElement plant)
        {
            var node = plant.OwnerDocument.CreateElement(Type.ToString());
            
            if (!string.IsNullOrEmpty(Name)) node.SetAttribute("name", Name);
            
            node.SetAttribute("pos", ((Vector2i)Control.Location).ToString());
            
            plant.AppendChild(node);

            Inputs.SaveXml(node);
            Outputs.SaveXml(node);

            return node;
        }

        public static Node LoadXml(Plant plant, XmlElement element, Dictionary<int, Link> ToResolve)
        {
            Node node = null;

            if (Enum.TryParse(element.Name, out NodeType type))
            {
                string name = element.GetAttribute("name");
                node = plant.CreateNode(type, name);

                if (Vector2i.TryParse(element.GetAttribute("pos"), out Vector2i pos))
                    node.Control.Location = pos;

                if (node is Processor processor)
                {
                    processor.LoadXml(element);
                }

                foreach (XmlElement xmlink in element.ChildNodes)
                {
                    string resource = xmlink.GetAttribute("res");

                    LinkCollection collection;
                    switch (xmlink.Name)
                    {
                        case "Input": collection = node.Inputs; break;
                        case "Output": collection = node.Outputs; break;
                        default: continue;
                    }

                    if (collection.Find(resource, out Link link))
                    {
                        if (!int.TryParse(xmlink.GetAttribute("id"), out link.xml_id))
                            link.xml_id = -1;
                        if (!int.TryParse(xmlink.GetAttribute("linkid"), out link.xml_linked_id))
                            link.xml_linked_id = -1;

                        if (link.xml_id > 0) ToResolve.Add(link.xml_id, link);
                    }
                }

            }
            return node;
        }
    }


}
