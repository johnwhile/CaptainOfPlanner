using System;
using System.Xml;

namespace CaptainOfPlanner.NewControls
{
    /// <summary>
    /// A node for only one resource that can merge more than one inputs and or split it to more than one ouputs
    /// </summary>
    public class Storage : Node
    {
        public override NodeType Type => NodeType.Storage;

        Resource resource;
        Resource Resource
        {
            get => resource;
            set { resource = value; }
        }

        public Storage(Plant plant, string name = "storage") :
            base(plant, string.IsNullOrEmpty(name) ? "storage" : name)
        {

        }

        protected override void SaveDerivedXml(XmlElement node)
        {
            node.SetAttribute("res", resource.Name);
        }
        protected override void LoadDerivedXml(XmlElement element)
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

                    Link link = null;
                    switch (xmlink.Name)
                    {
                        case "Input":
                            if (Inputs.Count != 0) break;
                            link = new Link(this, LinkType.Input, new ResourceCount(resource, 0));
                            Inputs.Add(link);
                            break;
                        case "Output":
                            if (Outputs.Count != 0) break;
                            link = new Link(this, LinkType.Output, new ResourceCount(resource, 0));
                            Outputs.Add(link);
                            break;
                        default: link = null; continue;
                    }
                    if (link != null)
                    {
                        if (!int.TryParse(xmlink.GetAttribute("id"), out link.xml_id))
                            link.xml_id = -1;
                        if (!int.TryParse(xmlink.GetAttribute("linkid"), out link.xml_linked_id))
                            link.xml_linked_id = -1;
                    }
                }
            }
            else
            {
                Console.WriteLine("ERROR unknow resource in xml balancer");
            }
        }
    }

}
