using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace CaptainOfPlanner.NewControls
{
    public delegate void PlantEventHandler(Plant sender);
    public delegate void NodeEventHandler(Node sender);

    [DebuggerDisplay("Name")]
    /// <summary>
    /// Main factory class
    /// </summary>
    public class Plant
    {
        /// <summary>
        /// Optional name of plat
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Node list is the basic plant structure, each node rapresent a resource process
        /// </summary>
        public NodeCollection Nodes { get; }
        

        public Plant(string name)
        {
            Name = name;
            Nodes = new NodeCollection(this);

        }


        /// <summary>
        /// Generate when you create a new Node
        /// </summary>
        public event NodeEventHandler OnNodeCreating;
        public event NodeEventHandler OnNodeRemoving;


        void OnNodeClosing(Node sender)
        {
            Nodes.RemoveNode(sender);

            foreach (var d in OnNodeCreating.GetInvocationList()) 
                OnNodeCreating -= (NodeEventHandler)d;

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
        /// Create a plant's node
        /// </summary>
        public Node CreateNode(NodeType type, string name = null)
        {
            Console.WriteLine("Create Node " + type);
            Node node = null;
            switch (type)
            {
                case NodeType.Processor:node= new Processor(this, name); break;
            }
            Nodes.AddNode(node);
            return node;
        }

        /// <summary>
        /// Save Plant scene to xml file
        /// </summary>
        public void SaveXml(string xmlfile)
        {
            int ID = 1;
            foreach (var node in Nodes)
            {
                //foreach (var link in node.Inputs) link.xml_id = ID++;
                //foreach (var link in node.Outputs) link.xml_id = ID++;
            }

            var doc = new XmlDocument();
            var plant = doc.CreateElement("Plant");
            plant.SetAttribute("name", Name);
            doc.AppendChild(plant);

            foreach (var node in Nodes) 
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
                Node.LoadXml(this, element, ToResolve);
            
            foreach (var link in ToResolve.Values)
            {
                if (link.xml_linked_id > 0 && ToResolve.TryGetValue(link.xml_linked_id, out Link tolink) && link.IsLinkable(tolink))
                {

                }
            }
            return true;
        }

        public override string ToString() => Name;
    }
}
