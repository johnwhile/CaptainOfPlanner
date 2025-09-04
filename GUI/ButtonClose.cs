using System.Windows.Forms;

namespace CaptainOfPlanner
{
    public class ButtonClose : Button
    {
        public ButtonClose()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BackgroundImage = Properties.Resources.close;
            FlatStyle = FlatStyle.Standard;
            Size = BackgroundImage.Size;
        }
    }
}
