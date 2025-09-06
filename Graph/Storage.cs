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
                    Inputs.Clear();
                    Outputs.Clear();
                    if (!value.IsUndefined)
                    {
                        Inputs.Add(new Link(this, LinkType.Input, new ResourceCount(value)));
                        Outputs.Add(new Link(this, LinkType.Output, new ResourceCount(value)));
                    }
                }
                resource = value;
            }
        }

        public Storage(Plant plant, string name = "storage") :
            base(plant, string.IsNullOrEmpty(name) ? "storage" : name)
        {
            Inputs.Add(new Link(this, LinkType.Input, ResourceCount.Undefined));
            Outputs.Add(new Link(this, LinkType.Output, ResourceCount.Undefined));
        }

        protected override void CompleteWritingXml(XmlElement node)
        {
            node.SetAttribute("resource", resource.Name);
        }
        protected override void CompleatReadingXml(XmlElement element)
        {
            if (!ResourcesManager.TryGetResource(element.GetAttribute("resource"), out resource))
                Console.WriteLine("ERROR unknow resource in xml balancer");

            // cut in case something wrong
            while (Inputs.Count > 1) Inputs.Remove(Inputs.Last);
            while (Outputs.Count > 1) Outputs.Remove(Inputs.Last);

            if (Inputs.Count == 0) Inputs.Add(new Link(this, LinkType.Input, new ResourceCount(resource)));
            if (Outputs.Count == 0) Outputs.Add(new Link(this, LinkType.Output, new ResourceCount(resource)));

        }
    }
}
