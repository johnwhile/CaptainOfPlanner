using System;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner.Controls
{
    public class ProcessorControl : NodeControl
    {
        ComboBox combox;

        public Recipe Recipe
        {
            get { if (Node is Processor processor) return processor.Recipe; return null; }
            set { if (Node is Processor processor) processor.Recipe = value; }
        }

        public ProcessorControl(PlantControl owner, Processor node) : base(owner, node)
        {
            HeaderColor = Color.FromArgb(160, 80, 40);

            combox = new ComboBox();
            combox.Items.Clear();

            int maxlength = 0;
            foreach (var recipe in RecipesManager.Recipes)
            {
                maxlength = Math.Max(maxlength, recipe.ToString().Length);
                combox.Items.Add(recipe);
            }

            combox.DropDownWidth = maxlength * 6;
            combox.Location = new Point(2, headerHeight + 2);
            combox.Size = new Size(Width - 4, 20);
            combox.Text = "-- select recipe --";
            combox.SelectedValueChanged += SelectionChanged;

            Controls.Add(combox);
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            var box = (ComboBox)sender;

            int index = box.SelectedIndex;
            if (index < 0) { RemoveLinkers(); return; }

            if (box.Items[index] is Recipe recipe && Node is Processor processor && processor.Recipe != recipe)
            {
                processor.Recipe = recipe;
                RemoveLinkers();


            }
        }

        void UpdateLayout()
        {

        }


        void RemoveLinkers()
        {

        }
    }




    public class BalancerControl : NodeControl
    {
        public BalancerControl(PlantControl owner, Balancer node) : base(owner, node)
        {
            HeaderColor = Color.FromArgb(40, 160, 90);
        }
    }

    public class StorageControl : NodeControl
    {
        public StorageControl(PlantControl owner, Storage node) : base(owner, node)
        {
            HeaderColor = Color.FromArgb(20, 60, 120);
        }
    }

}
