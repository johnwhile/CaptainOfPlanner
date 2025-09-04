using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;

namespace CaptainOfPlanner.NewControls
{
    public class Processor : Node
    {
        public override NodeType Type => NodeType.Processor;
        Recipe recipe;
        public Recipe Recipe
        {
            get => recipe;
            set
            {
                if (recipe != value)
                {
                    Inputs.Clear();
                    Outputs.Clear();
                    if (value != null)
                    {
                        foreach (var input in value.Inputs) Inputs.Add(new Link(this, LinkType.Input, input));
                        foreach (var output in value.OutPuts) Outputs.Add(new Link(this, LinkType.Output, output));
                    }
                }
                recipe = value;
            }
        }
        
        public Processor(Plant plant, string name = "processor") : 
            base(plant, string.IsNullOrEmpty(name) ? "processor" : name)
        {

        }
        protected override void SaveDerivedXml(XmlElement node)
        {
            node.SetAttribute("recipe", recipe.Encoded);
        }
        protected override void LoadDerivedXml(XmlElement element)
        {
            string encode = element.GetAttribute("recipe");
            
            if (RecipesManager.Recipes.TryGetByEncoded(encode, out Recipe rec))
            {
                Recipe = rec;

                foreach (XmlElement xmlink in element.ChildNodes)
                {
                    string resource = xmlink.GetAttribute("res");

                    LinkCollection collection;

                    switch (xmlink.Name)
                    {
                        case "Input": collection = Inputs; break;
                        case "Output": collection = Outputs; break;
                        default: continue;
                    }
                    if (collection.Find(resource, out Link link))
                    {
                        if (!int.TryParse(xmlink.GetAttribute("id"), out link.xml_id))
                            link.xml_id = -1;
                        if (!int.TryParse(xmlink.GetAttribute("linkid"), out link.xml_linked_id))
                            link.xml_linked_id = -1;
                    }
                    else
                    {
                        Console.WriteLine($"ERROR the link {link} not found");
                    }
                }

            }
            else
            {
                Console.WriteLine("ERROR unknow recipe in xml processor");
            }
        }
    }
}
