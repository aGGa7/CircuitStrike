using System;
using System.Collections.Generic;
using System.Drawing;

namespace CounterCount
{
    class Program
    {
        static void Main(string[] args)
        {
        Console.WriteLine("Hello World!");
        }
    }

   public class Contours : IContours
    {
        private List<IContour> contours;

        public Contours(List<IContour> contours)
        {
            this.contours = contours;
        }
        public IContour GetContour(int idx)
        {
            if (contours != null && contours.Count > 0)
                return contours[idx];
            else  throw new Exception("Массив контуров не существует");
        }

        public int GetContourCount()
        {
            if (contours != null)
                return contours.Count;
            else throw new Exception("Массив контуров не существует");
        }
    }

   public  class TContourEdit : Contour, TInterfacedObject, IContour, IContourEdit
    {
        public void AddContourBit(IContourBit contourbit)
        {
            counterBits.Add(contourbit);
        }
    }

   public class TContourBitEdit : CounterBit, IContourBit, IContourBitEdit, TInterfacedObject
    {

        public void AddPoint(double x, double y, double value)
        {
            ContourPoint newPoint = new ContourPoint(x, y, value);
            points.Add(newPoint);
        }

        public void SetClosed(bool closed)
        {
            isclosed = closed;
        }
    }
   

}
