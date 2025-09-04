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
                        Inputs.Add(new Link(this, LinkType.Input, new ResourceCount(value, 0)));
                        Outputs.Add(new Link(this, LinkType.Output, new ResourceCount(value, 0)));
                    }
                }
                resource = value;
            }
        }

        public Storage(Plant plant, string name = "storage") :
            base(plant, string.IsNullOrEmpty(name) ? "storage" : name)
        {

        }

        protected override void CompleteWritingXml(XmlElement node)
        {
            node.SetAttribute("res", resource.Name);
        }
        protected override void CompleatReadingXml(XmlElement element)
        {
            if (!ResourcesManager.TryGetResource(element.GetAttribute("res"), out resource))
                Console.WriteLine("ERROR unknow resource in xml balancer");
        }
    }
}
