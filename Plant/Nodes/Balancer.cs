using CaptainOfPlanner.Controls;
using System.Xml;

namespace CaptainOfPlanner
{
    /// <summary>
    /// A node for only one resource that can merge more than one inputs and or split it to more than one ouputs
    /// </summary>
    public class Balancer : Node
    {
        public override NodeType Type => NodeType.Balancer;

        protected override NodeControl GenerateControl() => new BalancerControl(this);

        internal Balancer(Plant plant, string name) : base(plant, name ?? "balancer")
        {

        }
    }

}
