using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CaptainOfPlanner
{
    public static class IconsManager
    {
        public static Vector2i Size { get; private set; }
        public static int Columns { get; private set; }
        public static int Rows { get; private set; }
        public static Dictionary<string, Vector2i> UvMap { get; private set; }
        public static Bitmap Image { get; private set; }


        public static Rectangle GetRectangle(string resource)
        {
            if (!UvMap.TryGetValue(resource, out Vector2i uv)) uv = Vector2i.Zero;
            if (uv.x >= Columns || uv.y >= Rows) throw new IndexOutOfRangeException("wrong uv map");
            return new Rectangle(uv.x * Size.x, uv.y * Size.y, Size.x, Size.y);
        }

        public static bool Load(string xml = "Icons.xml")
        {
            Image = Properties.Resources.icons;


            UvMap = new Dictionary<string, Vector2i>();

            var doc = new XmlDocument();
            doc.Load(xml);

            var root = doc.DocumentElement;

            //Resources must be the only one root node
            if (string.Compare(root.Name, "Icons", true) == 0)
            {
                if (!Vector2i.TryParse(root.GetAttribute("size"), out Vector2i size)) size = new Vector2i(32, 32);
                if (!int.TryParse(root.GetAttribute("column"), out int coloumns)) coloumns = 10;
                if (!int.TryParse(root.GetAttribute("row"), out int rows)) rows = 36;

                Size = size;
                Columns = coloumns;
                Rows = rows;

                foreach (XmlElement node in root.ChildNodes)
                    if (string.Compare(node.Name, "item", true) == 0)
                    {
                        if (!Vector2i.TryParse(node.GetAttribute("uv"), out Vector2i uv)) uv = Vector2i.Zero;
                        UvMap.Add(node.InnerText.Trim(), uv);
                    }
            }

            return true;
        }
    }
}
