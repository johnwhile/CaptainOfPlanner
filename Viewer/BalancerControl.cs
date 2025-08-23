using System.Drawing;

namespace CaptainOfPlanner
{
    public class BalancerControl : PlantNodeControl<BalancerNode>
    {
        public BalancerControl(BalancerNode plantnode) : base(plantnode)
        {
            TitleColor = ColorTranslator.FromHtml("#B9FF83");
        }
    }
}
