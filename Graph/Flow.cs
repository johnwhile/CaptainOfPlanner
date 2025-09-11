using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptainOfPlanner
{
    /// <summary>
    /// represents a resources flow
    /// </summary>
    public struct Flow
    {
        public readonly Resource Resource;

        public bool IsInfinite => float.IsPositiveInfinity(Rate);

        /// <summary>
        /// flow rate in minutes <br />
        /// 0 = not required for processor.<br />
        /// <see cref="float.PositiveInfinity"></see> = processor always satisfied<br />
        /// </summary>
        public float Rate;
        
        public Flow(Resource resource, float rate = 0) : this()
        {
            Resource = resource;
            Rate = rate;
            //if (asrequest) Resource.Flags = FlowFlag.Requested;
        }

        public static Flow Infinite(Resource resource) => new Flow(resource, float.PositiveInfinity);

        /// <summary>
        /// Custom logic when add two flows
        /// </summary>
        public static Flow operator +(Flow left, Flow right)
        {
            if (!left.Resource.IsCompatible(right.Resource)) throw new ArgumentException("can't operator incompatible flow's resources");

            if (left.IsInfinite || right.IsInfinite)
            {
                left.Rate = Math.Max(left.Rate, right.Rate);
            }
            else
            {
                left.Rate += right.Rate;
            }
            return left;
        }

        public override string ToString()
        {
            if (float.IsPositiveInfinity(Rate)) return "INF.";
            return Rate.ToString("0.00");
        }
    }
}
