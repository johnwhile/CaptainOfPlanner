using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace CaptainOfPlanner
{
    public partial class ProcessControl : NodeControl
    {
        Processor processor;
        public override Node Node => processor;

        /// <summary>
        /// </summary>
        public ProcessControl(Processor node = null) : base(node)
        {
            processor = node;
            NodeColor = ColorTranslator.FromHtml("#6666FF");
            Name = "ProcessorCtrl";
            InitializeComponent();


            OffsetInput = new Vector2i(2, comboRecipe.Bottom + 5);
            OffsetOutput = new Vector2i(Width - LinkControl.PreferedSize.width - 2, comboRecipe.Bottom + 5);

            RemoveLinkControls();
            CreateLinkControls(processor.Inputs, processor.Outputs);
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

        private void comboFilter_SelectedValueChanged(object sender, EventArgs e)
        {
            int index = comboFilter.SelectedIndex;

            if (index > -1 && ResourcesManager.TryGetResource((string)comboFilter.Items[index], out Resource resource))
                PopulateComboRecipe(resource);
            else
                PopulateComboRecipe(null);
        }

        private void comboRecipe_SelectedValueChanged(object sender, EventArgs e)
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
                    CreateLinkControls(processor.Inputs, processor.Outputs);
                    Invalidate();
                    ResumeLayout(true);
                }
            }
        }
    }
}
