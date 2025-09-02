using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptainOfPlanner
{
    public class NodeCollection : IEnumerable<Node>
    {
        Plant plant;
        List<Node> nodes;

        public NodeCollection(Plant plant)
        {
            this.plant = plant;
            nodes = new List<Node>();
        }
        public void Clear()
        {
            while (nodes.Count > 0) RemoveNode(nodes[nodes.Count - 1]);
        }
        public void AddNode(Node node)
        {
            if (node == null) return;
            nodes.Add(node);
            plant.Control.AddNodeControl(node.Control);
        }
        public void RemoveNode(Node node)
        {
            if (node == null) return;
            nodes.Remove(node);
            node.Inputs.Clear();
            node.Outputs.Clear();
            plant.Control.RemoveNodeControl(node.Control);
        }


        public IEnumerator<Node> GetEnumerator()
        {
            foreach (var node in nodes) yield return node;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    }
}
