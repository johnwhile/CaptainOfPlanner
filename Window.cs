using CaptainOfPlanner.Controls;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public partial class Window : Form
    {
        PlantControllersManager Manager;


        public Plant Plant;

        public Window()
        {
            InitializeComponent();
            listControl.Main = this;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ResourcesManager.Load("Resource\\Resources.xml");
            RecipesManager.Load("Resource\\Recipes.xml");
            //ResouceManager.Save("Resouces_out.xml", SaveResourceOption.SortByOrigin);

            Plant = new Plant("myplant");
            Plant.Control = plantControl;
            plantControl.Plant = Plant;

            Manager = new PlantControllersManager(plantControl, Plant);
        }

        private void SaveMenu_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.FileName = Plant.Name + ".xml";
                dialog.Filter = "plant blueprint (*.xml)|*.xml";
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Plant.SaveXml(dialog.FileName);
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
                    if (Plant != null) Plant.Dispose();
                    plantControl.Clear();
                    Plant = Plant.Load(dialog.FileName, plantControl);
                }
            }
        }
    }
}
