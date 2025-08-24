using System;
using System.Collections.Generic;

namespace CaptainOfPlanner
{
    /// <summary>
    /// 8 bytes
    /// </summary>
    public struct ResourceCount
    {
        /// <summary>
        /// Quantity processed
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

        public static ResourceCount Undefined = new ResourceCount() { Resource = Resource.Undefined };

        public override string ToString() => $"{Count} {Resource}";
    }


    public class Recipe
    {
        public int Id;
        public string Name;
        public int Time;
        public ResourceCount[] Inputs;
        public ResourceCount[] OutPuts;

        public override string ToString() =>
            string.Join(" + ", Inputs) + " → " + 
            string.Join(" + ", OutPuts);

    }

}
