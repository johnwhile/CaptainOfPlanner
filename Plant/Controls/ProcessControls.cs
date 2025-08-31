using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner.Controls
{
    public class ProcessorControl : NodeControl
    {
        public ComboBox combox;

        public ProcessorControl(Processor node) : base(node)
        {
            MyDefaultSize.width = 150;
            HeaderColor = Color.FromArgb(160, 80, 40);

            int maxlength = RecipesManager.MaxRecipesFormattedNameLenght;

            combox = new ComboBox();
            combox.Items.Clear();

            foreach (var recipe in RecipesManager.Recipes)
                combox.Items.Add(recipe.Display);

            combox.DropDownWidth = maxlength * 6;
            combox.Location = new Point(2, headerHeight + 2);
            combox.Size = new Size(Width - 4, 20);
            combox.Text = "-- select recipe --";
            combox.SelectedValueChanged += SelectionChanged;

            Controls.Add(combox);
        }


        public Vector2i GetOffsetLocation(LinkType type) =>
            new Vector2i(type == LinkType.Input ? 2 : Width - LinkControl.MyDefaultSize.width - 2, combox.Location.Y + combox.Height + 10);

        private void SelectionChanged(object sender, EventArgs e)
        {
            var box = (ComboBox)sender;
            if (!(Node is Processor processor))
            {
                throw new ArgumentException("invalid cast");
            }

            int index = box.SelectedIndex;

            if (index <= 0)
            {
                processor.Inputs.Clear();
                processor.Outputs.Clear();
                return;
            }

            var recipe = RecipesManager.Recipes[index];
            if (processor.Recipe != recipe) processor.Recipe = recipe;

        }
    }




    public class BalancerControl : NodeControl
    {
        public BalancerControl(Balancer node) : base(node)
        {
            HeaderColor = Color.FromArgb(40, 160, 90);
        }
    }

    public class StorageControl : NodeControl
    {
        public StorageControl(Storage node) : base(node)
        {
            HeaderColor = Color.FromArgb(20, 60, 120);
        }
    }

}
