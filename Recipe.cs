using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return $"{Id}_{Name}_{ItemIn[0]}→{ItemOut[0]}";
        }
    }

}
