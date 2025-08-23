using System.Drawing;

namespace CaptainOfPlanner
{
	public class ProcessorNode : PlantNode
	{
        public Recipe Recipe;

        public ProcessorNode(FactoryPlant plant, string name = "Processor") : base(plant, name)
        {

        }
    };
}
