using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace CaptainOfPlanner
{
    /// <summary>
    /// GUI interface for <see cref="Storage"/> node
    /// </summary>
    
    public partial class StorageControl : NodeControl
    {
        Storage storage;
        int currentSelectedResource = -1;
        public override Node Node => storage;


        public StorageControl() : this(null, null)
        {

        }

        public StorageControl(Storage node, PlantControl owner) : base(node, owner)
        {
            NodeColor = ColorTranslator.FromHtml("#FF66FF");
            Name = "StorageCtrl";

            //fix layout
            Size = preferedsize;
            InitializeComponent();
            comboResource.Width = Width - 7;

            comboResource.DataSource = ResourcesManager.Resources;
            comboResource.DisplayMember = "Name";

            OffsetInput = new Vector2i(2, comboResource.Bottom + 5);
            OffsetOutput = new Vector2i(Width - LinkControl.PreferedSize.width - 2, comboResource.Bottom + 5);
            Mirrored = node.Mirrored;

            if (DesignMode || node==null) return;

            //set the resource
            comboResource.SelectedIndex = comboResource.FindString(node.Resource);
            //build manualy
            storage = node;
            UpdateLinkControls();
        }

        protected override void RemoveThisNode() { storage.Plant.RemoveNode(storage); }


        void UpdateLinkControls()
        {
            SuspendLayout();
            RemoveLinkControls();
            CreateLinkControls(storage.InLinks, storage.OutLinks);
            ResumeLayout();
            Invalidate();
        }
        private void comboResource_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboResource.SelectedIndex;
            if (index == currentSelectedResource) return;
            currentSelectedResource = index;

            if (storage == null) return;

            if (index > -1)
            {
                var resource = (Resource)comboResource.Items[index];
                if (!storage.Resource.IsCompatible(resource))
                {
                    storage.Resource = resource;
                    UpdateLinkControls();
                }
            }
        }
    }
}
