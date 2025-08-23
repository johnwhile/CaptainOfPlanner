using System;

namespace CaptainOfPlanner
{
    public class Recipe
    {
        public int Id;
        public string Name;
        public int Time;
        public Resource[] ItemIn;
        public Resource[] ItemOut;
        public int[] RateIn;
        public int[] RateOut;


        public override string ToString()
        {
            string str = "";
            for (int i=0;i<ItemIn.Length;i++)
            {
                str += $"{RateIn[i]} {ItemIn[i]}";
                if (i < ItemIn.Length-1) str += " + ";
            }
            str += "→ ";
            for (int i = 0; i < ItemOut.Length; i++)
            {
                str += $"{RateOut[i]} {ItemOut[i]}";
                if (i < ItemOut.Length-1) str += " + ";
            }
            return str;
        }
    }

}
