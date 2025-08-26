using CaptainOfPlanner.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;

namespace CaptainOfPlanner
{
    /// <summary>
    /// A node rapresent a single resource's function.
    /// This base class can be used as generic node
    /// </summary>
    public class Node
    {
        protected static int instance_counter = 0;
        public virtual PlantNodeType Type { get => PlantNodeType.Generic; }
        
        /// <summary>
        /// optional name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// unique integer to define class
        /// </summary>
        public int Id { get; }
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

        internal Node(Plant plant, string name = "generic node") 
        {
            Name = name;
            Plant = plant;
            Id = instance_counter++;

            CreateController();
        }

        public virtual NodeControl CreateController()
        {
            Control = null;
            if (Plant.Control is PlantControl parent)
            {
                Control = new NodeControl(parent, this);
                parent.SuspendLayout();
                parent.Controls.Add(Control);
                parent.ResumeLayout(false);
            }
            return Control;
        }

        /// <summary>
        /// Mark as deleted, remove dependent controllers
        /// </summary>
        public void Delete()
        {
            if (Plant.Control is PlantControl parent)
            {
                parent.SuspendLayout();
                parent.Controls.Remove(Control);
                parent.ResumeLayout(false);
                Control.Dispose();
            }
        }
        public virtual void SaveXml(XmlDocument doc)
        {
            var node = doc.CreateElement("Node");
            node.SetAttribute("type", Type.ToString());
            node.SetAttribute("id", Id.ToString());
            doc.AppendChild(node);
        }
    }

    /// <summary>
    /// A node that transform some inputs in some outputs
    /// </summary>
    public class Processor : Node
    {
        public override PlantNodeType Type => PlantNodeType.Processor;

        internal Processor(Plant plant, string name = "processor") : base(plant, name)
        {

        }

        public override NodeControl CreateController()
        {
            Control = null;
            if (Plant?.Control is PlantControl parent)
            {
                Control = new Controls.ProcessorControl(parent, this);
                parent.SuspendLayout();
                parent.Controls.Add(Control);
                parent.ResumeLayout(false);
            }
            return Control;
        }

        public override void SaveXml(XmlDocument doc)
        {
            base.SaveXml(doc);
        }

    }

    /// <summary>
    /// A node for only one resource that can have one input and one output, can be used as generator for infinite resources
    /// or an infinite storage or a flow rate graph
    /// </summary>
    public class Storage : Node
    {
        public override PlantNodeType Type => PlantNodeType.Storage;
        internal Storage(Plant plant, string name = "storage") : base(plant, name)
        {

        }

        public override NodeControl CreateController()
        {
            Control = null;
            if (Plant?.Control is PlantControl parent)
            {
                Control = new StorageControl(parent, this);
                parent.SuspendLayout();
                parent.Controls.Add(Control);
                parent.ResumeLayout(false);
            }
            return Control;
        }
        public override void SaveXml(XmlDocument doc)
        {
            base.SaveXml(doc);
        }
    }

    /// <summary>
    /// A node for only one resource that can merge more than one inputs and or split it to more than one ouputs
    /// </summary>
    public class Balancer : Node
    {
        public override PlantNodeType Type => PlantNodeType.Balancer;
        internal Balancer(Plant plant, string name = "balancer") : base(plant, name)
        {

        }
        public override NodeControl CreateController()
        {
            Control = null;
            if (Plant?.Control is PlantControl parent)
            {
                Control = new Controls.BalancerControl(parent, this);
                parent.SuspendLayout();
                parent.Controls.Add(Control);
                parent.ResumeLayout(false);
            }
            return Control;
        }
        public override void SaveXml(XmlDocument doc)
        {
            base.SaveXml(doc);
        }

    }

    public class PlantNodes : IEnumerable<Node>
    {
        Plant root;
        List<Node> nodes;

        public PlantNodes(Plant root)
        {
            this.root = root;
            nodes = new List<Node>();
        }

        public void AddNode(Node node)
        {
            nodes.Add(node);
        }
        public void RemoveNode(Node node)
        {
            nodes.Remove(node);
        }


        public IEnumerator<Node> GetEnumerator()
        {
            foreach (var node in nodes) yield return node;
        }

        IEnumerator IEnumerable.GetEnumerator()=> GetEnumerator();
        
    }


    /// <summary>
    /// Main factory class
    /// </summary>
    public class Plant
    {
        public string Name { get; set; }
        /// <summary>
        /// Node list is the basic plant structure, each node rapresent a resource process
        /// </summary>
        public PlantNodes Nodes { get; }

        public PlantControl Control { get; }

        public Plant(string name)
        {
            Name = name;
            Nodes = new PlantNodes(this);
            Control = new PlantControl(this);
        }


        /// <summary>
        /// Create a plant's node and link to its controller
        /// </summary>
        public NODE CreateNode<NODE>(string name = null) where NODE : Node
        {
            object[] arg = new object[] { this, string.IsNullOrEmpty(name) ? Type.Missing : name };
            var constructorInfo = typeof(NODE).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.OptionalParamBinding,
                null,
                new Type[] { typeof(Plant), typeof(string) },
                null);
            
            var node = (NODE)constructorInfo.Invoke(arg);


            Nodes.AddNode(node);

            Control.ResumeLayout(true);

            return node;
        }

        public void RemoveNode(Node node)
        {
            Nodes.RemoveNode(node);
        }


        public void OnMouse(Point screen)
        {

        }


        /// <summary>
        /// Save Plant scene to xml file
        /// </summary>
        public void SaveXml(string xmlfile)
        {
            var doc = new XmlDocument();
            var plant = doc.CreateElement("Plant");
            plant.SetAttribute("name", Name);
            doc.AppendChild(plant);

            foreach (var node in Nodes) node.SaveXml(doc);

            File.Create(xmlfile).Dispose();
            doc.Save(xmlfile);
        }

        /// <summary>
        /// Load a new Plant scene from xml file
        /// </summary>
        public static Plant Load(string xmlfile)
        {
            var doc = new XmlDocument();
            doc.Load(xmlfile);
            if (doc.DocumentElement.Name != "Plant")
            {
                Console.WriteLine("Is not a plant xml");
                return null;
            }
            var plant = new Plant(doc.DocumentElement.GetAttribute("name"));

            foreach (XmlNode element in doc.DocumentElement.ChildNodes)
            {

            }

            return plant;

        }
    }
}
