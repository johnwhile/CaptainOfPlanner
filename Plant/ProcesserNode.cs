using System.Drawing;

namespace CaptainOfPlanner
{
	public class ProcesserNode : PlantNode
	{
        public Recipe Recipe;


        public ProcesserNode(FactoryPlant plant, string name = "Processer") : base(plant, name)
        {
            backColor = Color.LightBlue;
        }
    };
}
