using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public partial class ItemList : ListBox
    {
        public ItemList()
        {
            Items.Clear();
            Items.AddRange(new object[] { "Factory", "Container", "Balancer" });
        }
    }
}
