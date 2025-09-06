using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public partial class Window : Form
    {
        public bool IsDebug =
#if DEBUG
            true;
#else
            false;
#endif
        public ControlManager Manager;

        public Window()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsDebug)
            {
                ResourcesManager.Load("Resource\\Resources_Debug.xml");
                RecipesManager.Load("Resource\\Recipes_Debug.xml");
            }
            else
            {
                ResourcesManager.Load("Resource\\Resources.xml");
                RecipesManager.Load("Resource\\Recipes.xml");
            }
            //ResouceManager.Save("Resouces_out.xml", SaveResourceOption.SortByOrigin);

            Manager = new ControlManager(plantControl);
            var plant = new Plant("MyPlant");
            if (!plant.Load("C:\\Users\\Administrator\\Desktop\\myplant.xml"))
                MessageBox.Show("Error loading plant");
            Manager.Plant = plant;
        }
        private void MenuSave_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                Plant plant = Manager.Plant;

                dialog.FileName = plant.Name + ".xml";
                dialog.Filter = "plant blueprint (*.xml)|*.xml";
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    plant.SaveXml(dialog.FileName);
                }
            }
        }
        private void MenuOpen_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "plant blueprint (*.xml)|*.xml";
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var plant = new Plant("MyPlant");

                    if (!plant.Load(dialog.FileName))
                        MessageBox.Show("Error loading plant");
                    Manager.Plant = plant;
                }
            }
        }
        private void MenuRun_Click(object sender, EventArgs e)
        {
            Manager.Plant.RUN();
        }
        private void MenuAddProcessor_Click(object sender, EventArgs e) =>
            Manager.Control.AddNewNodeAndControler(NodeType.Processor);
        private void MenuAddBalancer_Click(object sender, EventArgs e) =>
            Manager.Control.AddNewNodeAndControler(NodeType.Balancer);
        private void MenuAddStorage_Click(object sender, EventArgs e) =>
            Manager.Control.AddNewNodeAndControler(NodeType.Storage);
    }
}
