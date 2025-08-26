using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public enum LinkerType
    {
        None,
        Input,
        Output,
        AlwayFull
    }


    public partial class LinkerControl : UserControl
    {
        bool draging = false;
        public const int DefaultWidth = 100;
        public const int DefaultHeight = 20;

        LinkerType linkerType;

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
                        labelType.BackColor = Color.LightGreen;
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
                        labelType.BackColor = Color.LightBlue;
                        labelType.Width = 17;
                        labelType.Text = "inf";
                        break;
                    default:
                        labelType.Dock = DockStyle.Left;
                        labelType.BackColor = Color.LightGray;
                        labelType.Width = 17;
                        labelType.Text = "none";
                        break;
                }
                ResumeLayout(true);
            }
        }

        public LinkNode LinkNode;

        /// <summary>
        /// maintain the direction of the flow of resources,
        /// the next one always corresponds to the arrival
        /// </summary>
        public LinkerControl NextLinker;

        public string LinkerText
        {
            get => labelResource.Text;
            set => labelResource.Text = value;
        }
        
        public LinkerControl()
        {
            InitializeComponent();

            LinkerType = LinkerType.AlwayFull;
            
            foreach(var child in Controls)
             {
                if (child is Control control)
                {
                    control.MouseDown += LinkerControl_MouseDown;
                    control.MouseUp += LinkerControl_MouseUp;
                }
            }

            MouseDown += LinkerControl_MouseDown;
            MouseUp += LinkerControl_MouseUp;
        }

        private void LinkerControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (draging && LinkNode!=null)
            {
                if (Parent.Parent is PlantViewer Viewer)
                {
                    var screen = PointToScreen(e.Location);

                    foreach(var child in Viewer.Controls)
                    {
                        if (child is PlantNodeBaseControl plantnode)
                        {
                            if (plantnode.IsMouseOver(screen))
                            {
                                Console.WriteLine("IS OVER PLANT NODE " + plantnode);
                            }
                        }
                    }


                }
            }
        }

        private void LinkerControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && LinkNode!=null) draging = true;
        }

        public LinkerControl(LinkerType type)
        {
            InitializeComponent();
            LinkerType = type;
        }


    }
}
