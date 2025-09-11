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
                if (!resource.IsCompatible(value))
                {
                    foreach (var link in InLinks)
                    {
                        link.UnLink();
                        link.Resource = value;
                    }
                    foreach (var link in OutLinks)
                    {
                        link.UnLink();
                        link.Resource = value;
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
                InLinks.Add(new Link(this, LinkType.Input, Resource.Undefined));
                OutLinks.Add(new Link(this, LinkType.Output, Resource.Undefined));
            }
        }

        int InputCount => InLinks.Count;

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
        public override void UpdateOutFlowRate()
        {
            
        }
        public override void UpdateFlowRate()
        {
            float entering = 0;
            int entercount = 0;
            foreach (var link in InLinks)
                if (link.IsLinked)
                {
                    entering = link.Linked.Forward;
                    entercount++;
                }

            float exiting = 0;
            int exitcount = 0;
            foreach (var link in OutLinks)
                if (link.IsLinked)
                {
                    exiting += link.Linked.Forward;
                    exitcount++;
                }
            float avarage = exiting / exitcount;

        }


        public void Increase()
        {
            if (InLinks.Count == MAXLINKS - 1) return;
            InLinks.Add(new Link(this, LinkType.Input, resource));
            OutLinks.Remove(OutLinks.Last);
        }

        public void Decrease()
        {
            if (OutLinks.Count == MAXLINKS - 1) return;
            OutLinks.Add(new Link(this, LinkType.Output, resource));
            InLinks.Remove(InLinks.Last);
        }

        #region Read/Write
        protected override void CompleteWritingXml(XmlElement element)
        {
            element.SetAttribute("resource", resource);
            element.SetAttribute("inputs", TotalInputs.ToString());
        }

        protected override void CompleatReadingXml(XmlElement element)
        {
            if (!ResourcesManager.TryGetResource(element.GetAttribute("resource"), out resource))
                Console.WriteLine("ERROR unknow resource in xml balancer");

            if (!int.TryParse(element.GetAttribute("inputs"), out int preferedInputs)) preferedInputs = 4;

            // cut in case something wrong
            while (InLinks.Count > MAXLINKS - 1) InLinks.Remove(InLinks.Last);
            while (OutLinks.Count > (MAXLINKS - InLinks.Count)) OutLinks.Remove(OutLinks.Last);


            int missing = MAXLINKS - OutLinks.Count - InLinks.Count;
            int missing_i = missing - InLinks.Count - 1;
            int missing_o = missing - OutLinks.Count - 1;
            while (missing_i-- > 0) InLinks.Add(new Link(this, LinkType.Input, resource));
            while (missing_o-- > 0) OutLinks.Add(new Link(this, LinkType.Output, resource));

        }
        #endregion
    }

}
