using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.CodeDom;

namespace CaptainOfPlanner
{
    public enum LinkerType
    {
        Input,
        Output,
        AlwayFull
    }


    public partial class LinkerControl : UserControl
    {
        public const int DefaultWidth = 100;
        public const int DefaultHeight = 20;

        LinkerType linkerType = LinkerType.Input;

        [Category("Appearance")]
        [Browsable(true)]
        public LinkerType LinkerType 
        {
            get => linkerType;
            set
            {
                if (linkerType == value) return;
                linkerType = value;

                SuspendLayout();

                switch(linkerType)
                {
                    case LinkerType.Input: 
                        labelType.Dock = DockStyle.Left;
                        labelType.BackColor = ColorTranslator.FromHtml("#00CC00");
                        labelType.Width = 17;
                        labelType.Text = "IN";
                        break;
                    case LinkerType.Output: 
                        labelType.Dock = DockStyle.Right;
                        labelType.BackColor = ColorTranslator.FromHtml("#CC0000");
                        labelType.Width = 17;
                        labelType.Text = "OUT";
                        break;
                    case LinkerType.AlwayFull:
                        labelType.Dock = DockStyle.Left;
                        labelType.BackColor = ColorTranslator.FromHtml("#0000CC");
                        labelType.Width = 17;
                        labelType.Text = "inf";
                        break;
                }
                ResumeLayout(true);

            }
        }

        public string LinkerText
        {
            get => labelResource.Text;
            set => labelResource.Text = value;
        }
        
        public LinkerControl()
        {
            InitializeComponent();
            LinkerType = LinkerType.AlwayFull;
        }
        public LinkerControl(LinkerType type)
        {
            InitializeComponent();
            LinkerType = type;
        }
    }
}
