using System;
using System.Xml;

namespace CaptainOfPlanner
{
    /// <summary>
    /// A node for only one resource that can merge more than one inputs and or split it to more than one ouputs
    /// </summary>
    public class Storage : Node
    {
        public override NodeType Type => NodeType.Storage;
        Resource resource = Resource.Undefined;
        public Resource Resource
        {
            get => resource;
            set
            {
                if (!resource.IsCompatible(value))
                {
                    InLinks.Clear();
                    OutLinks.Clear();

                    InLinks.Add(new Link(this, LinkType.Input, value));
                    OutLinks.Add(new Link(this, LinkType.Output, value));

                }
                resource = value;
            }
        }

        public Storage(Plant plant, string name = "storage") :
            base(plant, string.IsNullOrEmpty(name) ? "storage" : name)
        {
            InLinks.Add(new Link(this, LinkType.Input, Resource.Undefined));
            OutLinks.Add(new Link(this, LinkType.Output, Resource.Undefined));
        }

        public override void UpdateOutFlowRate()
        {
            
        }

        public override void UpdateFlowRate()
        {
            foreach (var link in InLinks)
            {
                link.Forward = 0;
            }
            foreach (var link in OutLinks)
            {
                link.Forward = float.PositiveInfinity;
            }
        }


        #region Read/Write
        protected override void CompleteWritingXml(XmlElement node)
        {
            node.SetAttribute("resource", resource);
        }
        protected override void CompleatReadingXml(XmlElement element)
        {
            if (!ResourcesManager.TryGetResource(element.GetAttribute("resource"), out resource))
                Console.WriteLine("ERROR unknow resource in xml balancer");

            // cut in case something wrong
            while (InLinks.Count > 1) InLinks.Remove(InLinks.Last);
            while (OutLinks.Count > 1) OutLinks.Remove(InLinks.Last);

            if (InLinks.Count == 0) InLinks.Add(new Link(this, LinkType.Input, resource));
            if (OutLinks.Count == 0) OutLinks.Add(new Link(this, LinkType.Output, resource));

        }
        #endregion
    }
}
