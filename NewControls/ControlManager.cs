using System;
using System.Drawing;
using System.Windows.Forms;

namespace CaptainOfPlanner.NewControls
{
    public class ControlManager
    {
        public static Font ArialBold6;
        public static SolidBrush SBrush;
        public static Pen BlackPen;

        PlantControl control;
        Plant plant;


        static ControlManager()
        {
            SBrush = new SolidBrush(Color.Gray);
            ArialBold6 = new Font("Arial", 6f, FontStyle.Bold);
            BlackPen = Pens.Black;
        }


        public PlantControl Control
        {
            get => control;
        }

        public Plant Plant
        {
            get => plant;
            set
            {
                if (plant == value) return;
                control.Plant = value;
                control.GenerateControllers(value);
                plant = value;
            }
        }
      

        /// <summary>
        /// </summary>
        /// <param name="viewer">must be an empty controller</param>
        public ControlManager(PlantControl viewer)
        {
            if (viewer == null) throw new ArgumentNullException();
            control = viewer;
        }




    }
}
