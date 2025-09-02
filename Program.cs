using System;
using System.Windows.Forms;

namespace CaptainOfPlanner.NewControls
{
    internal static class Program
    {
        public static Random Random = new Random();
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Window());
        }
    }
}
