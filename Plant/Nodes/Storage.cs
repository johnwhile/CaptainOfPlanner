using CaptainOfPlanner.Controls;
using System.Drawing;
using System.Xml;

namespace CaptainOfPlanner
{
    /// <summary>
    /// A node for only one resource that can have one input and one output, can be used as generator for infinite resources
    /// or an infinite storage or a flow rate graph
    /// </summary>
    public class Storage : Node
    {
        public override NodeType Type => NodeType.Storage;

        protected override NodeControl GenerateControl() => new StorageControl(this);

        internal Storage(Plant plant, string name) : base(plant, name ?? "storage")
        {

        }
    }


}
