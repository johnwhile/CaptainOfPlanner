using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CaptainOfPlanner
{
    public class Network : IEnumerable<Node>
    {
        Node Root;

        private Network()
        {

        }

        public IEnumerator<Node> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        /// <summary>
        /// a plant can have more than one separate and indipendent network
        /// </summary>
        public static List<Network> GetNetworks(Plant plant)
        {
            var remains = new HashSet<Node>(plant);
            var networks = new List<Network>();

            var backward = new NodeBackwardEnumerator(remains.Last());
            foreach (var node in backward)
            {
                Console.Write($"{node.Name} > ");
            }

            return networks;
        }

    }


    public abstract class NodeEnumerator : IEnumerator<Node>, IEnumerable<Node>
    {
        protected Node root;
        protected Node current;
        protected Stack<Node> stack;
        protected HashSet<Node> visited;

        public NodeEnumerator(Node root)
        {
            stack = new Stack<Node>();
            visited = new HashSet<Node>();
            this.root = root;
            Reset();
        }
        public Node Current => current;
        object IEnumerator.Current => Current;

        public void Dispose()
        {
            stack.Clear();
            visited.Clear();
            current = null;
        }
        public abstract bool MoveNext();
        public void Reset()
        {
            stack.Clear();
            visited.Clear();
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
            return false;
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
            visited.Add(current);

            foreach (var input in current.Inputs)
            {
                if (input.Linked != null && !visited.Contains(input.Linked.Owner))
                    stack.Push(input.Linked.Owner);
            }

            return true;
        }
    }
}
