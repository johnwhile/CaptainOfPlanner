using System;
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

        void Test()
        {

            ResourcesManager.TryGetResource("A", out Resource A);
            ResourcesManager.TryGetResource("B", out Resource B);
            ResourcesManager.TryGetResource("C", out Resource C);
            ResourcesManager.TryGetResource("D", out Resource D);

            var inputs = new Resource[] { A, B, C };
            var incount = new byte[] { 1, 2, 4 };
            var outputs = new Resource[] {D };
            var outcount = new byte[] { 8 };
            var recipe = new Recipe(60, inputs, incount, outputs, outcount);

            var process = new Processor(null);
            process.Recipe = recipe;


            process.InLinks.Find("A", out Link a);
            process.InLinks.Find("B", out Link b);
            process.InLinks.Find("C", out Link c);
            process.OutLinks.Find("D", out Link d);

            a.Entering = 20;
            b.Entering = 10;
            c.Entering = 30;

            process.UpdateFlowRate();

            Console.WriteLine("a " + a.Forward);
            Console.WriteLine("b " + b.Forward);
            Console.WriteLine("c " + c.Forward);
            Console.WriteLine("d " + d.Entering);


        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            IconsManager.Load("Resource\\Icons.xml");
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
                    Manager.SaveControllerInfo();
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
            Manager.Control.Invalidate(true);
        }
        private void MenuAddProcessor_Click(object sender, EventArgs e) =>
            Manager.Control.AddNewNodeAndControler(NodeType.Processor);
        private void MenuAddBalancer_Click(object sender, EventArgs e) =>
            Manager.Control.AddNewNodeAndControler(NodeType.Balancer);
        private void MenuAddStorage_Click(object sender, EventArgs e) =>
            Manager.Control.AddNewNodeAndControler(NodeType.Storage);
    }
}
