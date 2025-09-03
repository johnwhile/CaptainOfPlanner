using System.Drawing;

namespace CaptainOfPlanner.NewControls
{
    public class StorageControl : NodeControl
    {
        Storage storage;

        public override Node Node => storage;

        public StorageControl(Storage node) : base(node)
        {
            storage = node;
            NodeColor = ColorTranslator.FromHtml("#FF66FF");
            Name = "StorageCtrl";
        }
    }
}
