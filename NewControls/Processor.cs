using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace CaptainOfPlanner.NewControls
{

    public class Processor : Node
    {
        public override NodeType Type => NodeType.Processor;

        public Processor(Plant plant, string name = "processor") : 
            base(plant, string.IsNullOrEmpty(name) ? "processor" : name)
        {

        }

        protected override void LoadXml(XmlElement element, Dictionary<int, Link> ToResolve)
        {
            string encode = element.GetAttribute("recipe");
            
            if (RecipesManager.Recipes.TryGetByEncoded(encode, out Recipe recipe))
            {
                foreach (XmlElement xmlink in element.ChildNodes)
                {
                    /*
                    string resource = xmlink.GetAttribute("res");

                    
                    LinkCollection collection;
                    switch (xmlink.Name)
                    {
                        case "Input": collection = node.Inputs; break;
                        case "Output": collection = node.Outputs; break;
                        default: continue;
                    }

                    if (collection.Find(resource, out Link link))
                    {
                        if (!int.TryParse(xmlink.GetAttribute("id"), out link.xml_id))
                            link.xml_id = -1;
                        if (!int.TryParse(xmlink.GetAttribute("linkid"), out link.xml_linked_id))
                            link.xml_linked_id = -1;

                        if (link.xml_id > 0) ToResolve.Add(link.xml_id, link);
                    }*/
                }
            }
            else
            {
                Console.WriteLine("ERROR unknow recipe in xml element");
            }
        }
    }
}
