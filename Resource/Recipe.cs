using System;
using System.Text;

namespace CaptainOfPlanner
{
    /// <summary>
    /// </summary>
    public struct ResourceCount
    {
        /// <summary>
        /// Quantity processed. If zero are considered infinite input or output
        /// </summary>
        public byte Count;
        /// <summary>
        /// type of resource
        /// </summary>
        public Resource Resource;
        /// <summary>
        /// rate as processed per minutes, calculated as Count / Recipe's Time * 60
        /// </summary>
        public float Rate;

        public ResourceCount(Resource resource, byte count=0) : this()
        {
            Count = count;
            Resource = resource;
        }


        public static ResourceCount Undefined = new ResourceCount() { Resource = Resource.Undefined };

        public override string ToString() => $"{Resource}";
        //public override string ToString() => $"{Count} {Resource}";
    }


    public class Recipe : IComparable<Recipe>
    {
        public int Id;
        public string Name;
        public string Display;
        public readonly int Time;
        public readonly ResourceCount[] Inputs;
        public readonly ResourceCount[] OutPuts;
        public readonly string Encoded;


        public Recipe() : this(0, null, null)
        {

        }


        public Recipe(int time, ResourceCount[] inputs, ResourceCount[] output)
        {
            Name = "Recipe";
            Time = time;
            Inputs = inputs;
            OutPuts = output;

            if (Inputs == null) Inputs = new ResourceCount[0];
            if (OutPuts == null) OutPuts = new ResourceCount[0];

            var encoded = Encode(this);
            StringBuilder builder = new StringBuilder();
            foreach (var value in encoded)
            {
                builder.Append(value.ToString());
                builder.Append('.');
            }
            Encoded = builder.ToString();

            Display = ToFormatString();
        }

        /// <summary>
        /// </summary>
        /// <param name="name">resource name</param>
        public bool Contains(string name, out LinkType type)
        {
            type = LinkType.Undefined;
            if (ResourcesManager.TryGetResource(name, out Resource resource))
                return Contains(resource, out type);
            return false;
        }
        public bool Contains(Resource resource, out LinkType type)
        {
            type = LinkType.Undefined;
            foreach (var res in Inputs) if (res.Resource.IsCompatible(resource)) { type = LinkType.Input; return true; }
            foreach (var res in OutPuts) if (res.Resource.IsCompatible(resource)) { type = LinkType.Output; return true; }
            return false;
        }
        public int CompareTo(Recipe other) => Display.CompareTo(other.Display);

        public static Recipe Empty => new Recipe() { Id = -1, Name = "undefined" };

        void buildformattedstring(ResourceCount[] array, StringBuilder builder)
        {
            if (array == null || array.Length == 0)
            {
                builder.Append("empty");
                return;
            }
            builder.Append(array[0].Resource.ToFormatString(10));
            for (int i = 1; i < array.Length; i++)
            {
                builder.Append(" + ");
                builder.Append(array[i].Resource.ToFormatString(10));
            }
        }

        public static ushort[] Encode(Recipe recipe)
        {
            byte ni = (byte)(recipe.Inputs != null ? recipe.Inputs.Length : 0);
            byte no = (byte)(recipe.OutPuts != null ? recipe.OutPuts.Length : 0);

            ushort[] buffer = new ushort[ni + no + 1];

            buffer[0] = (ushort)(ni | no << 8);

            int j = 1;
            for (int i = 0; i < ni; i++)
                buffer[j++] = (ushort)(recipe.Inputs[i].Resource.ID | recipe.Inputs[i].Count << 8);

            for (int i = 0; i < no; i++)
                buffer[j++] = (ushort)(recipe.OutPuts[i].Resource.ID | recipe.OutPuts[i].Count << 8);

            return buffer;
        }


        public string ToFormatString()
        {
            StringBuilder builder = new StringBuilder();
            buildformattedstring(Inputs, builder);
            builder.Append(" → ");
            buildformattedstring(OutPuts, builder);
            return builder.ToString();
        }

        public override string ToString()
        {
            string result = "";
            if (Inputs != null) result += string.Join(" + ", Inputs);
            result += " → ";
            if (OutPuts != null) result += string.Join(" + ", OutPuts);
            return result;
        }


    }

}
