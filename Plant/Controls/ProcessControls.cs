using System;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner.Controls
{
    public class ProcessorControl : NodeControl
    {
        public ComboBox comboRecipe;
        public ComboBox comboFilter;
        public Processor node;

        public ProcessorControl(Processor node) : base(node)
        {
            MyDefaultSize.width = 150;
            HeaderColor = Color.FromArgb(160, 80, 40);

            int maxlength = RecipesManager.MaxRecipesFormattedNameLenght;

            comboRecipe = new ComboBox();
            comboFilter = new ComboBox();

            foreach (var resource in ResourcesManager.Resources)
                comboFilter.Items.Add(resource.Name);

            comboFilter.DropDownWidth = 20;
            comboFilter.Location = new Point(2, headerHeight + 2);
            comboFilter.Size = new Size(Width - 4, 20);
            comboFilter.Text = "-- filter resource --";
            comboFilter.SelectedValueChanged += FilterSelectionChanged;


            foreach (var recipe in RecipesManager.Recipes)
                comboRecipe.Items.Add(recipe.Display);

            comboRecipe.DropDownWidth = maxlength * 6;
            comboRecipe.Location = new Point(2, headerHeight + 2 + comboFilter.Height);
            comboRecipe.Size = new Size(Width - 4, 20);
            comboRecipe.Text = "-- select recipe --";
            comboRecipe.SelectedValueChanged += RecipeSelectionChanged;

            Controls.Add(comboRecipe);
        }


        public Vector2i GetOffsetLocation(LinkType type) =>
            new Vector2i(type == LinkType.Input ? 2 : Width - LinkControl.MyDefaultSize.width - 2, comboRecipe.Location.Y + comboRecipe.Height + 10);


        private void FilterSelectionChanged(object sender, EventArgs e)
        {

        }


        private void RecipeSelectionChanged(object sender, EventArgs e)
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
