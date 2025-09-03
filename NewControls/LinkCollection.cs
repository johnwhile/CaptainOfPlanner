using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CaptainOfPlanner.NewControls
{
    public class LinkCollection : IEnumerable<Link>
    {
        Node Owner;
        LinkType type;
        List<Link> list;

        public LinkCollection(Node owner, LinkType type)
        {
            Owner = owner;
            list = new List<Link>();
            this.type = type;
        }

        public bool Find(string resource, out Link link)
        {
            link = list.Find(x => x.ResourceCount.Resource.Name.CompareTo(resource) == 0);
            return link != null;
        }


        public Link Last => list.GetLast();


        public int Count => list.Count;
        public void Clear()
        {
            while (list.Count > 0) Remove(list.First());
            list.Clear();
        }

        public void Add(Link link)
        {
            list.Add(link);
        }

        public void AddRange(IEnumerable<Link> list)
        {
            foreach (Link link in list) Add(link);
        }

        public void Remove(Link link)
        {
            list.Remove(link);
        }

        public IEnumerator<Link> GetEnumerator() { foreach (var link in list) yield return link; }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
