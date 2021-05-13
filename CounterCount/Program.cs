using System;

namespace CounterCount
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
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
    class TContourEdit : TInterfacedObject, IContour, IContourEdit
    {
        public void AddContourBit(IContourBit counterbit)
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

}
