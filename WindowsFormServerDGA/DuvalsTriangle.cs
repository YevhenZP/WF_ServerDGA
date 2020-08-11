using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormServerDGA
{
    public class DuvalsTriangle : MainForm
    {
        int d = 20;
        int a = 0, x1 = 0, x2 = 0, x3 = 0, y1 = 0, y2 = 0, y3 = 0;
        float[,] POINT = new float[3, 4]; //координаты точки диагностики

        //Координаты для отрисовки зоны PD 
        float[,] PD = new float[,] 
        {
            { 98, 2, 100, 0, 0},
            {100, 0, 100, 0, 0},
            { 98, 0,  98, 0, 0}
        };

        float[,] D1 = new float[,] 
        {
            {  0,   0,   0, 0, 0},
            {  0,  23,  23, 0, 0},
            { 64,  23,  87, 0, 0},
            { 87,   0,  87, 0, 0},
            {  0,   0,   0, 0, 0}
        };

        float[,] D2 = new float[,]
        {
            {  0,  23,  23, 0, 0},
            {  0,  71,  71, 0, 0},
            { 31,  40,  71, 0, 0},
            { 47,  40,  87, 0, 0},
            { 64,  23,  87, 0, 0}
        };

        float[,] DT = new float[,]
        {
            {  0,  71,  71, 0, 0},
            {  0,  85,  85, 0, 0},
            { 35,  50,  85, 0, 0},
            { 46,  50,  96, 0, 0},
            { 96,   0,  96, 0, 0},
            { 87,   0,  87, 0, 0},
            { 47,  40,  87, 0, 0},
            { 31,  40,  71, 0, 0},
            {  0,  71,  71, 0, 0}
        };

        float[,] T1 = new float[,]
        {
            { 76,  20,  96, 0, 0},
            { 80,  20, 100, 0, 0},
            { 98,   2, 100, 0, 0},
            { 98,   0,  98, 0, 0},
            { 96,   0,  96, 0, 0}
        };

        float[,] T2 = new float[,]
        {
            { 46,  50,  96, 0, 0},
            { 50,  50, 100, 0, 0},
            { 80,  20, 100, 0, 0},
            { 76,  20,  96, 0, 0}
        };

        float[,] T3 = new float[,]
        {
            {  0,  85,  85, 0, 0},
            {  0, 100, 100, 0, 0},
            { 50,  50, 100, 0, 0},
            { 35,  50,  85, 0, 0},
            {  0,  85,  85, 0, 0}
        };

        private Pen pn = null;

        public void trianleDraw(object obj)
        {
            MainForm frm1 = obj as MainForm;

            int h = frm1.pbDuvalsTriangle.Height;
            int w = frm1.pbDuvalsTriangle.Width;

            Bitmap bmp = new Bitmap(w, h);
            //MessageBox.Show($"Height: {h}, Width: {w}");
            trianglesTopsCoordinats_test(w, h);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                /// Рисование на белом фоне. Делаем заливку белым цветом
                //g.Clear(Color.White);
                Pen pn = new Pen(Color.Green, 3);
                g.DrawLine(pn, x1, y1, x2, y2);
                pn = new Pen(Color.Red, 3);
                g.DrawLine(pn, x2, y2, x3, y3);
                pn = new Pen(Color.Blue, 3);
                g.DrawLine(pn, x3, y3, x1, y1);
            }
            /// Назначаем наш Bitmap свойству Image
            frm1.pbDuvalsTriangle.Image = bmp;
        } 
        public void trianglesTopsCoordinats_test(int w, int h)
        {
            if ((w/(double)h)>1.05)
            {
                //определям размер стороны равностороннего треугольника через его высоту и высоту контрола pbDuvalsTriangle
                a = (int)(2*(h - 2 * d)/(Math.Sqrt(3)));
                //определям координаты вершин равностороннего треугольника
                x1 = (w - a) / 2;
                x2 = x1 + a;
                x3 = x1 + a / 2;
                y1 = h - 2 * d;
                y2 = y1;
                y3 = d;
            }
            else
            {
                //определям размер стороны равностороннего треугольника через ширину контрола pbDuvalsTriangle
                a = (w - 2 * d);
                int dh = (int)((a * Math.Sqrt(3)) / 2);
                //определям координаты вершин равностороннего треугольника
                x1 = d; ;
                x2 = x1 + a;
                x3 = x1 + a / 2;
                y1 = dh+(h - dh)/2;
                y2 = y1;
                y3 = (h - dh) / 2;
            }
        }
        public void triangleCleare(object obj)
        {
            MainForm frm1 = obj as MainForm;
            frm1.pbDuvalsTriangle.Image = null;
        }

        public void trianglesStartCoordinats(int w, int h)
        {
            if ((w/(double)h)>1.05)
            {
                //определям размер стороны равностороннего треугольника через его высоту и высоту контрола pbDuvalsTriangle
                a = (int)(2*(h - 2 * d)/(Math.Sqrt(3)));
                //определям координаты вершин равностороннего треугольника
                x1 = (w - a) / 2;
                y1 = h -  d;
            }
            else
            {
                //определям размер стороны равностороннего треугольника через ширину контрола pbDuvalsTriangle
                a = (w - 2 * d);
                int dh = (int)((a * Math.Sqrt(3)) / 2);
                //определям координаты вершин равностороннего треугольника
                x1 = d; ;
                y1 = dh+(h - dh)/2;
            }
            calcCoordinates();
        }

        public void calcCoordinates()
        {
            double tan210 = Math.Tan(210 * Math.PI / 180);
            double tan330 = Math.Tan(330 * Math.PI / 180);
            double cos30 = Math.Cos(30 * Math.PI / 180);

            for (int i = 0; i < PD.GetLength(0); i++)
            {
                PD[i, 3] = (float)((PD[i, 1] * tan210 - PD[i, 2] * tan330) / (tan210 - tan330) * a / 100);
                PD[i, 4] = (float)((PD[i, 0] * cos30) * a / 100);
            }

            for (int i = 0; i < D1.GetLength(0); i++)
            {
                D1[i, 3] = (float)((D1[i, 1] * tan210 - D1[i, 2] * tan330) / (tan210 - tan330)*a/100);
                D1[i, 4] = (float)((D1[i,0]* cos30)*a/100);
            }

            for (int i = 0; i < D2.GetLength(0); i++)
            {
                D2[i, 3] = (float)((D2[i, 1] * tan210 - D2[i, 2] * tan330) / (tan210 - tan330) * a / 100);
                D2[i, 4] = (float)((D2[i, 0] * cos30) * a / 100);
            }

            for (int i = 0; i < DT.GetLength(0); i++)
            {
                DT[i, 3] = (float)((DT[i, 1] * tan210 - DT[i, 2] * tan330) / (tan210 - tan330) * a / 100);
                DT[i, 4] = (float)((DT[i, 0] * cos30) * a / 100);
            }

            for (int i = 0; i < T1.GetLength(0); i++)
            {
                T1[i, 3] = (float)((T1[i, 1] * tan210 - T1[i, 2] * tan330) / (tan210 - tan330) * a / 100);
                T1[i, 4] = (float)((T1[i, 0] * cos30) * a / 100);
            }

            for (int i = 0; i < T2.GetLength(0); i++)
            {
                T2[i, 3] = (float)((T2[i, 1] * tan210 - T2[i, 2] * tan330) / (tan210 - tan330) * a / 100);
                T2[i, 4] = (float)((T2[i, 0] * cos30) * a / 100);
            }

            for (int i = 0; i < T3.GetLength(0); i++)
            {
                T3[i, 3] = (float)((T3[i, 1] * tan210 - T3[i, 2] * tan330) / (tan210 - tan330) * a / 100);
                T3[i, 4] = (float)((T3[i, 0] * cos30) * a / 100);
            }
        }

        public void drawZone(object obj)
        {
            MainForm frm1 = obj as MainForm;

            int h = frm1.pbDuvalsTriangle.Height;
            int w = frm1.pbDuvalsTriangle.Width;

            Bitmap bmp = new Bitmap(w, h);

            trianglesStartCoordinats(w, h);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;

                pn = new Pen(Color.Green, 3);

                for (int i = 0; i<D1.GetLength(0); i++)
                {
                    if (i< D1.GetLength(0)-1)
                    {
                        g.DrawLine(pn, x1+D1[i,3], y1-D1[i,4], x1+D1[i+1, 3], y1-D1[i+1, 4]);
                    }
                    else
                    {
                        g.DrawLine(pn, x1+D1[i,3], y1-D1[i,4], x1+D1[0, 3], y1-D1[0, 4]);
                    }
                }

                pn = new Pen(Color.OliveDrab, 3);

                for (int i = 0; i < D2.GetLength(0); i++)
                {
                    if (i < D2.GetLength(0) - 1)
                    {
                        g.DrawLine(pn, x1 + D2[i, 3], y1 - D2[i, 4], x1 + D2[i + 1, 3], y1 - D2[i + 1, 4]);
                    }
                    else
                    {
                        g.DrawLine(pn, x1 + D2[i, 3], y1 - D2[i, 4], x1 + D2[0, 3], y1 - D2[0, 4]);
                    }
                }

                pn = new Pen(Color.DarkBlue, 3);

                for (int i = 0; i < DT.GetLength(0); i++)
                {
                    if (i < DT.GetLength(0) - 1)
                    {
                        g.DrawLine(pn, x1 + DT[i, 3], y1 - DT[i, 4], x1 + DT[i + 1, 3], y1 - DT[i + 1, 4]);
                    }
                    else
                    {
                        g.DrawLine(pn, x1 + DT[i, 3], y1 - DT[i, 4], x1 + DT[0, 3], y1 - DT[0, 4]);
                    }
                }
                pn = new Pen(Color.Gold, 3);

                for (int i = 0; i < T1.GetLength(0); i++)
                {
                    if (i < T1.GetLength(0) - 1)
                    {
                        g.DrawLine(pn, x1 + T1[i, 3], y1 - T1[i, 4], x1 + T1[i + 1, 3], y1 - T1[i + 1, 4]);
                    }
                    else
                    {
                        g.DrawLine(pn, x1 + T1[i, 3], y1 - T1[i, 4], x1 + T1[0, 3], y1 - T1[0, 4]);
                    }
                }

                pn = new Pen(Color.OrangeRed, 3);

                for (int i = 0; i < T2.GetLength(0); i++)
                {
                    if (i < T2.GetLength(0) - 1)
                    {
                        g.DrawLine(pn, x1 + T2[i, 3], y1 - T2[i, 4], x1 + T2[i + 1, 3], y1 - T2[i + 1, 4]);
                    }
                    else
                    {
                        g.DrawLine(pn, x1 + T2[i, 3], y1 - T2[i, 4], x1 + T2[0, 3], y1 - T2[0, 4]);
                    }
                }

                pn = new Pen(Color.DarkRed, 3);

                for (int i = 0; i < T3.GetLength(0); i++)
                {
                    if (i < T3.GetLength(0) - 1)
                    {
                        g.DrawLine(pn, x1 + T3[i, 3], y1 - T3[i, 4], x1 + T3[i + 1, 3], y1 - T3[i + 1, 4]);
                    }
                    else
                    {
                        g.DrawLine(pn, x1 + T3[i, 3], y1 - T3[i, 4], x1 + T3[0, 3], y1 - T3[0, 4]);
                    }
                }

                pn = new Pen(Color.Black, 3);

                for (int i = 0; i < PD.GetLength(0); i++)
                {
                    if (i < PD.GetLength(0) - 1)
                    {
                        g.DrawLine(pn, x1 + PD[i, 3], y1 - PD[i, 4], x1 + PD[i + 1, 3], y1 - PD[i + 1, 4]);
                    }
                    else
                    {
                        g.DrawLine(pn, x1 + PD[i, 3], y1 - PD[i, 4], x1 + PD[0, 3], y1 - PD[0, 4]);
                    }
                }
                //getLocation();

                //a / 100 - коэффициент маштаба (для правильной отрисовки треугольника, в зависимости от изменения размеров формы)
                pn = new Pen(Color.Blue, 1);
                g.DrawLine(pn, x1 + POINT[0, 0] * a / 100, y1 - POINT[0, 1] * a / 100, x1 + POINT[0, 2] * a / 100, y1 - POINT[0, 3] * a / 100);
                pn = new Pen(Color.Red, 1);
                g.DrawLine(pn, x1 + POINT[1, 0] * a / 100, y1 - POINT[1, 1] * a / 100, x1 + POINT[1, 2] * a / 100, y1 - POINT[1, 3] * a / 100);
                pn = new Pen(Color.Green, 1);
                g.DrawLine(pn, x1 + POINT[2, 0] * a / 100, y1 - POINT[2, 1] * a / 100, x1 + POINT[2, 2] * a / 100, y1 - POINT[2, 3] * a / 100);


            }
            // Назначаем наш Bitmap свойству Image
            frm1.pbDuvalsTriangle.Image = bmp;
        }

        public void getLocation(double ch4, double c2h4, double c2h2)
        {
            //Значения газов в ppm
            double ch4_ppm = 0, c2h4_ppm = 0, c2h2_ppm = 0;

            //Значения газов в %
            //ch4 = 0, c2h4 = 0, c2h2 = 0;
            ch4_ppm = ch4;
            c2h4_ppm = c2h4;
            c2h2_ppm = c2h2;

            double sumCH_ppm = ch4_ppm + c2h4_ppm + c2h2_ppm;

            ch4 = ((ch4_ppm / sumCH_ppm) * 100);
            c2h4 = ((c2h4_ppm / sumCH_ppm) * 100);
            c2h2 = ((c2h2_ppm / sumCH_ppm) * 100);

            //CH4
            POINT[0, 0] = (float)(ch4 * Math.Sin(30 * Math.PI / 180));
            POINT[0, 1] = (float)(ch4 * Math.Cos(30 * Math.PI / 180));

            double numerator = (100 - c2h2) * Math.Tan(210 * Math.PI / 180) - c2h4 * Math.Tan(330 * Math.PI / 180);
            double denominator = Math.Tan(210 * Math.PI / 180) - Math.Tan(330 * Math.PI / 180);
            POINT[0, 2] = (float)(numerator / denominator);

            POINT[0, 3] = (float)(ch4 * Math.Cos(30 * Math.PI / 180));

            //C2H4
            POINT[1, 0] = (float)(50 + c2h4 * Math.Sin(30 * Math.PI / 180));
            POINT[1, 1] = (float)(86.6 - c2h4 * Math.Cos(30 * Math.PI / 180));

            numerator = (100 - c2h2) * Math.Tan(210 * Math.PI / 180) - c2h4 * Math.Tan(330 * Math.PI / 180);
            denominator = Math.Tan(210 * Math.PI / 180) - Math.Tan(330 * Math.PI / 180);
            POINT[1, 2] = (float)(numerator / denominator);
            POINT[1, 3] = (float)(ch4 * Math.Cos(30 * Math.PI / 180));

            //C2H2
            POINT[2, 0] = (float)((100 - c2h2));
            POINT[2, 1] = 0;

            numerator = (100 - c2h2) * Math.Tan(210 * Math.PI / 180) - c2h4 * Math.Tan(330 * Math.PI / 180);
            denominator = Math.Tan(210 * Math.PI / 180) - Math.Tan(330 * Math.PI / 180);
            POINT[2, 2] = (float)(numerator / denominator);
            POINT[2, 3] = (float)(ch4 * Math.Cos(30 * Math.PI / 180));


        }

    }
}
