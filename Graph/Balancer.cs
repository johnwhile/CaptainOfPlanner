using System;
using System.Xml;

namespace CaptainOfPlanner
{
    /// <summary>
    /// A node for only one resource that can merge more than one inputs and or split it to more than one ouputs
    /// </summary>
    public class Balancer : Node
    {
        const int MAXLINKS = 8;
        Resource resource = Resource.Undefined;

        public override NodeType Type => NodeType.Balancer;

        public Resource Resource
        {
            get => resource;
            set
            {
                if (value.IsUndefined) return;

                if (!resource.IsCompatible(value))
                {
                    foreach (var link in Inputs)
                    {
                        link.UnLink();
                        link.ResourceCount = new ResourceCount(value, 0);
                    }
                    foreach (var link in Outputs)
                    {
                        link.UnLink();
                        link.ResourceCount = new ResourceCount(value, 0);
                    }

                    resource = value;
                }
            }
        }

        public Balancer(Plant plant, string name = "balancer") :
            base(plant, string.IsNullOrEmpty(name) ? "balancer" : name)
        {
            for (int i = 0; i < 4; i++)
            {
                Inputs.Add(new Link(this, LinkType.Input, ResourceCount.Undefined));
                Outputs.Add(new Link(this, LinkType.Output, ResourceCount.Undefined));
            }
        }

        int InputCount => Inputs.Count;

        public int TotalInputs
        {
            get => InputCount;
            set
            {
                if (value > MAXLINKS - 1) value = MAXLINKS - 1;
                if (value < 1) value = 1;

                if (InputCount != value)
                {
                    while (value < InputCount) Decrease();
                    while (value > InputCount) Increase();
                }
            }
        }


        public void Increase()
        {
            if (Inputs.Count == MAXLINKS - 1) return;
            Inputs.Add(new Link(this, LinkType.Input, new ResourceCount(resource)));
            Outputs.Remove(Outputs.Last);
        }

        public void Decrease()
        {
            if (Outputs.Count == MAXLINKS - 1) return;
            Outputs.Add(new Link(this, LinkType.Output, new ResourceCount(resource)));
            Inputs.Remove(Inputs.Last);
        }

        protected override void CompleteWritingXml(XmlElement element)
        {
            element.SetAttribute("resource", resource.Name);
        }

        protected override void CompleatReadingXml(XmlElement element)
        {
            if (!ResourcesManager.TryGetResource(element.GetAttribute("resource"), out resource))
                Console.WriteLine("ERROR unknow resource in xml balancer");

            // cut in case something wrong
            while (Inputs.Count > MAXLINKS - 1) Inputs.Remove(Inputs.Last);
            while (Outputs.Count > (MAXLINKS - Inputs.Count)) Outputs.Remove(Outputs.Last);

            int missing = MAXLINKS - Outputs.Count - Inputs.Count;
            while (missing-- > 0) Inputs.Add(new Link(this, LinkType.Input, new ResourceCount(resource)));
            if (Outputs.Count == 0) Outputs.Add(new Link(this, LinkType.Output, new ResourceCount(resource)));

        }
    }

}
