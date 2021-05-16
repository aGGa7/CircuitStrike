using System;
using System.Collections.Generic;
using System.Text;

namespace CounterCount
{
    /// <summary>
    /// точка
    /// </summary>
    public interface IContourPoint 
    {
        double GetX();
        double GetY();
    }
    /// <summary>
    /// фрагмент контура
    /// </summary>
    public interface IContourBit
    {
        int GetPointCount();
        bool IsClosed();
        IContourPoint GetPoint(int idx);
    }
    /// <summary>
    /// контур. состоит из нескольких фрагментов
    /// </summary>
    public interface IContour
    {
        int GetContourBitCount();
        IContourBit GetContourBit(int idx);

    }
    /// <summary>
    /// коллекция контуров
    /// </summary>
   public interface IContours
    {
        int GetContourCount();
        IContour GetContour(int idx);
    }
    /// <summary>
    /// 
    /// </summary>
   public interface IContourEdit:IContour
    {
        /// <summary>
        /// //Добавляет контурбит в контур
        /// </summary>
        /// <param name="counterbit"></param>
        void AddContourBit(IContourBit counterbit);
    }
   public interface IContourBitEdit
    {
        /// <summary>
        /// Value всегда должно быть = 0
        /// </summary>
        void AddPoint(double x, double y, double value);
        void SetClosed(bool closed);

    }
  public  interface TInterfacedObject
    {

    }
}
