
using System;
using System.Drawing;


namespace CaptainOfPlanner
{
    public partial class ContainerControl : PlantNodeBaseControl
    {

        public ContainerControl(ContainerNode plantnode) : base(plantnode)
        {
            TitleColor = ColorTranslator.FromHtml("#AECE95");
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            comboBox.Items.Clear();
            foreach (var item in ResourcesManager.ResourcesName) comboBox.Items.Add(item);
        }
    }
}
