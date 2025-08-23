
using System.Drawing;

namespace CaptainOfPlanner
{
    public abstract class PlantNode
	{
        public PlantNodeBaseControl viewControl;

        public readonly FactoryPlant Plant;
		public readonly string Name;

        public PlantNode(FactoryPlant plant, string name = "node")
        {
            Name = name;
            Plant = plant;
            plant.AddFactoryNode(this);
        }
    }


}
