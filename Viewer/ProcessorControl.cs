
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public class ProcessorControl : PlantNodeBaseControl
    {     
        ComboBox comboBox;
        ProcessorNode Processor;

        List<LinkerControl> InputLinkers;
        List<LinkerControl> OutputLinkers;


        public ProcessorControl(ProcessorNode plantnode) : base(plantnode)
        {
            Processor = plantnode;
            TitleColor = ColorTranslator.FromHtml("#DB7B7B");
            
            InitializeComponent();

            comboBox.SelectedValueChanged += ComboBox_SelectionChanged;

            InputLinkers = new List<LinkerControl>();
            OutputLinkers = new List<LinkerControl>();
        }

        private void ComboBox_SelectionChanged(object sender, EventArgs e)
        {
            var box = (ComboBox)sender;

            int index = box.SelectedIndex;
            if (index < 0) { RemoveLinkers(); return; }

            if (box.Items[index] is Recipe recipe)
            {
                if (Processor.Recipe == recipe) return;

                Processor.Recipe = recipe;
                RemoveLinkers();
                CreateLinkers(recipe);
            }
        }

        void RemoveLinkers()
        {
            SuspendLayout();
            foreach (var linker in InputLinkers) Controls.Remove(linker);
            foreach (var linker in OutputLinkers) Controls.Remove(linker);
            ResumeLayout(false);
            PerformLayout();
        }
        void CreateLinkers(Recipe recipe)
        {
            SuspendLayout();

            int offsetL = comboBox.Location.X;
            int offsetR = Size.Width - 2 - LinkerControl.DefaultWidth;
            int offsety = comboBox.Top + comboBox.Height + 2;

            int incount = recipe.ItemIn.Length;
            for (int i = 0; i < incount; i++)
            {
                var linker = new LinkerControl()
                {
                    LinkerType = LinkerType.AlwayFull,
                    LinkerText = $"{recipe.RateIn[i]} {recipe.ItemIn[i]}",
                    Left = offsetL,
                    Top = offsety + i * LinkerControl.DefaultHeight
                };

                InputLinkers.Add(linker);
                Controls.Add(linker);

                Height = Math.Max(Height, linker.Bottom + 4);
            }

            int outcount = recipe.ItemOut.Length;
            for (int i = 0; i < outcount; i++)
            {
                var linker = new LinkerControl()
                {
                    LinkerType = LinkerType.Output,
                    LinkerText = $"{recipe.RateOut[i]} {recipe.ItemOut[i]}",
                    Left = offsetR,
                    Top = offsety + i * LinkerControl.DefaultHeight
                };

                InputLinkers.Add(linker);
                Controls.Add(linker);

                Height = Math.Max(Height, linker.Bottom + 4);
            }

            ResumeLayout(false);
            PerformLayout();
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
            Name = "ProcessorControl";
            Size = new Size(219, 67);
            Controls.SetChildIndex(comboBox, 0);
            ResumeLayout(false);
        }
    }
}
