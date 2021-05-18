using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CounterCount;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Paint += pictureBox1_Paint;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_Paint (object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            TContourEdit contour = new TContourEdit();
            Random random = new Random();
            int countPoint=0;
            //List<Point> points = new List<Point>();
            for (int i=0; i<1; i++)
            {
                TContourBitEdit contourBitEdit = new TContourBitEdit();
                //for(int y=10; y<100; y*=2)
                //{
                //   int ran = random.Next(100);
                //    contourBitEdit.AddPoint(ran + y*3, i * 2 + ran, 0);
                //   // points.Add(new Point(ran + y * 3, i * 2 + ran));
                //    countPoint++;
                //}
                contourBitEdit.AddPoint(50, 5, 0);
                contourBitEdit.AddPoint(50, 150, 0);
                contourBitEdit.AddPoint(150, 150, 0);
                contourBitEdit.AddPoint(150, 5, 0);

                if (i % 2 == 0)
                    contourBitEdit.SetClosed(true);
                else
                    contourBitEdit.SetClosed(false);
                contour.AddContourBit(contourBitEdit);
                
            }


            GraphicsPath path = new GraphicsPath();
            for(int i=0; i< contour.GetContourBitCount(); i++)
            {
                Point[] points = new Point[contour.GetContourBit(i).GetPointCount()];
                for (int y = 0; y < contour.GetContourBit(i).GetPointCount(); y++)
                {
                    Point newpoint = new Point((int)contour.GetContourBit(i).GetPoint(y).GetX(), (int)contour.GetContourBit(i).GetPoint(y).GetY());
                    points[y] = newpoint;
                }

                 path.AddLines(points);
            }
            
            Pen myWind = new Pen(Color.Black);
            g.DrawPath(myWind, path);
          
            TRect_Float rect_Float = new TRect_Float();
            rect_Float.X1 = 10;
            rect_Float.X2 = 100;
            rect_Float.Y1 = 10;
            rect_Float.Y2 = 100;
            myWind = new Pen(Color.Red);
            g.DrawRectangle(myWind, new Rectangle((int)rect_Float.TrectAngle()[0].GetX() ,(int)rect_Float.TrectAngle()[0].GetY(), (int)rect_Float.TrectAngle()[2].GetX(), (int)rect_Float.TrectAngle()[2].GetY()));
            Contours contours = new Contours ();
            contours.AddContour(contour);
            IContour contour1 = contour.CutContoursByWindow(contours, rect_Float);
        }

        //void GetPointRecursive(Contour contour, Point[] points, int idxcontourBit, int idxPoint, int countPoint)
        //{
        //    if(idxPoint == countPoint)
        //    {
        //        points[idxPoint] = new Point((int)contour.GetContourBit(idxcontourBit).GetPoint(idxPoint).GetX(), (int)contour.GetContourBit(idxcontourBit).GetPoint(idxPoint).GetY());
        //    }
        //    else 
        //    {
        //        if (idxcontourBit == 0)
        //        {
        //            points[idxPoint] = new Point((int)contour.GetContourBit(idxcontourBit).GetPoint(idxPoint).GetX(), (int)contour.GetContourBit(idxcontourBit).GetPoint(idxPoint).GetY());
        //        }
        //        else
        //        {
        //            idxcontourBit--;
        //            GetPointRecursive(contour, points, idxcontourBit, idxPoint, countPoint);
        //        }
                    
        //        idxPoint++;
        //        GetPointRecursive(contour, points, idxcontourBit, idxPoint, countPoint);
        //        points[idxPoint] = new Point((int)contour.GetContourBit(idxcontourBit).GetPoint(idxPoint).GetX(), (int)contour.GetContourBit(idxcontourBit).GetPoint(idxPoint).GetY());
               
        //    }

        //}
    }
}

//g.Clear(Color.Blue);
//// Create a pen to draw Ellipse
//Pen pen = new Pen(Color.Red);
//g.DrawEllipse(pen, 10, 10, 20, 20);


// myWind.Dispose();
//  g.Dispose();
// Console.Write("Укажите координаты квадрата, сначала левый верхний угол (в пикселях через пробел, например: 10 10)");
//string text = Console.ReadLine();
//string[] leftUpPoint = text.Split(" ");
//Console.Write("Теперь правый нижний угол (в пикселях через пробел, например: 20 20)");
//text = Console.ReadLine();
//string[] rightDownPoint = text.Split(" ");
//TRect_Float rect_Float = new TRect_Float(int.Parse(leftUpPoint[0]), int.Parse(leftUpPoint[1]), int.Parse(rightDownPoint[0]), int.Parse(rightDownPoint[1]));

// System.Drawing.Graphics formGraphics = this.CreateGraphics();
// g.DrawRectangle(myWind, new Rectangle (10, 10, 100, 100));
//   e.Graphics.DrawPath(new Pen(Color.FromArgb(255, 255, 0, 0), 2), path);