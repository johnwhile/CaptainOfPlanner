using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public class ProcessControl : NodeControl
    {
        Processor processor;
        public ComboBox comboRecipe { get; }
        public ComboBox comboFilter { get; }
        public override Node Node => processor;

        public ProcessControl(Processor node) : base(node)
        {
            SuspendLayout();

            processor = node;
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

            PopulateComboRecipe(null);

            comboRecipe.DropDownWidth = RecipesManager.MaxRecipesFormattedNameLenght * 6;
            comboRecipe.Location = new Point(2, HeaderHeight + 2 + comboFilter.Height);
            comboRecipe.Size = new Size(Width - 4, 20);
            comboRecipe.Text = "-- select recipe --";
            comboRecipe.SelectedValueChanged += RecipeSelectionChanged;

            Controls.Add(comboFilter);
            Controls.Add(comboRecipe);

            RemoveLinkControls();
            CreateLinkControls(comboRecipe.Bottom+5, processor.Inputs, processor.Outputs);
            Invalidate();

            ResumeLayout();
        }


        void PopulateComboRecipe(Resource? resource)
        {
            List<Recipe> datasource = new List<Recipe>();

            Func<Recipe, bool> selector = recipe => recipe.Contains(resource.Value, out _);

            if (resource.HasValue)
                foreach (var recipe in RecipesManager.Recipes.Where(selector))
                    datasource.Add(recipe);
            else
                foreach (var recipe in RecipesManager.Recipes)
                    datasource.Add(recipe);

            comboRecipe.DataSource = datasource;
            comboRecipe.ValueMember = "Display";
        }


        private void FilterSelectionChanged(object sender, EventArgs e)
        {
            int index = comboFilter.SelectedIndex;

            if (index > -1 && ResourcesManager.TryGetResource((string)comboFilter.Items[index], out Resource resource))
                PopulateComboRecipe(resource);
            else
                PopulateComboRecipe(null);
        }


        private void RecipeSelectionChanged(object sender, EventArgs e)
        {
            int index = comboRecipe.SelectedIndex;
            if (index > -1)
            {
                var recipe = (Recipe)comboRecipe.Items[index];
                if (recipe != processor.Recipe)
                {
                    processor.Recipe = recipe;
                    SuspendLayout();
                    RemoveLinkControls();
                    CreateLinkControls(comboRecipe.Bottom+5, processor.Inputs, processor.Outputs);
                    Invalidate();
                    ResumeLayout(true);
                }
            }
        }
    }
}
