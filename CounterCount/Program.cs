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

     class TContourEdit : TInterfacedObject, IContour, IContourEdit
    {
        public TContourEdit(IContour contour)
        {

        }

        public void AddContourBit(IContourBit contourbit)
        {
            throw new NotImplementedException();
        }

        public IContourBit GetContourBit(int idx)
        {
            throw new NotImplementedException();
        }

        public int GetContourBitCount()
        {
            throw new NotImplementedException();
        }
    }

    class TContourBitEdit : IContourBit, IContourBitEdit, TInterfacedObject
    {
        public void AddPoint(double x, double y, double value)
        {
            throw new NotImplementedException();
        }

        public IContourPoint GetPoint(int idx)
        {
            throw new NotImplementedException();
        }

        public int GetPointCount()
        {
            throw new NotImplementedException();
        }

        public bool IsClosed()
        {
            throw new NotImplementedException();
        }

        public void SetClosed(bool closed)
        {
            throw new NotImplementedException();
        }
    }
   

}
