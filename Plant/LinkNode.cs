using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CaptainOfPlanner.Plant
{
    public abstract class LinkNode
    {
        public ResourceCount ResourceCount { get; set; }
        public readonly PlantNode Owner;

        protected LinkNode(PlantNode owner)
        {
            Owner = owner;
            ResourceCount = ResourceCount.Undefined;
        }

        public override string ToString()
        {
            return $"{Owner.ClassName} {ResourceCount}";
        }
    }

    public class InputNode : LinkNode
    {
        internal InputNode(PlantNode owner) : base(owner) { }
    }

    public class OutputNode : LinkNode
    {
        internal OutputNode(PlantNode owner) : base(owner) { }
    }
}

