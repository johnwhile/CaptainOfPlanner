using System.Drawing;

namespace CaptainOfPlanner
{
    public class ContainerNode : PlantNode
    {
        public Resource Resource;

        public ContainerNode(FactoryPlant plant, string name = "Container") : base(plant, name)
        {
        }
    }


}
