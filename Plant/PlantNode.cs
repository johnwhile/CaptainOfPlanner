
using System.Drawing;

namespace CaptainOfPlanner
{
    public abstract class PlantNode
	{
        public PlantNodeBaseControl viewControl;
        protected Color backColor;

        public readonly FactoryPlant Plant;
		public readonly string Name;
        public Color BackColor => backColor;

        public PlantNode(FactoryPlant plant, string name = "node")
        {
            Name = name;
            Plant = plant;
            backColor = Color.Gray;
            plant.AddFactoryNode(this);
        }
    }


}
