using System;
using System.Collections.Generic;
using System.Text;

namespace CounterCount
{
    /// <summary>
    /// точка
    /// </summary>
    interface IContourPoint 
    {
        double GetX();
        double GetY();
    }
    /// <summary>
    /// фрагмент контура
    /// </summary>
    interface IContourBit
    {
        int GetPointCount();
        bool IsClosed();
        IContourPoint GetPoint(int idx);
    }
    /// <summary>
    /// контур. состоит из нескольких фрагментов
    /// </summary>
    interface IContour
    {
        int GetContourBitCount();
        IContourBit GetContourBit(int idx);

    }
    /// <summary>
    /// коллекция контуров
    /// </summary>
    interface IContours
    {
        int GetContourCount();
        IContour GetContour(int idx);
    }
    /// <summary>
    /// 
    /// </summary>
    interface IContourEdit:IContour
    {
        /// <summary>
        /// //Добавляет контурбит в контур
        /// </summary>
        /// <param name="counterbit"></param>
        void AddContourBit(IContourBit counterbit);
    }
    interface IContourBitEdit
    {
        /// <summary>
        /// Value всегда должно быть = 0
        /// </summary>
        void AddPoint(double x, double y, double value);
        void SetClosed(bool closed);

    }
    interface TInterfacedObject
    {

    }
}
