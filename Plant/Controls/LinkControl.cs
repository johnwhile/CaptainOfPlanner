using System;
using System.Windows.Forms;

namespace CaptainOfPlanner.Controls
{

    /// <summary>
    /// Base control, define the resources linkers between Nodes
    /// </summary>
    public abstract class LinkControl : UserControl
    {

        protected LinkControl(NodeControl owner)
        {

        }


    }

    public class InputLinkControl : LinkControl
    {
        public InputLinkControl(NodeControl owner) : base(owner)
        {
        }
    }
    public class OutputLinkControl : LinkControl
    {
        public OutputLinkControl(NodeControl owner) : base(owner)
        {
        }
    }
}
