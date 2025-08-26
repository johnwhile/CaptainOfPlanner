using System.Collections.Generic;

namespace CaptainOfPlanner
{
    public abstract class PlantNode
	{
        static int instance_counter = 0;

        /// <summary>
        /// for derived class
        /// </summary>
        public string ClassName => GetType().Name;

        public PlantNodeBaseControl viewControl;

        public readonly FactoryPlant Plant;
		public readonly string Name;

        public List<InputNode> Inputs;
        public List<OutputNode> Outputs;

        /// <summary>
        /// Create new input linker and add to inputlist
        /// </summary>
        public InputNode CreateInput()
        {
            var node = new InputNode(this);
            Inputs.Add(node);
            return node;
        }
        public OutputNode CreateOutput()
        {
            var node = new OutputNode(this);
            Outputs.Add(node);
            return node;
        }
        public PlantNode(FactoryPlant plant, string name = "node")
        {
            Name = instance_counter++.ToString() + "_" + name;
            Plant = plant;
            Inputs = new List<InputNode>();
            Outputs = new List<OutputNode>();

            plant.AddFactoryNode(this);
        }
    }


}
