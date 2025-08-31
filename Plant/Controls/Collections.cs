using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptainOfPlanner.Controls
{
    public class LinkCollection : IEnumerable<LinkControl>
    {
        NodeControl owner;
        LinkType type;

        public LinkCollection(NodeControl owner, LinkType type)
        {
            this.owner = owner;
            this.type = type;
        }

        public LinkControl Last
        {
            get
            {
                LinkControl last = null;
                foreach (var link in this) last = link;
                return last;
            }
        }


        public IEnumerator<LinkControl> GetEnumerator()
        {
            var collection = type == LinkType.Input ? owner.Node.Inputs : owner.Node.Outputs;
            foreach (var control in owner.Controls)
            {
                if (control is LinkControl linker)
                    if (collection.Contains(linker.Node))
                        yield return linker;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
