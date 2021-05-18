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
                        //если еще есть точки
                        if (p < contour.GetContourBit(y).GetPointCount() - 1)
                        {
                            point2 = contour.GetContourBit(y).GetPoint(p + 1);
                        }
                        //когда последняя точка в контурбите но контурбиты еще есть (проверка на замыкание по идеи излишне)
                        else if (y < contour.GetContourBitCount() - 1 && !contour.GetContourBit(y).IsClosed())
                        {
                            point2 = contour.GetContourBit(y + 1).GetPoint(0);
                        }
                        //когда последняя точка в последнем контурбит, контурбит замкнут
                        else if (y == contour.GetContourBitCount() - 1 && contour.GetContourBit(y).IsClosed())
                        {
                            point2 = contour.GetContourBit(0).GetPoint(0);
                        }
                        //когда последняя точка в последнем контурбит, контурбит не замкнут
                        else if (y == contour.GetContourBitCount() - 1 && !contour.GetContourBit(y).IsClosed())
                        {
                            break;
                        };

                        List<(double z, double x, double y)> z = new List<(double z, double x, double y)>();
                        for (int j = 0; j < 4; j++)
                        {
                            IContourPoint pointbuf1 = rect_Float.TrectAngle()[j];
                            IContourPoint pointbuf2 = rect_Float.TrectAngle()[(j + 1) > 3 ? 0 : j + 1];
                            IContourPoint pointSide1 = pointbuf1.GetX() + pointbuf1.GetY() < pointbuf2.GetX() + pointbuf2.GetY() ? pointbuf1 : pointbuf2;
                            IContourPoint pointSide2 = pointbuf1.GetX() + pointbuf1.GetY() > pointbuf2.GetX() + pointbuf2.GetY() ? pointbuf1 : pointbuf2;
                            // вычисление коэфф z, если z<0 то отрезки пересекаются, если z=0 лежат на одной прямой
                            z.Add(ZCalc(pointSide1, pointSide2, point1, point2));
                            if (z[j].z < 0)
                            {
                                //если пересекаются 
                                if(z[j].x >= pointSide1.GetX() && z[j].x <= pointSide2.GetX() && z[j].y >= pointSide1.GetY() && z[j].y <= pointSide2.GetY())
                                {
                                    if (!lastPointAdd)
                                    {
                                        pointRes.AddPoint(point1.GetX(), point1.GetY(), 0);
                                        lastPointAdd = true;
                                    }
                                    pointRes.AddPoint(point2.GetX(), point2.GetY(), 0);
                                    break;
                                }
                                else
                                {
                                    lastPointAdd = false;
                                }
                               
                            }
                            else if (z[j].z == 0)
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
                            else if(z.Count==4)
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

        /// <summary>
        ///  Вычисление коэфф z, если z<0 то отрезки пересекаются, если z=0 лежат на одной прямой
        /// </summary>
        /// <param name="PointSideRect1"></param>
        /// <param name="PointSideRect2"></param>
        /// <param name="ComparePoint1"></param>
        /// <param name="ComparePoint2"></param>
        /// <returns>1 член кортежа коэфф z, 2 и 3 члены кортежа это точка пересечения отрезков (при z<0)</returns>
        public (double z, double x, double y) ZCalc(IContourPoint PointSideRect1, IContourPoint PointSideRect2, IContourPoint ComparePoint1, IContourPoint ComparePoint2)
        {
            double x1 = Math.Min(PointSideRect1.GetX(), PointSideRect2.GetX());
            double x2 = Math.Max(PointSideRect1.GetX(), PointSideRect2.GetX());
            double y1 = Math.Min(PointSideRect1.GetY(), PointSideRect2.GetY());
            double y2 = Math.Max(PointSideRect1.GetY(), PointSideRect2.GetY());
            double x3 = ComparePoint1.GetX();
            double x4 = ComparePoint2.GetX();
            double y3 = ComparePoint1.GetY();
            double y4 = ComparePoint2.GetY();
            double a1 = y2 - y1;
            double b1 = x1 - x2;
            double c1 = -x1 * (y2 - y1) + y1 * (x2 - x1);
            double a2 = y4 - y3;
            double b2 = x3 - x4;
            double c2 = -x3 * (y4 - y3) + y3* (x4 - x3);
             double z1 = a1 * x3 + b1 * y3 + c1;
            double z2 = a1 * x4 + b1 * y4 + c1;
            double intersectionX = 0;
            double intersectionY = 0;
            //double z3 = a2 * x1 + b2 * y1 + c2;
            //double z4 = a2 * x2 + b2 * y2 + c2;
            // double z1 = (x3 - x1) * (y2 - y1) - (y3 - y1) * (x2 - x1);
            // double z2 = (x4 - x1) * (y2 - y1) - (y4 - y1) * (x2 - x1);
            // double z3 = (x1 - x3) * (y4 - y3) - (y1 - y3) * (x4 - x3);
            // double z4 = (x2 - x3) * (y4 - y3) - (y2 - y3) * (x4 - x3);
            if(z1* z2 <0)
            {
                intersectionX = (b1 * c2 - b2 * c1) / (b2 * a2 - b1 * a2);
                intersectionY = (a1 * c2 - a2 * c1) / (a2 * b1 - a1 * b2);
            }
            return (z1*z2, intersectionX, intersectionY);
        }


        /// <summary>
        /// если точки лежат на одной прямой, определеить пересекаются ли соответсвующие проекции на оси Y и X
        /// </summary>
        /// <param name="pointSideRect1"></param>
        /// <param name="pointSideRect2"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
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
    /// <summary>
    /// метод расширения для прямоугольника, возвращает список углов от левого нижнего до правого нижнего
    /// </summary>
    public static class Trect_Extention
    {
        public static List<IContourPoint> TrectAngle (this TRect_Float rect_Float)
        {
            return new List<IContourPoint>
            {
                new ContourPoint(rect_Float.X1, rect_Float.Y1, 0),
                new ContourPoint(rect_Float.X1, rect_Float.Y2, 0),
                 new ContourPoint(rect_Float.X2, rect_Float.Y2, 0),
                 new ContourPoint(rect_Float.X2, rect_Float.Y1, 0),
            };
        }
    }


}
