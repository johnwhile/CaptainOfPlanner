using System;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public class StorageControl : NodeControl
    {
        Storage storage;
        public ComboBox comboResource { get; }

        public override Node Node => storage;

        public StorageControl(Storage node) : base(node)
        {
            SuspendLayout();

            storage = node;
            NodeColor = ColorTranslator.FromHtml("#FF66FF");
            Name = "StorageCtrl";

            comboResource = new ComboBox();

            foreach (var resource in ResourcesManager.Resources)
                comboResource.Items.Add(resource.Name);

            comboResource.DropDownWidth = 20;
            comboResource.Location = new Point(2, HeaderHeight + 2);
            comboResource.Size = new Size(Width - 4, 20);
            comboResource.Text = "-- set resource --";
            comboResource.SelectedValueChanged += ResourceSelectionChanged;

            Controls.Add(comboResource);

            RemoveLinkControls();
            CreateLinkControls(storage.Inputs, storage.Outputs);
            Invalidate();

            ResumeLayout();
        }

        private void ResourceSelectionChanged(object sender, EventArgs e)
        {
            int index = comboResource.SelectedIndex;
            SuspendLayout();
            if (index > -1 && ResourcesManager.TryGetResource((string)comboResource.Items[index], out Resource resource))
            {
                if (!storage.Resource.IsCompatible(resource))
                {
                    storage.Resource = resource;
                    RemoveLinkControls();
                    CreateLinkControls(storage.Inputs, storage.Outputs);
                    Invalidate();
                }
            }
            ResumeLayout();

        }
    }
}
