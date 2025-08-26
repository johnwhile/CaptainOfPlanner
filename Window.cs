using CaptainOfPlanner.Controls;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public partial class Window : Form
    {
        Timer timer;

        public Plant Plant;

        public Window()
        {
            Plant = new Plant("myplant");
            plantControl = Plant.Control;


            InitializeComponent();
            
            listControl.Main = this;
            timer = new Timer();
            timer.Interval = 500;
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            timer.Stop();

        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            timer.Start();
        }   


        private void Timer_Tick(object sender, EventArgs e)
        {
            //plantViewer.Refresh();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ResourcesManager.Load("Resource\\Resources.xml");
            RecipesManager.Load("Resource\\Recipes.xml");
            //ResouceManager.Save("Resouces_out.xml", SaveResourceOption.SortByOrigin);

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
                    Plant = CaptainOfPlanner.Plant.Load(dialog.FileName);
                }
            }
        }
    }
}
