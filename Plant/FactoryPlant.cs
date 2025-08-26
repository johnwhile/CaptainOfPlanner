
using System.Collections;
using System.Collections.Generic;

namespace CaptainOfPlanner
{
    public enum PlantNodeType
    {
        Generic,
        Processor,
        Balancer,
        Storage
    }

    public class FactoryPlant : IEnumerable<PlantNode>
    {
        List<PlantNode> Nodes = new List<PlantNode>();


        public FactoryPlant()
        {


        }

        public PlantNode CreateNode(PlantNodeType type)
        {
            switch(type)
            {
                case PlantNodeType.Processor: return new ProcessorNode(this);
                case PlantNodeType.Balancer: return new BalancerNode(this);
                case PlantNodeType.Storage: return new ContainerNode(this);
                default: return null;
            }
        }


        public void AddFactoryNode(PlantNode node)
        {
            Nodes.Add(node);
        }

        public void RemoveFactoryNode(PlantNode node)
        {
            Nodes.Remove(node);
        }

        public IEnumerator<PlantNode> GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
