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
        public Contours() : this(new List<IContour>())
        {
        }
        public IContour GetContour(int idx)
        {
            if (contours != null && contours.Count > 0)
                return contours[idx];
            else throw new Exception("Массив контуров не существует");
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

    public class TContourEdit : Contour, TInterfacedObject, IContour, IContourEdit
    {
        public void AddContourBit(IContourBit contourbit)
        {
            counterBits.Add(contourbit);
        }

        public IContour CutContoursByWindow(IContours contours, TRect_Float rect_Float)
        {
            TContourBitEdit pointRes = new TContourBitEdit();
            TContourEdit contourRes = new TContourEdit();
            IContour contour;

            for (int i = 0; i < contours.GetContourCount(); i++)
            {
                contour = contours.GetContour(i);
                for (int y = 0; y < contour.GetContourBitCount(); y++)
                {
                    bool lastPointAdd = false;
                    for (int p = 0; p < contour.GetContourBit(y).GetPointCount(); p++)
                    {
                        if (p == contour.GetContourBit(y).GetPointCount() - 1 && y == contour.GetContourBitCount() - 1 && !contour.GetContourBit(y).IsClosed())
                            break;
                        IContourPoint point1 = contour.GetContourBit(y).GetPoint(p);
                        IContourPoint point2 = null;

                        if (p < contour.GetContourBit(y).GetPointCount() - 1)
                        {
                            point2 = contour.GetContourBit(y).GetPoint(p + 1);
                        }
                        else if (y < contour.GetContourBitCount() - 1 && !contour.GetContourBit(y).IsClosed())
                        {
                            point2 = contour.GetContourBit(y + 1).GetPoint(0);
                        }
                        else if (contour.GetContourBit(y).IsClosed() && p == contour.GetContourBit(y).GetPointCount() - 1)
                        {
                            point2 = contour.GetContourBit(y).GetPoint(0);
                        }

                        double[] z = new double[4];
                        for (int j = 0; j < 4; j++)
                        {
                            IContourPoint pointSide1 = rect_Float.SidePoints[j];
                            IContourPoint pointSide2 = rect_Float.SidePoints[(j + 1) > 3 ? 0 : j + 1];
                            z[j] = ZCalc(pointSide1, pointSide2, point1, point2);
                            if (z[j] < 0)
                            {
                                if(!lastPointAdd)
                                {
                                    pointRes.AddPoint(point1.GetX(), point1.GetY(), 0);
                                    lastPointAdd = true;
                                }
                                pointRes.AddPoint(point2.GetX(), point2.GetY(), 0);
                                break;
                            }
                            else if (z[j] == 0)
                            {
                                bool isCrossSide = CrossSide(pointSide1, pointSide2, point1, point2);
                                //bool isCrossSideY = CrossSide(pointSide1, pointSide2, point1.GetY(), point2.GetY());
                                if (isCrossSide)
                                {
                                    if (!lastPointAdd)
                                    {
                                        pointRes.AddPoint(point1.GetX(), point1.GetY(), 0);
                                        lastPointAdd = true;
                                    }
                                    pointRes.AddPoint(point2.GetX(), point2.GetY(), 0);
                                }
                            }
                            else if(z.Length==4)
                                lastPointAdd = false;
                        };

                    }
                    if (pointRes.GetPointCount() > 0)
                    {
                        pointRes.SetClosed(contour.GetContourBit(y).IsClosed());
                        contourRes.AddContourBit(pointRes);
                    }

                }

            }

            return contourRes;
        }
        public double ZCalc(IContourPoint PointSideRect1, IContourPoint PointSideRect2, IContourPoint ComparePoint1, IContourPoint ComparePoint2)
        {
            double x1 = Math.Min(PointSideRect1.GetX(), PointSideRect2.GetX());
            double x2 = Math.Max(PointSideRect1.GetX(), PointSideRect2.GetX());
            double y1 = Math.Min(PointSideRect1.GetY(), PointSideRect2.GetY());
            double y2 = Math.Max(PointSideRect1.GetY(), PointSideRect2.GetY());
            double x1point1 = ComparePoint1.GetX();
            double x2point2 = ComparePoint2.GetX();
            double y1point1 = ComparePoint1.GetY();
            double y2point2 = ComparePoint2.GetY();
            double a1 = y2 - y1;
            double b1 = x1 - x2;
            double c1 = -x1 * (y2 - y1) + y1 * (x2 - x1);
            double a2 = y2point2 - y1point1;
            double b2 = x1point1 - x2point2;
            double c2 = -x1point1 * (y2point2 - y1point1) + y1point1* (x2point2 - x1point1);
            double z1 = a1 * x1point1 + b1 * y1point1 + c1;
            double z2 = a1 * x2point2 + b1 * y2point2 + c1;
            double z3 = a2 * x1 + b2 * y1 + c2;
            double z4 = a2 * x2 + b2 * y2 + c2;
            return z1 * z2 * z3 * z4;
        }

        public bool CrossSide(IContourPoint pointSideRect1, IContourPoint pointSideRect2, IContourPoint point1, IContourPoint point2)
        {
            double xMaxPoint = Math.Max(point1.GetX(), point2.GetX());
            double xMinPoint = Math.Min(point1.GetX(), point2.GetX());
            double yMaxPoint = Math.Max(point1.GetY(), point2.GetY());
            double yMinPoint = Math.Min(point1.GetY(), point2.GetY());
            double xMaxSide = Math.Max(pointSideRect1.GetX(), pointSideRect2.GetX());
            double xMinSide = Math.Min(pointSideRect1.GetX(), pointSideRect2.GetX());
            double yMaxSide = Math.Max(pointSideRect1.GetY(), pointSideRect2.GetY());
            double yMinSide = Math.Min(pointSideRect1.GetY(), pointSideRect2.GetY());
            double Lx = Math.Max(xMinPoint, xMinSide);
            double Ly = Math.Max(yMinPoint, yMinSide);
            double Rx = Math.Min(xMaxPoint, xMaxSide);
            double Ry = Math.Min(yMaxPoint, yMaxSide);
            return Lx <= Rx && Ly <= Ry;
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
