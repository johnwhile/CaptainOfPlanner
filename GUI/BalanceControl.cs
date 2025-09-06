using System;
using System.Drawing;


namespace CaptainOfPlanner
{
    /// <summary>
    /// GUI interface for <see cref="Balancer"/> node
    /// </summary>
    public partial class BalanceControl : NodeControl
    {
        Balancer balancer;
        int currentSelectedResource = -1;
        public override Node Node => balancer;

        public BalanceControl(Balancer node = null, PlantControl owner = null) : base(node, owner)
        {
            NodeColor = ColorTranslator.FromHtml("#FFFF66");

            //fix layout
            Size = preferedsize;
            InitializeComponent();
            comboResource.Width = Width - 21 - 21 - 10;
            buttonIncrease.Location = new Point(Width - 21 - 21 - 4, 21);
            buttonDecrease.Location = new Point(Width - 21 - 4, 21);


            buttonIncrease.Font = ControlManager.ArialBold6;
            buttonDecrease.Font = ControlManager.ArialBold6;

            comboResource.DataSource = ResourcesManager.Resources;
            comboResource.DisplayMember = "Name";

            OffsetInput = new Vector2i(2, comboResource.Bottom + 5);
            OffsetOutput = new Vector2i(Width - LinkControl.PreferedSize.width - 2, comboResource.Bottom + 5);

            if (DesignMode || node == null) return;
            //set the resource
            comboResource.SelectedIndex = comboResource.FindString(node.Resource.Name);
            //build manualy
            balancer = node;
            UpdateLinkControls();

        }


        void UpdateLinkControls()
        {
            SuspendLayout();
            RemoveLinkControls();
            CreateLinkControls(balancer.Inputs, balancer.Outputs);
            ResumeLayout();
            Invalidate();
        }

        private void comboResource_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboResource.SelectedIndex;
            if (index == currentSelectedResource) return;
            currentSelectedResource = index;

            if (balancer == null) return;

            if (index > -1)
            {
                var resource = (Resource)comboResource.Items[index];
                if (!balancer.Resource.IsCompatible(resource))
                {
                    balancer.Resource = resource;
                    UpdateLinkControls();
                }
            }
        }

        private void buttonIncrease_Click(object sender, EventArgs e)
        {
            balancer.Increase();
            UpdateLinkControls();
        }

        private void buttonDecrease_Click(object sender, EventArgs e)
        {
            balancer.Decrease();
            UpdateLinkControls();
        }
    }
}
