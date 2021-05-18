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
            for (int i=0; i<2; i++)
            {
                TContourBitEdit contourBitEdit = new TContourBitEdit();
                //for(int y=10; y<100; y*=2)
                //{
                //   int ran = random.Next(100);
                //    contourBitEdit.AddPoint(ran + y*3, i * 2 + ran, 0);
                //   // points.Add(new Point(ran + y * 3, i * 2 + ran));
                //    countPoint++;
                //}
                if(i==0)
                {
                    contourBitEdit.AddPoint(10, 5, 0);
                    contourBitEdit.AddPoint(10, 50, 0);
                    contourBitEdit.AddPoint(50, 100, 0);
                }
               if(i==1)
                {
                    contourBitEdit.AddPoint(150, 150, 0);
                    contourBitEdit.AddPoint(150, 50, 0);
                    contourBitEdit.AddPoint(100, 10, 0);
                }

                if (i % 2 != 0)
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

      