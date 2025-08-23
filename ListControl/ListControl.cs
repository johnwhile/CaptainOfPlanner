
using System;
using System.Windows.Forms;

namespace CaptainOfPlanner.ListControl
{
    public partial class ListControl : UserControl
    {
        public Window Main;
        public FactoryPlant Plant => Main.Plant;

        public ListControl()
        {
            InitializeComponent();

            itemList.Items.AddRange(new object[] 
            {
                PlantNodeType.Processer,
                PlantNodeType.Balancer,
                PlantNodeType.Container
            });

            itemList.DoubleClick += InsertAction;

        }

        private void InsertAction(object sender, EventArgs e)
        {
            if (!Enum.TryParse(itemList.SelectedItem?.ToString(), out PlantNodeType type)) return;

            var node = Main.Plant.CreateNode(type);

            if (node == null) return;

            var nodecontroller = Main.plantViewer.AddPlantNode(node);
        }
    }
}
