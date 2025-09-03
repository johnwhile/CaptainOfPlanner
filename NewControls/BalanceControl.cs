using System.Drawing;

namespace CaptainOfPlanner.NewControls
{
    public class BalanceControl : NodeControl
    {
        Balancer balancer;

        public override Node Node => balancer;

        public BalanceControl(Balancer node) : base(node)
        {
            balancer = node;
            NodeColor = ColorTranslator.FromHtml("#FFFF66");
            Name = "BalancerCtrl";
        }
    }
}
