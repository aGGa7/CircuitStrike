using System;
using System.Collections.Generic;
using System.Text;

namespace CounterCount
{
   public class ContourPoint : IContourPoint
    {
        //public ContourPoint():this(null,null, 0)
        //{           
        //}
        public ContourPoint(double x, double y, double value)
        {
            //if(x==null || y==null)
            //{
            //    Random rng = new Random();
            //    this.x = (double)( x==null? rng.NextDouble() : x);
            //    this.y = (double)( y==null? rng.NextDouble() : y);
            //}
            this.x =(double) x;
            this.y =(double) y;
        }

      
        private double x;
        private double y;
        private double value;
        public double GetX()
        {
            return x;
        }

        public double GetY()
        {       
            return y;
        }
    }

    public class CounterBit : IContourBit

    {
        public CounterBit(): this(false)
        {
        }
        public CounterBit(bool isclosed)
        {
        }
       //public CounterBit(int pointCount, bool isclosed)
       // {
       //     for(int i=0; i<= pointCount; i++)
       //     {
       //         ContourPoint point = new ContourPoint();
       //         points.Add(point);
       //     }
       //     this.isclosed = isclosed;
       // }

        protected bool? isclosed = false;
        protected List<IContourPoint> points = new List<IContourPoint>();

        public IContourPoint GetPoint(int idx)
        {
            if (points.Count >= idx && idx >= 0)
                return points[idx];
            else throw new Exception("Массив точек отсутствует");
        }

        public int GetPointCount()
        {
            if (points != null)
                return points.Count;
            else throw new Exception("Массив точек отсутствует");
        }

        public bool IsClosed()
        {
            return (bool)isclosed;
        }
    }

   public class Contour: IContour
    {
        public Contour()
        {
        }
        public Contour(int count)
        {
            while(count>0)
            {
                bool isclose = count%2==0? true: false;
                CounterBit counterBit = new CounterBit(isclose);
                counterBits.Add(counterBit);
                count--;
            }
        }

        protected List<IContourBit> counterBits = new List<IContourBit>();

        public IContourBit GetContourBit(int idx)
        {
            if (idx >= 0 && counterBits.Count >= idx)
                return counterBits[idx];
            else throw new Exception("Массив контурбитов отсутствует");
        }

        public int GetContourBitCount()
        {
            if (counterBits != null)
                return counterBits.Count;
            else throw new Exception("Массив контурбитов отсутствует");
        }

    }

    public class TRect_Float
    {

        public double X1, X2, Y1, Y2;
      
    }
}
