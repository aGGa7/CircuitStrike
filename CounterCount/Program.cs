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
        public Contours():this(new List<IContour>())
        {
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
        public void AddContour(IContour contour)
        {
            contours.Add(contour);
        }
    }

   public  class TContourEdit : Contour, TInterfacedObject, IContour, IContourEdit
    {
        public void AddContourBit(IContourBit contourbit)
        {
            counterBits.Add(contourbit);
        }

        public IContour CutContoursByWindow (IContours contours, TRect_Float rect_Float)
        {
            TContourBitEdit res= new TContourBitEdit();
            TContourEdit contourRes = new TContourEdit();
            IContour contour;
            
            for (int i = 0; i < contours.GetContourCount(); i++)
            {
                
                contour = contours.GetContour(i);
              
                for (int y = 0; y < contour.GetContourBitCount(); y++)
                {
                    bool prevPointIsCheked = false;
                    bool lastPointAdd = true;
                    for (int p = 0; p < contour.GetContourBit(y).GetPointCount(); p++)
                    {
                        double xpoint = contour.GetContourBit(y).GetPoint(p).GetX();
                        double ypoint = contour.GetContourBit(y).GetPoint(p).GetY();
                        if (prevPointIsCheked)
                        {
                            res.AddPoint(xpoint, ypoint, 0);
                            prevPointIsCheked = false;
                        }

                        else
                        {
                            double[] z = new double[4];
                            for (int j = 0; j < 4; j++)
                            {
                                z[j] = ZCalc(rect_Float.SidePoints[j], rect_Float.SidePoints[(j + 1) > 3 ? 0 : j + 1], contour.GetContourBit(y).GetPoint(p));
                                if (z[j] == 0)
                                {
                                    if (xpoint >= rect_Float.SidePoints[j].GetX() && xpoint <= rect_Float.SidePoints[(j + 1) > 4 ? 0 : j + 1].GetX() &&
                                        ypoint >= rect_Float.SidePoints[j].GetY() && ypoint <= rect_Float.SidePoints[(j + 1) > 4 ? 0 : j + 1].GetY())
                                    {
                                        res.AddPoint(xpoint, ypoint, 0);
                                        if(!lastPointAdd)
                                            res.AddPoint(contour.GetContourBit(y).GetPoint(p-1).GetX(), contour.GetContourBit(y).GetPoint(p - 1).GetY(),0);
                                        prevPointIsCheked = true;
                                        lastPointAdd = true;
                                        break;
                                    }
                                    else
                                        break;
                                }
                                else if (j == 3)
                                {
                                    if (z[0] > 0 && z[2] < 0 && z[1] > 0 && z[3] < 0)
                                    {
                                        res.AddPoint(xpoint, ypoint, 0);
                                        if (!lastPointAdd)
                                            res.AddPoint(contour.GetContourBit(y).GetPoint(p - 1).GetX(), contour.GetContourBit(y).GetPoint(p - 1).GetY(), 0);
                                        prevPointIsCheked = true;
                                        lastPointAdd = true;
                                    }
                                    else
                                        lastPointAdd = false;
                                }

                                //contourPoints.Add(contour.GetContourBit(y).GetPoint(p));
                            }
                        }
                    }
                    if (res.GetPointCount() > 0)
                    {
                        res.SetClosed(contour.GetContourBit(y).IsClosed());
                        contourRes.AddContourBit(res);
                    }
                        
                }
                   
            }
               
            return contourRes;
        }
        public double ZCalc(IContourPoint PointSideRect1, IContourPoint PointSideRect2, IContourPoint ComparePoint)
        {
            double z = (ComparePoint.GetX() - PointSideRect1.GetX()) * (PointSideRect2.GetY() - PointSideRect1.GetY()) - (ComparePoint.GetY() - PointSideRect1.GetY())*(PointSideRect2.GetX()-PointSideRect1.GetX());
            return z;
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
