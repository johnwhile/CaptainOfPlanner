using System.Drawing;

namespace CaptainOfPlanner
{
    public class ContainerNode : PlantNode
    {
        public ContainerNode(FactoryPlant plant, string name = "Container") : base(plant, name)
        {
            backColor = Color.LightGreen;
        }
    }

}
