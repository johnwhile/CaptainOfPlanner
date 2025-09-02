using System;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner.NewControls
{
    public class ProcessControl : NodeControl
    {
        public ComboBox comboRecipe { get; }
        public ComboBox comboFilter { get; }
        public Processor Node { get; }

        public ProcessControl(Processor node) : base()
        {
            Node = node;
            NodeColor = ColorTranslator.FromHtml("#6666FF");
            Name = "ProcessorCtrl";

            comboRecipe = new ComboBox();
            comboFilter = new ComboBox();

            foreach (var resource in ResourcesManager.Resources)
                comboFilter.Items.Add(resource.Name);

            comboFilter.DropDownWidth = 20;
            comboFilter.Location = new Point(2, HeaderHeight + 2);
            comboFilter.Size = new Size(Width - 4, 20);
            comboFilter.Text = "-- filter resource --";
            comboFilter.SelectedValueChanged += FilterSelectionChanged;


            foreach (var recipe in RecipesManager.Recipes)
                comboRecipe.Items.Add(recipe.Display);

            comboRecipe.DropDownWidth = RecipesManager.MaxRecipesFormattedNameLenght * 6;
            comboRecipe.Location = new Point(2, HeaderHeight + 2 + comboFilter.Height);
            comboRecipe.Size = new Size(Width - 4, 20);
            comboRecipe.Text = "-- select recipe --";
            comboRecipe.SelectedValueChanged += RecipeSelectionChanged;
        }

        private void FilterSelectionChanged(object sender, EventArgs e)
        {

        }


        private void RecipeSelectionChanged(object sender, EventArgs e)
        {
            int index = comboRecipe.SelectedIndex;
        }
    }
}
