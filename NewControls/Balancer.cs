using System;
using System.Xml;

namespace CaptainOfPlanner.NewControls
{
    /// <summary>
    /// A node for only one resource that can merge more than one inputs and or split it to more than one ouputs
    /// </summary>
    public class Balancer : Node
    {
        public override NodeType Type => NodeType.Balancer;

        Resource resource;
        Resource Resource
        {
            get => resource;
            set { resource = value; }
        }

        public Balancer(Plant plant, string name = "balancer") :
            base(plant, string.IsNullOrEmpty(name) ? "balancer" : name)
        {

        }

        protected override void LoadXml(XmlElement element)
        {
            if( ResourcesManager.TryGetResource(element.GetAttribute("res"), out Resource res))
            {
                Resource = res;

                foreach (XmlElement xmlink in element.ChildNodes)
                {
                    if (!ResourcesManager.TryGetResource(xmlink.GetAttribute("res"), out Resource res_link))
                    {
                        Console.WriteLine("ERROR unknow resource in xml balancer link collection");
                        break;
                    }

                    Link link;
                    switch (xmlink.Name)
                    {
                        case "Input":
                            link = new Link(this, LinkType.Input, new ResourceCount(resource, 0));
                            Inputs.Add(link);
                            break;
                        case "Output":
                            link = new Link(this, LinkType.Output, new ResourceCount(resource, 0));
                            Outputs.Add(link);
                            break;
                        default: continue;
                    }

                    if (!int.TryParse(xmlink.GetAttribute("id"), out link.xml_id))
                        link.xml_id = -1;
                    if (!int.TryParse(xmlink.GetAttribute("linkid"), out link.xml_linked_id))
                        link.xml_linked_id = -1;

                }
            }
            else
            {
                Console.WriteLine("ERROR unknow resource in xml balancer");
            }
        }
    }

}
