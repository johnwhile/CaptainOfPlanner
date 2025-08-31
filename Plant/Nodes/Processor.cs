
using CaptainOfPlanner.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Xml;
using static System.Windows.Forms.LinkLabel;

namespace CaptainOfPlanner
{
    /// <summary>
    /// A node that transform some inputs in some outputs
    /// </summary>
    public class Processor : Node
    {
        public override NodeType Type => NodeType.Processor;
        Recipe recipe;

        /// <summary>
        /// Changing recipe cause invalidating all inputs and outputs
        /// </summary>
        public Recipe Recipe
        {
            get => recipe;
            set
            {
                if (recipe == value) return;
                recipe = value;

                Inputs.Clear();
                Outputs.Clear();

                if (Control is ProcessorControl processorControl)
                {
                    Point offset = processorControl.GetOffsetLocation(LinkType.Input);
                    foreach (var itemcount in recipe.Inputs)
                    {
                        var link = new Link(this, LinkType.Input, itemcount);
                        Inputs.Add(link);
                        Control.AddLinkControl(link.Control, offset);
                    }
                    offset = processorControl.GetOffsetLocation(LinkType.Output);
                    foreach (var itemcount in recipe.OutPuts)
                    {
                        var link = new Link(this, LinkType.Output, itemcount);
                        Outputs.Add(link);
                        Control.AddLinkControl(link.Control, offset);
                    }
                    
                }

            }
        }

        protected override NodeControl GenerateControl() => new ProcessorControl(this);


        internal Processor(Plant plant, string name) : base(plant, name ?? "processor")
        {

        }

        public void LoadXml(XmlElement element)
        {
            if (RecipesManager.TryGetValue(element.GetAttribute("recipe"), out Recipe recipe))
            {
                if (Control is ProcessorControl processor)
                {
                    for (int i = 0; i < processor.combox.Items.Count; i++)
                        if (processor.combox.Items[i] is string name && name.CompareTo(recipe.Display) == 0)
                        {
                            processor.combox.SelectedIndex = i;
                            break;
                        }
                }
            }
        }
        public override XmlElement SaveXml(XmlElement plant)
        {
            if (recipe == null) return null;
            var node = base.SaveXml(plant);
            
            node.SetAttribute("recipe", recipe.Encoded);
            
            return node;
        }

    }



}
