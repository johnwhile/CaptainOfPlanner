using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaptainOfPlanner.NewControls
{
    [DebuggerDisplay("Count = {Count}")]
    public class NodeCollection : IEnumerable<Node>
    {
        Plant plant;
        List<Node> nodes;

        public NodeCollection(Plant plant)
        {
            this.plant = plant;
            nodes = new List<Node>();
        }

        public int Count => nodes.Count;

        public void Clear()
        {
            while (nodes.Count > 0) RemoveNode(nodes[nodes.Count - 1]);
        }
        public void AddNode(Node node)
        {
            if (node == null) return;
            nodes.Add(node);
        }
        public void RemoveNode(Node node)
        {
            if (node == null) return;
            nodes.Remove(node);
        }


        public IEnumerator<Node> GetEnumerator()
        {
            foreach (var node in nodes) yield return node;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
