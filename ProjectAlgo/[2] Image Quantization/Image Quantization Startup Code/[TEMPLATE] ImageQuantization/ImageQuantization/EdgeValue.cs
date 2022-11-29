using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    class EdgeValue
    {
       public int root;//->O(1)
        public  int adjacent;//->O(1)
        public double Distance;//->O(1)
        public EdgeValue()
        {

        }
        public EdgeValue(double Distance,int root,int adjacent)//->O(1)
        {
            this.adjacent = adjacent;//->O(1)
            this.root = root;//->O(1)
            this.Distance = Distance;//->O(1)
        }
    }
}
