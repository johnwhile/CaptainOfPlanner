using System;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public class BalanceControl : NodeControl
    {
        Balancer balancer;
        Button ButtonIncrease;
        Button ButtonDecrease;
        ComboBox comboResource;

        public override Node Node => balancer;

        public BalanceControl(Balancer node) : base(node)
        {
            SuspendLayout();

            balancer = node;
            NodeColor = ColorTranslator.FromHtml("#FFFF66");
            Name = "BalancerCtrl";

            comboResource = new ComboBox();

            foreach (var resource in ResourcesManager.Resources)
                comboResource.Items.Add(resource.Name);

            comboResource.DropDownWidth = 20;
            comboResource.Location = new Point(2, HeaderHeight + 2);
            comboResource.Size = new Size(Width - 20-20-8, 20);
            comboResource.Text = "-- set resource --";
            comboResource.SelectedValueChanged += ResourceSelectionChanged;

            ButtonIncrease = new Button()
            {
                Size = new Size(20, 20),
                Text = "+",
                Font = ControlManager.ArialBold6,
                Location = new Point(comboResource.Right+2, HeaderHeight + 2),
                
            };
            ButtonDecrease = new Button()
            {
                Size = new Size(20, 20),
                Text = "-",
                Font = ControlManager.ArialBold6,
                Location = new Point(ButtonIncrease.Right+2, HeaderHeight + 2)
            };
            ButtonIncrease.Click += ButtonClick;
            ButtonDecrease.Click += ButtonClick;
            Controls.Add(comboResource);
            Controls.Add(ButtonIncrease);
            Controls.Add(ButtonDecrease);

            UpdateLinkControls();

            ResumeLayout();
        }

        private void ButtonClick(object sender, System.EventArgs e)
        {
            var button = (Button)sender;
            if (button.Text == "+") balancer.Increase();
            if (button.Text == "-") balancer.Decrease();
            UpdateLinkControls();

        }

        void UpdateLinkControls()
        {
            RemoveLinkControls();
            CreateLinkControls(comboResource.Bottom +2, balancer.Inputs, balancer.Outputs);
            Invalidate();
        }

        private void ResourceSelectionChanged(object sender, EventArgs e)
        {
            int index = comboResource.SelectedIndex;
            SuspendLayout();
            if (index > -1 && ResourcesManager.TryGetResource((string)comboResource.Items[index], out Resource resource))
            {
                if (!balancer.Resource.IsCompatible(resource))
                {
                    balancer.Resource = resource;
                    RemoveLinkControls();
                    CreateLinkControls(comboResource.Bottom + 2, balancer.Inputs, balancer.Outputs);
                    Invalidate();
                }
            }
            ResumeLayout();

        }
    }
}
