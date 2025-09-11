using System;
using System.Xml;


namespace CaptainOfPlanner
{
    public class Processor : Node
    {
        public override NodeType Type => NodeType.Processor;

        Recipe recipe = Recipe.Empty;

        /// <summary>
        /// It is equivalent to the number of processes needed to process a certain input flow 
        /// relative to the maximum possible output flow.
        /// </summary>
        public float Efficiency { get; set; } = 1;

        /// <summary>
        /// Set a new recipe invalidate all inputs and outpusts
        /// </summary>
        public Recipe Recipe
        {
            get => recipe;
            set
            {
                if (recipe != value)
                {
                    InLinks.Clear();
                    OutLinks.Clear();

                    if (value != null)
                    {
                        for (int i = 0; i < value.Inputs.Length; i++)
                            InLinks.Add(new Link(this, LinkType.Input, value.Inputs[i], value.InCount[i]));
                        for (int i = 0; i < value.OutPuts.Length; i++)
                            OutLinks.Add(new Link(this, LinkType.Output, value.OutPuts[i], value.OutCount[i]));
                    }
                }
                recipe = value;
            }
        }

        public Processor(Plant plant, string name = "processor") :
            base(plant, string.IsNullOrEmpty(name) ? "processor" : name)
        {
        }

        public override void UpdateOutFlowRate()
        {

        }

        public override void UpdateFlowRate()
        {
            float t = 60f / recipe.Time;
            //apply flow for not linked inputs

            
            
            
            
            //compute consume
            foreach (var current in InLinks)
            {
                float min = float.MaxValue;
                float current_quantity = current.Quantity;
                foreach (var item in InLinks)
                {
                    float ratio =  current_quantity / item.Quantity;
                    float flow = item.Entering * ratio * t;
                    min = Math.Min(min, flow);
                }
                current.Forward = min;
            }

            //all output can be calculated from first input item consumed
            float first_quantity = InLinks.First.Quantity;
            float first_consumed = InLinks.First.Forward;

            foreach (var current in OutLinks)
            {
                current.Forward = first_consumed * current.Quantity / first_quantity;
            }
        }


        #region Read/Write
        protected override void CompleteWritingXml(XmlElement node)
        {
            node.SetAttribute("recipe", recipe.Encoded);
        }
        protected override void CompleatReadingXml(XmlElement element)
        {
            string encode = element.GetAttribute("recipe");

            if (!RecipesManager.Recipes.TryGetByEncoded(encode, out recipe))
            {
                Console.WriteLine("ERROR unknow recipe in xml processor");
                return;
            }

            // Add missing or unlinked inputs and output not saved into xml
            foreach (var input in recipe.InputCollection)
                if (!InLinks.Find(input.Item1, out Link link))
                    InLinks.Add(new Link(this, LinkType.Input, input.Item1, input.Item2));
                else
                    link.Quantity = input.Item2;

            foreach (var output in recipe.OutputCollection)
                if (!OutLinks.Find(output.Item1, out Link link))
                    OutLinks.Add(new Link(this, LinkType.Output, output.Item1, output.Item2));
                else
                    link.Quantity = output.Item2;
        }
        #endregion
    }
}
