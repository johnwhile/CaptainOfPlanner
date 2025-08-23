using System;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public partial class Window : Form
    {
        public FactoryPlant Plant;
        Timer timer;

        public Window()
        {
            InitializeComponent();
            
            listControl.Main = this;
            plantViewer.Main = this;
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
            plantViewer.Refresh();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Plant = new FactoryPlant();

            ResourcesManager.Load("Resource\\Resources.xml");
            RecipesManager.Load("Resource\\Recipes.xml");
            //ResouceManager.Save("Resouces_out.xml", SaveResourceOption.SortByOrigin);

        }
    }
}
