using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Linq;

namespace CaptainOfPlanner.NewControls
{
    [DebuggerDisplay("Name")]
    /// <summary>
    /// Main factory class
    /// </summary>
    public class Plant : IEnumerable<Node>
    {
        /// <summary>
        /// Optional name of plat
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Node list is the basic plant structure, each node rapresent a resource process
        /// </summary>
        List<Node> nodes;

        public Plant(string name)
        {
            Name = name;
            nodes = new List<Node>();

        }

        /// <summary>
        /// Create a new node and add to nodes list
        /// </summary>
        public Node GenerateNode(NodeType type, string name = null)
        {
            Node node = null;
            switch (type)
            {
                case NodeType.Processor: node = new Processor(this, name); break;
                case NodeType.Balancer: node = new Balancer(this, name); break;
                case NodeType.Storage: node = new Storage(this, name); break;
                default: throw new Exception($"Unknow nodetype {type}");
            }
            AddNode(node);
            return node;
        }

        public void AddNode(Node node) 
        { 
            if (node != null) nodes.Add(node);
        }

        public void Clear()
        {
            while (nodes.Count > 0)
                RemoveNode(nodes[nodes.Count - 1]);
        }
        public void RemoveNode(Node node)
        {
            if (node == null) return;
            foreach (var link in node.Inputs) link.UnLink();
            foreach (var link in node.Outputs) link.UnLink();
            nodes.Remove(node);
           
        }


        /// <summary>
        /// Save Plant scene to xml file
        /// </summary>
        public void SaveXml(string xmlfile)
        {
            int ID = 1;
            foreach (var node in this)
            {
                foreach (var link in node.Inputs) link.xml_id = ID++;
                foreach (var link in node.Outputs) link.xml_id = ID++;
            }

            var doc = new XmlDocument();
            var plant = doc.CreateElement("Plant");
            plant.SetAttribute("name", Name);
            doc.AppendChild(plant);

            foreach (var node in this) 
                node.SaveXml(plant);

            File.Create(xmlfile).Dispose();
            doc.Save(xmlfile);
        }


        /// <summary>
        /// Load a new Plant scene from xml file
        /// </summary>
        public bool Load(string xmlfile)
        {
            var doc = new XmlDocument();
            doc.Load(xmlfile);
            if (doc.DocumentElement.Name != "Plant")
            {
                Console.WriteLine("Is not a plant xml");
                return false;
            }

            Name = doc.DocumentElement.GetAttribute("name");

            Dictionary<int, Link> ToResolve = new Dictionary<int, Link>();

            foreach (XmlElement element in doc.DocumentElement.ChildNodes)
            {
                Node node = Node.LoadXml(this, element, ToResolve);
            }
               
            foreach (var link in ToResolve.Values)
            {
                if (link.xml_linked_id > 0 && ToResolve.TryGetValue(link.xml_linked_id, out Link tolink) && link.IsLinkable(tolink))
                {
                    link.DoLink(tolink);
                }
            }
            return true;
        }


        public IEnumerator<Node> GetEnumerator() => nodes.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public override string ToString() => Name;
    }
}
