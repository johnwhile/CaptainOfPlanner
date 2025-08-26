
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace CaptainOfPlanner
{
    public class ProcessorControl : PlantNodeBaseControl
    {     
        ComboBox comboBox;
        
        ProcessorNode Processor;

        public ProcessorControl(ProcessorNode plantnode) : base(plantnode)
        {
            Processor = plantnode;
            TitleColor = ColorTranslator.FromHtml("#DB7B7B");
            
            InitializeComponent();

            comboBox.SelectedValueChanged += ComboBox_SelectionChanged;


        }

        private void ComboBox_SelectionChanged(object sender, EventArgs e)
        {
            var box = (ComboBox)sender;

            int index = box.SelectedIndex;
            if (index < 0) { RemoveLinkers(); return; }

            if (box.Items[index] is Recipe recipe)
            {
                if (Processor.Recipe != recipe)
                {
                    Processor.Recipe = recipe;
                    RemoveLinkers();

                    int h = comboBox.Top + comboBox.Height + 2;

                    CreateInputLinkers(comboBox.Location.X, h );
                    CreateOuputLinkers(Size.Width - 2 - LinkerControl.DefaultWidth, h);

                    ResumeLinkers();
                }
            }
        }

        protected virtual void CreateInputLinkers(int offsetx, int offsety)
        {
            int i = 0;
            foreach(var link in Processor.Inputs)
            {
                var linker = new LinkerControl()
                {
                    LinkerType = LinkerType.AlwayFull,
                    LinkerText = link.ResourceCount.ToString(),
                    Left = offsetx,
                    Top = offsety + i++ * LinkerControl.DefaultHeight,
                    LinkNode = link
                };
                InputControls.Add(linker);
                Height = Math.Max(Height, linker.Bottom + 4);
            }
        }
        protected virtual void CreateOuputLinkers(int offsetx, int offsety)
        {
            int i = 0;
            foreach (var link in Processor.Outputs)
            {
                var linker = new LinkerControl()
                {
                    LinkerType = LinkerType.Output,
                    LinkerText = link.ResourceCount.ToString(),
                    Left = offsetx,
                    Top = offsety + i++ * LinkerControl.DefaultHeight,
                    LinkNode = link
                };
                OutputControls.Add(linker);
                Height = Math.Max(Height, linker.Bottom + 4);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            comboBox.Items.Clear();

            int maxlength = 0;
            foreach (var recipe in RecipesManager.Recipes)
            {
                maxlength = Math.Max(maxlength, recipe.ToString().Length);
                comboBox.Items.Add(recipe);
            }

            comboBox.DropDownWidth = maxlength * 6;
            var size = comboBox.Size;
            size.Width = Width - 10;
            comboBox.Size = size;

            buttonClose.Left = Width - 3 - buttonClose.Width;
        }


        private void InitializeComponent()
        {
            comboBox = new ComboBox();
            SuspendLayout();

            comboBox.Location = new Point(4, 32);
            comboBox.Name = "comboBox";
            comboBox.Size = new Size(212, 21);
            comboBox.TabIndex = 2;

            AutoScaleDimensions = new SizeF(6F, 13F);
            Controls.Add(comboBox);
            Size = new Size(219, 67);
            Controls.SetChildIndex(comboBox, 0);
            ResumeLayout(false);
        }
    }
}
