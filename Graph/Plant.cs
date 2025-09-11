using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace CaptainOfPlanner
{
    [DebuggerDisplay("Name")]
    /// <summary>
    /// Main factory class
    /// Node list is the basic plant structure, each node rapresent a resource process
    /// </summary>
    public class Plant : IEnumerable<Node>
    {
        List<Processor> Processors;
        List<Storage> Storages;
        List<Balancer> Balancers;
        /// <summary>
        /// Total nodes count
        /// </summary>
        public int Count => Processors.Count + Storages.Count + Balancers.Count;
        /// <summary>
        /// Optional name of plat
        /// </summary>
        public string Name { get; set; }

        public Plant(string name)
        {
            Name = name;
            Storages = new List<Storage>();
            Balancers = new List<Balancer>();
            Processors = new List<Processor>();
        }
        /// <summary>
        /// Main algorithm
        /// </summary>
        public void RUN(int iterations = 10)
        {
            foreach (var node in this) node.IteratorId = 0;

            //find best starting node. There can be separate networks so for each network
            //there is a different starting point

            //inizialize first state of flow

            foreach (var node in this)
            {
                node.UpdateFlowRate();
            }
            //compute process and set output
            foreach (var node in this)
            {
                node.UpdateFlowRate();
            }
        }





        /// <summary>
        /// Create a new node and add to nodes list
        /// </summary>
        public Node GenerateNode(NodeType type, string name = null)
        {
            switch (type)
            {
                case NodeType.Processor:
                    var pro = new Processor(this, name);
                    Processors.Add(pro);
                    return pro;
                case NodeType.Balancer:
                    var bal = new Balancer(this, name);
                    Balancers.Add(bal);
                    return bal;
                case NodeType.Storage: 
                    var sto = new Storage(this, name);
                    Storages.Add(sto);
                    return sto;
                default:
                    throw new Exception($"Unknow nodetype {type}");
            }
        }

        public void Clear()
        {
            while (Processors.Count > 0) RemoveNode(Processors.Last());
            while (Balancers.Count > 0) RemoveNode(Balancers.Last());
            while (Storages.Count > 0) RemoveNode(Storages.Last());
        }

        public void RemoveNode(Processor node)
        {
            Remove(node);
            Processors.Remove(node);
        }
        public void RemoveNode(Balancer node)
        {
            Remove(node);
            Balancers.Remove(node);
        }
        public void RemoveNode(Storage node)
        {
            Remove(node);
            Storages.Remove(node);
        }
        void Remove(Node node)
        {
            if (node == null) return;
            foreach (var link in node.InLinks) 
                link.UnLink();
            foreach (var link in node.OutLinks) link.UnLink();
        }

        /// <summary>
        /// Save Plant scene to xml file
        /// </summary>
        public void SaveXml(string xmlfile)
        {
            int ID = 1;
            foreach (var node in this)
            {
                foreach (var link in node.InLinks) link.xml_id = ID++;
                foreach (var link in node.OutLinks) link.xml_id = ID++;
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


        public IEnumerator<Node> GetEnumerator()
        {
            foreach (var node in Storages) yield return node;
            foreach (var node in Balancers) yield return node;
            foreach (var node in Processors) yield return node;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public override string ToString() => Name;

    }
}
