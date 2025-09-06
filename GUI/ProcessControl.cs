using System;
using System.Data;
using System.Drawing;
using System.Linq;

namespace CaptainOfPlanner
{

    /// <summary>
    /// GUI interface for <see cref="Processor"/> node
    /// </summary>
    public partial class ProcessControl : NodeControl
    {
        Processor processor;
        public override Node Node => processor;

        int currentSelectedFilter = -1;
        int currentSelectedRecipe = -1;

        /// <summary>
        /// </summary>
        public ProcessControl(Processor node = null, PlantControl owner = null) : base(node, owner)
        {
            NodeColor = ColorTranslator.FromHtml("#6666FF");
            
            //fix layout
            Size = preferedsize;
            InitializeComponent();
            comboFilter.Width = Width - 6;
            comboRecipe.Width = Width - 6;

            OffsetInput = new Vector2i(2, comboRecipe.Bottom + 5);
            OffsetOutput = new Vector2i(Width - LinkControl.PreferedSize.width - 2, comboRecipe.Bottom + 5);

            comboFilter.DataSource = ResourcesManager.Resources;
            comboFilter.DisplayMember = "Name";
            PopulateComboRecipe(Resource.Undefined);

            comboFilter.SelectedValueChanged += comboFilter_SelectedValueChanged;
            comboRecipe.SelectedValueChanged += comboRecipe_SelectedValueChanged;
            //set undefined resources, mean all recipes
            comboFilter.SelectedIndex = 0;
            //set the recipe
            comboRecipe.SelectedIndex = comboRecipe.FindString(node?.Recipe?.Display);
           

            //now you can associate the node to this controller
            if (DesignMode || node == null) return;
            processor = node;
            RemoveLinkControls();
            CreateLinkControls(processor.Inputs, processor.Outputs);
            Invalidate();
            ResumeLayout();
        }

        /// <summary>
        /// change recipe list using resource filter
        /// </summary>
        void PopulateComboRecipe(Resource resource)
        {
            if (resource.IsUndefined)
            {
                comboRecipe.DataSource = RecipesManager.Recipes;
            }
            else
            {
                Func<Recipe, bool> selector = recipe => recipe.Contains(resource, out _);
                comboRecipe.DataSource = RecipesManager.Recipes.Where(selector).ToList() ;
            }
            comboRecipe.ValueMember = "Display";
        }

        private void comboFilter_SelectedValueChanged(object sender, EventArgs e)
        {
            int index = comboFilter.SelectedIndex;

            if (index == currentSelectedFilter) return;
            currentSelectedFilter = index;

            if (index > -1)
                PopulateComboRecipe((Resource)comboFilter.Items[index]);
            else
                PopulateComboRecipe(Resource.Undefined);
        }
        private void comboRecipe_SelectedValueChanged(object sender, EventArgs e)
        {
            int index = comboRecipe.SelectedIndex;
            if (index == currentSelectedRecipe) return;
            currentSelectedRecipe = index;

            if (processor == null) return;

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
