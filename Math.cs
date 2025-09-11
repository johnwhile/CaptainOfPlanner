using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace CaptainOfPlanner
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Vector2i
    {
        [FieldOffset(0)]
        public int x;
        [FieldOffset(0)]
        public int width;
        [FieldOffset(4)]
        public int height;
        [FieldOffset(4)]
        public int y;

        public Vector2i(int x, int y) : this()
        {
            this.x = x;
            this.y = y;
        }
        public static Vector2i Zero => new Vector2i();

        public static Vector2i operator /(Vector2i left, int scalar) =>
            new Vector2i(left.x / scalar, left.y / scalar);
        public static Vector2i operator *(Vector2i left, int scalar) =>
            new Vector2i(left.x * scalar, left.y * scalar);
        public static Vector2i operator -(Vector2i left, Vector2i right) =>
            new Vector2i(left.x - right.x, left.y - right.y);
        public static Vector2i operator +(Vector2i left, Vector2i right) =>
            new Vector2i(left.x + right.x, left.y + right.y);

        public static implicit operator Point(Vector2i vector) =>
            new Point(vector.x, vector.y);

        public static implicit operator Vector2i(Point vector) =>
            new Vector2i(vector.X, vector.Y);

        public static implicit operator Vector2i(Size vector) =>
            new Vector2i(vector.Width, vector.Height);

        public static implicit operator Size(Vector2i vector) =>
            new Size(vector.x, vector.y);

        /// <summary>
        /// parse a two integer vector
        /// </summary>
        /// <param name="value">must be in format \"0,0\"</param>
        /// <param name="separator">the char used to separate two integers</param>
        public static bool TryParse(string value,  out Vector2i result, char separator = ',')
        {
            result = default;
            var split = value.Split(separator);
            if (split.Length > 1 && int.TryParse(split[0], out result.x) && int.TryParse(split[1], out result.y)) return true;
            return false;
        }
        public override string ToString() => $"{x},{y}";
    }


    [StructLayout(LayoutKind.Explicit)]
    public struct Vector2f
    {
        [FieldOffset(0)]
        public float x;
        [FieldOffset(0)]
        public float width;
        [FieldOffset(4)]
        public float height;
        [FieldOffset(4)]
        public float y;
        public Vector2f(float x, float y) : this()
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2f operator /(Vector2f left, int scalar) =>
            new Vector2f(left.x / scalar, left.y / scalar);
        public static Vector2f operator *(Vector2f left, int scalar) =>
            new Vector2f(left.x * scalar, left.y * scalar);
        public static Vector2f operator -(Vector2f left, Vector2f right) =>
            new Vector2f(left.x - right.x, left.y - right.y);
        public static Vector2f operator +(Vector2f left, Vector2f right) =>
            new Vector2f(left.x + right.x, left.y + right.y);

        public static implicit operator PointF(Vector2f vector) =>
            new PointF(vector.x, vector.y);

        public static implicit operator Vector2f(PointF vector) =>
            new Vector2f(vector.X, vector.Y);


        /// <summary>
        /// parse a two float vector
        /// </summary>
        /// <param name="value">must be in format \"0.0,0.0\"</param>
        /// <param name="separator">the char used to separate two integers, can't be '.'</param>
        public static bool TryParse(string value, out Vector2f result, char separator = ',')
        {
            result = default;
            var split = value.Split(separator);
            if (split.Length > 1 && float.TryParse(split[0], out result.x) && float.TryParse(split[1], out result.y)) return true;
            return false;
        }

        public override string ToString() => $"{x.ToString(CultureInfo.InvariantCulture)},{y.ToString(CultureInfo.InvariantCulture)}";
    }

}
