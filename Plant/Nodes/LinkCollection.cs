using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using CaptainOfPlanner.Controls;

namespace CaptainOfPlanner
{
    public class LinkCollection : IEnumerable<Link>
    {
        Node Owner;
        List<Link> list;

        public LinkCollection(Node owner)
        {
            Owner = owner;
            list = new List<Link>();
        }

        public bool Find(string resource, out Link link)
        {
            link = list.Find(x => x.ResourceCount.Resource.Name.CompareTo(resource) == 0);
            return link != null;
        }


        public Link Last => list.GetLast();
            
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
            link.Dispose();
            if (link.Control.Parent is NodeControl parent)
                parent.RemoveLinkControl(link.Control);
        }

        public void SaveXml(XmlElement node)
        {
            foreach (var link in this) link.SaveXml(node);
        }


        public IEnumerator<Link> GetEnumerator() { foreach (var link in list) yield return link; }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
