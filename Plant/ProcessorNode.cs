using System.Drawing;

namespace CaptainOfPlanner
{
	public class ProcessorNode : PlantNode
	{
        Recipe recipe;

        /// <summary>
        /// Changing recipe cause invalidating all inputs and outputs
        /// </summary>
        public Recipe Recipe
        {
            get => recipe;
            set
            {
                if (recipe == value) return;
                recipe = value;

                Inputs.Clear();
                Outputs.Clear();

                foreach (var itemcount in recipe.Inputs)
                    CreateInput().ResourceCount = itemcount;
                foreach (var itemcount in recipe.OutPuts)
                    CreateOutput().ResourceCount = itemcount;

            }
        }

        public ProcessorNode(FactoryPlant plant, string name = "Processor") : base(plant, name)
        {

        }
    };
}
