using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaptainOfPlanner
{
    public class Recipe : IComparable<Recipe>
    {
        public static Recipe Empty = new Recipe() { Name = "undefined" };

        public int Id;
        public string Name { get; set; }
        public string Display { get;}
        public readonly int Time;
        public readonly byte[] InCount;
        public readonly byte[] OutCount;
        public readonly Resource[] Inputs;
        public readonly Resource[] OutPuts;
        public readonly string Encoded;


        public IEnumerable<(Resource,byte)> InputCollection
        {
            get
            {
                if (Inputs != null) for (int i = 0; i < Inputs.Length; i++)
                        yield return (Inputs[i], InCount[i]);
                yield break;
            }
        }
        public IEnumerable<(Resource, byte)> OutputCollection
        {
            get
            {
                if (OutPuts != null) for (int i = 0; i < OutPuts.Length; i++)
                        yield return (OutPuts[i], OutCount[i]);
                yield break;
            }
        }

        public Recipe() : this(0, null, null, null, null)
        {

        }

        public Recipe(int time, IEnumerable<(Resource, byte)> inputs, IEnumerable<(Resource, byte)> output) :
            this(time,
                inputs.Select(x => x.Item1).ToArray(),
                inputs.Select(x => x.Item2).ToArray(),
                output.Select(x => x.Item1).ToArray(),
                output.Select(x => x.Item2).ToArray())
        { }

        public Recipe(int time, Resource[] inputs, byte[] inputquantity, Resource[] output, byte[] outputquantity)
        {
            Name = "Recipe";
            Time = time;
            Inputs = inputs;
            OutPuts = output;
            InCount = inputquantity;
            OutCount = outputquantity;

            if (Inputs == null) Inputs = new Resource[0];
            if (OutPuts == null) OutPuts = new Resource[0];

            var encoded = Encode(this);
            StringBuilder builder = new StringBuilder();
            foreach (var value in encoded)
            {
                builder.Append(value.ToString());
                builder.Append('.');
            }
            Encoded = builder.ToString();

            Display = ToString();
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
            foreach (var res in Inputs) if (res.IsCompatible(resource)) { type = LinkType.Input; return true; }
            foreach (var res in OutPuts) if (res.IsCompatible(resource)) { type = LinkType.Output; return true; }
            return false;
        }
        public static ushort[] Encode(Recipe recipe)
        {
            byte ni = (byte)(recipe.Inputs != null ? recipe.Inputs.Length : 0);
            byte no = (byte)(recipe.OutPuts != null ? recipe.OutPuts.Length : 0);

            ushort[] buffer = new ushort[ni + no + 1];

            buffer[0] = (ushort)(ni | no << 8);

            int j = 1;
            for (int i = 0; i < ni; i++)
                buffer[j++] = (ushort)(recipe.Inputs[i].ID | recipe.InCount[i] << 8);

            for (int i = 0; i < no; i++)
                buffer[j++] = (ushort)(recipe.OutPuts[i].ID | recipe.OutCount[i] << 8);

            return buffer;
        }

        [Obsolete]
        void buildformattedstring(Resource[] array, StringBuilder builder)
        {
            if (array == null || array.Length == 0)
            {
                builder.Append("empty");
                return;
            }
            builder.Append(array[0].Name.CutString(10));
            for (int i = 1; i < array.Length; i++)
            {
                builder.Append(" + ");
                builder.Append(array[i].Name.CutString(10));
            }
        }

        [Obsolete]
        public string ToFormatString()
        {
            StringBuilder builder = new StringBuilder();
            buildformattedstring(Inputs, builder);
            builder.Append(" → ");
            buildformattedstring(OutPuts, builder);
            return builder.ToString();
        }

        public int CompareTo(Recipe other) => Display.CompareTo(other.Display);
        
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
