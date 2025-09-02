
using System;
using System.Windows.Forms;

namespace CaptainOfPlanner.NewControls
{
    public partial class ListControl : UserControl
    {
        public ListControl()
        {
            InitializeComponent();

            itemList.Items.AddRange(new object[] 
            {
                NodeType.Processor,
                NodeType.Balancer,
                NodeType.Storage
            });

            itemList.DoubleClick += InsertNode;

        }

        private void InsertNode(object sender, EventArgs e)
        {
            if (!Enum.TryParse(itemList.SelectedItem?.ToString(), out NodeType type)) return;

            if (Parent is Window window)
                window.Manager.Plant.CreateNode(type);
        }
    }
}
