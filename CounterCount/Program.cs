using System;
using System.Collections.Generic;

namespace CounterCount
{
    class Program
    {
        static void Main(string[] args)
        {
        Console.WriteLine("Hello World!");
        }
    }

    class Contours : IContours
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

     class TContourEdit : Counter, TInterfacedObject, IContour, IContourEdit
    {
        public void AddContourBit(IContourBit contourbit)
        {
            counterBits.Add(contourbit);
        }
    }

    class TContourBitEdit : CounterBit, IContourBit, IContourBitEdit, TInterfacedObject
    {

        public void AddPoint(double x, double y, double value)
        {
            CounterPoint newPoint = new CounterPoint(x, y, value);
            points.Add(newPoint);
        }

        public void SetClosed(bool closed)
        {
            isclosed = closed;
        }
    }
   

}
