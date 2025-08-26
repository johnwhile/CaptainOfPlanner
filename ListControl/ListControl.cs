
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
                PlantNodeType.Processor,
                PlantNodeType.Balancer,
                PlantNodeType.Storage
            });

            itemList.DoubleClick += InsertNode;

        }

        private void InsertNode(object sender, EventArgs e)
        {
            if (!Enum.TryParse(itemList.SelectedItem?.ToString(), out PlantNodeType type)) return;


            switch(type)
            {
                case PlantNodeType.Processor: Main.Plant.CreateNode<Processor>(); break;
                case PlantNodeType.Balancer: Main.Plant.CreateNode<Balancer>(); break;
                case PlantNodeType.Storage: Main.Plant.CreateNode<Storage>(); break;
            }
        }
    }
}
