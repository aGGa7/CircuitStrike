using System;
using System.Collections.Generic;
using System.Text;

namespace CounterCount
{
    class CounterPoint : IContourPoint
    {
        Random rng = new Random();
        private double x;
        private double y;
        public double GetX()
        {
           x = rng.Next();
            return x;
        }

        public double GetY()
        {
            y = rng.Next();
            return y;
        }
    }
}
