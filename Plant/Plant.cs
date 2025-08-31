using CaptainOfPlanner.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace CaptainOfPlanner
{
    public class PlantNodes : IEnumerable<Node> , IDisposable
    {
        Plant plant;
        List<Node> nodes;

        public PlantNodes(Plant plant)
        {
            this.plant = plant;
            nodes = new List<Node>();
        }
        public void AddNode(Node node)
        {
            if (node == null) return;
            nodes.Add(node);
            plant.Control.AddNodeControl(node.Control);
        }
        public void RemoveNode(Node node)
        {
            if (node == null) return;
            nodes.Remove(node);
            node.Inputs.Clear();
            node.Outputs.Clear();
            plant.Control.RemoveNodeControl(node.Control);
        }


        public IEnumerator<Node> GetEnumerator()
        {
            foreach (var node in nodes) yield return node;
        }

        IEnumerator IEnumerable.GetEnumerator()=> GetEnumerator();

        public void Dispose()
        {
            while (nodes.Count > 0) RemoveNode(nodes[nodes.Count - 1]);
        }
    }

    /// <summary>
    /// Main factory class
    /// </summary>
    public class Plant : IDisposable
    {
        PlantControl control;

        public string Name { get; set; }
        /// <summary>
        /// Node list is the basic plant structure, each node rapresent a resource process
        /// </summary>
        public PlantNodes Nodes { get; }
        public PlantControl Control
        {
            get => control;
            set
            {
                if (control != null) control.OnNodeClosing -= OnNodeClosing;
                if (value != null)
                {
                    value.OnNodeClosing += OnNodeClosing;
                }
                control = value;
            }
        }
        
        public Plant(string name)
        {
            Name = name;
            Nodes = new PlantNodes(this);
        }

        private void OnNodeClosing(Node sender)
        {
            Nodes.RemoveNode(sender);
        }

        public void UnLinking(Link A)
        {
            var tmp = A.Linked;
            A.Linked = null;
            if (tmp != null)
            {
                tmp.Linked = null;
                tmp.Control?.Invalidate();
            }
        }

        public bool Linking(Link A, Link B)
        {
            if (!A.IsLinkable(B)) return false;

            //avoid self linking
            if (A.Owner==B.Owner) return false;

            UnLinking(A);
            UnLinking(B);

            A.Linked = B;
            B.Linked = A;

            return true;
        }


        /// <summary>
        /// Create a plant's node and its controller.
        /// </summary>
        public Node CreateNode(NodeType type, string name = null)
        {
            Console.WriteLine("Create Node " + type);
            Node node;
            switch (type)
            {
                case NodeType.Processor:node= new Processor(this, name); break;
                case NodeType.Balancer:node = new Balancer(this, name); break;
                case NodeType.Storage:node = new Storage(this, name); break;
                default:node = new Node(this, name); break;
            }
            Nodes.AddNode(node);
            return node;
        }

        /// <summary>
        /// Save Plant scene to xml file
        /// </summary>
        public void SaveXml(string xmlfile)
        {
            RecalculateLinkId();

            var doc = new XmlDocument();
            var plant = doc.CreateElement("Plant");
            plant.SetAttribute("name", Name);
            doc.AppendChild(plant);

            foreach (var node in Nodes) 
                node.SaveXml(plant);

            File.Create(xmlfile).Dispose();
            doc.Save(xmlfile);
        }


        void RecalculateLinkId()
        {
            int ID = 1;
            foreach(var node in Nodes)
            {
                foreach (var link in node.Inputs)
                    link.xml_id = ID++;
                foreach (var link in node.Outputs)
                    link.xml_id = ID++;
            }
        }

        public void Dispose()
        {
            Nodes.Dispose();
            Control = null;
        }


        /// <summary>
        /// Load a new Plant scene from xml file
        /// </summary>
        public static Plant Load(string xmlfile, PlantControl control)
        {
            var doc = new XmlDocument();
            doc.Load(xmlfile);
            if (doc.DocumentElement.Name != "Plant")
            {
                Console.WriteLine("Is not a plant xml");
                return null;
            }
            var plant = new Plant(doc.DocumentElement.GetAttribute("name"));
            plant.Control = control;
            control.Plant = plant;

            Dictionary<int, Link> ToResolve = new Dictionary<int, Link>();

            foreach (XmlElement element in doc.DocumentElement.ChildNodes)
                Node.LoadXml(plant, element, ToResolve);
            

            foreach (var link in ToResolve.Values)
            {
                if (link.xml_linked_id > 0 && ToResolve.TryGetValue(link.xml_linked_id, out Link tolink) && link.IsLinkable(tolink))
                {
                    link.Linked = tolink;
                    tolink.Linked = link;
                    link.Control?.Invalidate();
                    tolink.Control?.Invalidate();
                }

            }
            plant.Control.Invalidate();
            return plant;
        }
    }
}
