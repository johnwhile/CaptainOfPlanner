using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CaptainOfPlanner
{
    public class Network : IEnumerable<Node>
    {
        NodeForwardEnumerator forward;
        public Node First { get; }

        private Network(Node first)
        {
            First = first;
            forward = new NodeForwardEnumerator(first);
        }

        public IEnumerator<Node> GetEnumerator() => forward;
        IEnumerator IEnumerable.GetEnumerator()=> GetEnumerator();

        /// <summary>
        /// a plant can have more than one separate and indipendent network
        /// </summary>
        public static List<Network> GetNetworks(Plant plant)
        {
            var remains = new HashSet<Node>(plant);
            var networks = new List<Network>();

            //Get the most backward node
            while (remains.Count > 0)
            {
                var last = remains.Last();
                var backward = new NodeBackwardEnumerator(last);
                foreach (var node in backward) last = node;
                foreach (var node in backward.Visited) remains.Remove(node);
                networks.Add(new Network(last));
            }
            return networks;
        }
    }
    public abstract class NodeEnumerator : IEnumerator<Node>, IEnumerable<Node>
    {
        protected Node root;
        protected Node current;
        protected Stack<Node> stack;
        
        public readonly HashSet<Node> Visited;
        public readonly List<Node> WithoutInputs;

        public NodeEnumerator(Node root)
        {
            stack = new Stack<Node>();
            Visited = new HashSet<Node>();
            this.root = root;
            Reset();
        }
        public Node Current => current;
        object IEnumerator.Current => Current;

        public void Dispose()
        {
            stack.Clear();
            Visited.Clear();
            current = null;
        }
        public abstract bool MoveNext();
        public void Reset()
        {
            stack.Clear();
            Visited.Clear();
            stack.Push(root);
        }
        public IEnumerator<Node> GetEnumerator()
        {
            Reset();
            while (MoveNext())
                yield return Current;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public class NodeForwardEnumerator : NodeEnumerator
    {
        public NodeForwardEnumerator(Node root) : base(root)
        {
        }

        public override bool MoveNext()
        {
            if (stack.Count == 0) return false;

            current = stack.Pop();
            Visited.Add(current);

            foreach (var output in current.OutLinks)
            {
                if (output.Linked != null && !Visited.Contains(output.Linked.Owner))
                {
                    if (current == output.Linked.Owner) throw new Exception("detect link to same node");
                    stack.Push(output.Linked.Owner);
                }
                    
            }

            return true;
        }
    }
    public class NodeBackwardEnumerator : NodeEnumerator
    {
        public NodeBackwardEnumerator(Node root) : base(root)
        {
        }

        public override bool MoveNext()
        {
            if (stack.Count == 0) return false;

            current = stack.Pop();
            Visited.Add(current);

            foreach (var input in current.InLinks)
            {
                if (input.Linked != null && !Visited.Contains(input.Linked.Owner))
                {
                    if (current == input.Linked.Owner) throw new Exception("detect link to same node");
                    stack.Push(input.Linked.Owner);
                }  
            }

            return true;
        }
    }
}
