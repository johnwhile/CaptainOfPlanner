using System;
using System.Xml;


namespace CaptainOfPlanner
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
        protected override void CompleteWritingXml(XmlElement node)
        {
            node.SetAttribute("recipe", recipe.Encoded);
        }
        protected override void CompleatReadingXml(XmlElement element)
        {
            string encode = element.GetAttribute("recipe");
            
            if (!RecipesManager.Recipes.TryGetByEncoded(encode, out recipe))
                Console.WriteLine("ERROR unknow recipe in xml processor");


            // Add missing or unlinked inputs and output not saved into xml
            foreach(var input in recipe.Inputs)
                if (!Inputs.Find(input.Resource.Name, out _)) 
                    Inputs.Add(new Link(this, LinkType.Input, input));

            foreach (var output in recipe.OutPuts)
                if (!Outputs.Find(output.Resource.Name, out _))
                    Outputs.Add(new Link(this, LinkType.Output, output));
        }
    }
}
