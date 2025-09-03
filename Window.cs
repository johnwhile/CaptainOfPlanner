using System;
using System.Windows.Forms;

namespace CaptainOfPlanner.NewControls
{
    public partial class Window : Form
    {
        public ControlManager Manager;

        public Window()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ResourcesManager.Load("Resource\\Resources.xml");
            RecipesManager.Load("Resource\\Recipes.xml");
            //ResouceManager.Save("Resouces_out.xml", SaveResourceOption.SortByOrigin);

            Manager = new ControlManager(plantControl);
            Manager.Plant = new Plant("MyPlant");
        }

        private void SaveMenu_Click(object sender, EventArgs e)
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

        private void OpenMenu_Click(object sender, EventArgs e)
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
    }
}
