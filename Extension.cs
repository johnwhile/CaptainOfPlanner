using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace CaptainOfPlanner
{

    internal static class Extensions
    {
        public static void ClearEvents(object instance)
        {
            var eventsToClear = instance.GetType().GetEvents(
                BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Instance | BindingFlags.Static);

            foreach (var eventInfo in eventsToClear)
            {
                var fieldInfo = instance.GetType().GetField(
                    eventInfo.Name,
                    BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

                if (fieldInfo.GetValue(instance) is Delegate eventHandler)
                    foreach (var invocatedDelegate in eventHandler.GetInvocationList())
                        eventInfo.GetRemoveMethod(fieldInfo.IsPrivate).Invoke(
                            instance,
                            new object[] { invocatedDelegate });
            }
        }


        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        public static byte[] HexStringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static T GetLast<T>(this List<T> list) => list.Count > 0 ? list[list.Count-1] : default(T);
        

        public static GraphicsPath RoundedRectanglePath(int x, int y, int width, int height, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(x, y, radius, radius, 180, 90);
            path.AddArc(x + width - radius, y, radius, radius, 270, 90);
            path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
            path.AddArc(x, y + height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
        public static bool IsMouseOver(this Control control, Vector2i screen)
        {
            Vector2i local = control.PointToClient(screen);
            return !(
                local.x < 0 || 
                local.y < 0 ||
                local.x > control.Width ||
                local.y > control.Height);
        }
    }
}
