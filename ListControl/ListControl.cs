
using System;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public partial class ListControl : UserControl
    {
        public Window Main;
        public Plant Plant => Main.Plant;

        public ListControl()
        {
            InitializeComponent();

            itemList.Items.AddRange(new object[] 
            {
                NodeType.Generic,
                NodeType.Processor,
                NodeType.Balancer,
                NodeType.Storage
            });

            itemList.DoubleClick += InsertNode;

        }

        private void InsertNode(object sender, EventArgs e)
        {
            if (!Enum.TryParse(itemList.SelectedItem?.ToString(), out NodeType type)) return;

            Plant Plant = Main.Plant;
            Node node = Plant.CreateNode(type);
        }
    }
}
