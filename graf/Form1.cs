using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace graf
{

    public partial class Form1 : Form
    {

        public struct Point //класс точка с координатами цветом и номером
        {
            public int X;
            public int Y;
            public Color Color;
            public int Number;
        }

        public struct Line  // класс линия с координатами номером и именем
        {
            public int PointBegin;
            public int PointEnd;
            public int Number;
            public string Name;
        }

        public struct TReestr //реестр вкоторый записываются все текущие значиения
        {
            public int TypeRecord;
            public int LineCount;
            public int PointCount;
            public int PaintBegin;
            public int PaintEnd;
        }

        int PointCount = 0;
        int LineCount = 0;
        Point[] gg = new Point[100];
        Line[] wp = new Line[100 * 100];
        TReestr[] Reestr = new TReestr[100 * 100 * 100];
        int BeginPoint = -1;
        int EndPoint = -1;
        bool[] GMATRIX = new bool[100];
        Color[] ColorMatrix = new Color[10];
        int colorch = 0;
        int reestrch = 0;


        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs kmouse)
        {
            if (radioButton1.Checked == true)
            {
                bool dostup = true;
                if (PointCount > 0)//проверка запускается если больше 1 вершины
                {
                    for (int i = 0; i < PointCount; i++)//проверка по координатам длинна вектора между вершинами
                    {
                        if (Math.Sqrt((gg[i].X - kmouse.X) * (gg[i].X - kmouse.X) + (gg[i].Y - kmouse.Y) * (gg[i].Y - kmouse.Y)) < 30)
                        {
                            dostup = false;
                            MessageBox.Show("Вершины друг на друга не ставятся", "Евгений Сергеевич не ломайте программу(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //Graphics g = pictureBox1.CreateGraphics();
                            //for (int j = 0; j < LineCount; j++)
                            // {
                            //PointF point1 = new PointF(gg[wp[j].PointBegin].X, gg[wp[j].PointBegin].Y);
                            //PointF point2 = new PointF(gg[wp[j].PointEnd].X, gg[wp[j].PointEnd].Y);
                            //g.DrawLine(new Pen(Brushes.Black, 2), point1, point2);
                            //}
                            //for (int j = 0; j < PointCount; j++)
                            //{
                            // g.DrawEllipse(new Pen(Brushes.Black, 2), gg[j].X - 15, gg[j].Y - 15, 30, 30);
                            //     g.FillEllipse(new SolidBrush(Color.LightSkyBlue), gg[j].X - 14, gg[j].Y - 14, 28, 28);
                            //g.DrawString(Convert.ToString(gg[j].Number), new Font("Arial", 12), Brushes.Black, new PointF(gg[j].X - 5, gg[j].Y - 9));
                            // }
                            break;
                        }
                    }
                }
                if (dostup == true)//проверку прошли-доступ открыли
                {
                    Graphics g = pictureBox1.CreateGraphics();//рисуем кружок и задаем ему параметры
                    gg[PointCount].X = kmouse.X;
                    gg[PointCount].Y = kmouse.Y;
                    gg[PointCount].Color = Color.LightSkyBlue;
                    gg[PointCount].Number = PointCount + 1;
                    g.DrawEllipse(new Pen(Brushes.Black, 2), kmouse.X - 15, kmouse.Y - 15, 30, 30);

                    g.DrawString(Convert.ToString(PointCount + 1), new Font("Arial", 12), Brushes.Black, new PointF(kmouse.X - 5, kmouse.Y - 9));
                    PointCount += 1;
                    reestrch += 1;//порядковый номер реестра
                    Reestr[reestrch].TypeRecord = 1;//тип бекапа для отмены
                    Reestr[reestrch].PointCount = PointCount;
                    if (reestrch > 0)
                    {
                        button1.Enabled = true;
                        button2.Enabled = true;
                        button3.Enabled = true;
                    }

                }
            }
            
            if (radioButton2.Checked == true)//определяем из какой вершины идет точка
            {
                for (int i = 0; i <= PointCount; i++)
                {
                    if ((kmouse.X - gg[i].X) * (kmouse.X - gg[i].X) + (kmouse.Y - gg[i].Y) * (kmouse.Y - gg[i].Y) <= 15 * 15)
                    {
                        BeginPoint = i;
                        break;
                    }
                    else BeginPoint = -1;
                }
            }

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                if (BeginPoint != -1)
                {
                    for (int i = 0; i <= PointCount; i++)
                    {
                        if ((e.X - gg[i].X) * (e.X - gg[i].X) + (e.Y - gg[i].Y) * (e.Y - gg[i].Y) <= 15 * 15)
                        {
                            EndPoint = i;
                            if (BeginPoint == EndPoint)
                            {
                                MessageBox.Show("В одной вершине ребро не разместить", "Евгений Сергеевич не ломайте программу(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                goto notppt;
                            }
                            if (Povtor(BeginPoint, EndPoint, LineCount, wp) == true)
                            {
                                MessageBox.Show("Два ребра сюда не поместятся", "Евгений Сергеевич не ломайте программу(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                goto notppt;
                            } 
                            if ((gg[BeginPoint].Color == Color.LightSkyBlue) || (gg[EndPoint].Color == Color.LightSkyBlue))
                                goto ppt;
                            if (gg[BeginPoint].Color != gg[EndPoint].Color) goto ppt; goto notppt;

                            ppt:
                            {
                                Graphics g = pictureBox1.CreateGraphics();
                                PointF point1 = new PointF(gg[BeginPoint].X, gg[BeginPoint].Y);
                                PointF point2 = new PointF(gg[EndPoint].X, gg[EndPoint].Y);
                                wp[LineCount].PointBegin = BeginPoint;
                                wp[LineCount].PointEnd = EndPoint;
                                wp[LineCount].Number = LineCount;
                                LineCount += 1;
                                g.DrawLine(new Pen(Brushes.Black, 2), point1, point2);

                                Color brushcol = gg[BeginPoint].Color;
                                g.DrawEllipse(new Pen(Brushes.Black, 2), gg[BeginPoint].X - 15, gg[BeginPoint].Y - 15, 30, 30);
                                g.FillEllipse(new SolidBrush(brushcol), gg[BeginPoint].X - 14, gg[BeginPoint].Y - 14, 28, 28);
                                g.DrawString(Convert.ToString(gg[BeginPoint].Number), new Font("Arial", 12), Brushes.Black, new PointF(gg[BeginPoint].X - 5, gg[BeginPoint].Y - 9));

                                brushcol = gg[EndPoint].Color;
                                g.DrawEllipse(new Pen(Brushes.Black, 2), gg[EndPoint].X - 15, gg[EndPoint].Y - 15, 30, 30);
                                g.FillEllipse(new SolidBrush(brushcol), gg[EndPoint].X - 14, gg[EndPoint].Y - 14, 28, 28);
                                g.DrawString(Convert.ToString(gg[EndPoint].Number), new Font("Arial", 12), Brushes.Black, new PointF(gg[EndPoint].X - 5, gg[EndPoint].Y - 9));

                                reestrch += 1;
                                Reestr[reestrch].TypeRecord = 2;
                                Reestr[reestrch].LineCount = LineCount;

                                if (reestrch > 0)
                                {
                                    button1.Enabled = true;
                                    button2.Enabled = true;
                                    button3.Enabled = true;
                                }

                                break;
                            }
                        }
                    }
                }
                else MessageBox.Show("Ребро без вершины, как ПМИ без математики", "Евгений Сергеевич не ломайте программу(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                notppt: { };
            }
        }

        public static bool Povtor(int n1, int n2, int lc, Line[] L)//Проверяем была ли уже такая линия
        {
            bool p = false;
            for (int i = 0; i < lc; i++)
            {
                if ((L[i].PointBegin == n1 && L[i].PointEnd == n2) || (L[i].PointEnd == n1 && L[i].PointBegin == n2))
                {
                    p = true;
                }
            }
            return p;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ColorMatrix[0] = Color.Red;
            ColorMatrix[1] = Color.Green;
            ColorMatrix[2] = Color.Blue;
            ColorMatrix[3] = Color.Yellow;
            ColorMatrix[4] = Color.Pink;
            ColorMatrix[5] = Color.Purple;
            ColorMatrix[6] = Color.Peru;
            ColorMatrix[7] = Color.Red;

            for (int i = 0; i < 100 * 100 * 100; i++)
            {
                Reestr[i].TypeRecord = 0;
                Reestr[i].LineCount = -1;
                Reestr[i].PointCount = -1;
                Reestr[i].PaintBegin = -1;
                Reestr[i].PaintEnd = -1;
            }
            reestrch = 0;
        }

        public static bool AllBrush(bool[] GM, int PC)
        {
            bool TT = false;
            for (int i = 0; i < PC; i++)
            {
                if (GM[i] == false)
                {
                    TT = true;
                }
            }
            return TT;
        }

        public static bool rebrush(int num, int lc, Line[] L, Point[] P, Color[] CM, int ch)
        {
            bool re = true;
            for (int i = 0; i < lc; i++)
            {
                if (L[i].PointBegin == num)
                {
                    if (P[L[i].PointEnd].Color == CM[ch])
                    {
                        re = false;
                        
                    }
                }
                if (L[i].PointEnd == num)
                {
                    if (P[L[i].PointBegin].Color == CM[ch])
                    {
                        re = false;
                    }
                }
            }
            return re;
        }

        public static int findbegin(int rc, TReestr[] R)
        {
            int fb = 0;
            for (int i = rc - 1; i > 0; i--)
            {
                if (R[i].TypeRecord == 3)
                {
                    fb = R[i].PaintEnd /*+ 1*/;
                    break;
                }
            }
            return fb;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            colorch = 0;
            for (int i = 0; i < PointCount; i++)
            {
                GMATRIX[i] = false;
            }
            Graphics g = pictureBox1.CreateGraphics();
            Color brushcol;
            while (AllBrush(GMATRIX, PointCount))
            {
                for (int i = 0; i < PointCount; i++)
                {
                    if (GMATRIX[i] == false)
                    {
                        if (rebrush(i, LineCount, wp, gg, ColorMatrix, colorch) == true)
                        {
                            brushcol = ColorMatrix[colorch];
                            g.DrawEllipse(new Pen(Brushes.Black, 2), gg[i].X - 15, gg[i].Y - 15, 30, 30);
                            g.FillEllipse(new SolidBrush(brushcol), gg[i].X - 14, gg[i].Y - 14, 28, 28);
                            g.DrawString(Convert.ToString(gg[i].Number), new Font("Arial", 12), Brushes.Black, new PointF(gg[i].X - 5, gg[i].Y - 9));
                            gg[i].Color = ColorMatrix[colorch];
                            GMATRIX[i] = true;
                        }
                    }
                }
                colorch += 1; 
            }

            reestrch += 1;
            Reestr[reestrch].TypeRecord = 3;
            Reestr[reestrch].PaintEnd = PointCount;
            Reestr[reestrch].PaintBegin = findbegin(reestrch, Reestr);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 100 * 100 * 100; i++)
            {
                Reestr[i].TypeRecord = 0;
                Reestr[i].LineCount = -1;
                Reestr[i].PointCount = -1;
                Reestr[i].PaintBegin = -1;
                Reestr[i].PaintEnd = -1;
            }
            reestrch = 0;

            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.LightSkyBlue);

            for (int i = 0; i < PointCount; i++)
            {
                gg[i].X = 0;
                gg[i].Y = 0;
                gg[i].Number = -1;
                gg[i].Color = Color.LightSkyBlue;
            }

            PointCount = 0;

            for (int i = 0; i < LineCount; i++)
            {
                wp[i].PointBegin = -1;
                wp[i].PointEnd = -1;
                wp[i].Number = -1;
                wp[i].Name = "";
            }

            LineCount = 0;
            radioButton1.Checked = true;
            button3.Enabled = false;
            button2.Enabled = false;
            button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)//отмена
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.LightSkyBlue);
            Color brushcol;
            //если 1-точка 2-линия 3-краска
            if (Reestr[reestrch].TypeRecord == 1)
            {
                gg[Reestr[reestrch].PointCount].X = 0;
                gg[Reestr[reestrch].PointCount].Y = 0;
                gg[Reestr[reestrch].PointCount].Number = -1;
                gg[Reestr[reestrch].PointCount].Color = Color.LightSkyBlue;
                GMATRIX[Reestr[reestrch].PointCount] = false;
                PointCount -= 1;

                Reestr[reestrch].TypeRecord = 0;
                Reestr[reestrch].LineCount = -1;
                Reestr[reestrch].PointCount = -1;
                Reestr[reestrch].PaintBegin = -1;
                Reestr[reestrch].PaintEnd = -1;
                reestrch -= 1;
            }
            else if (Reestr[reestrch].TypeRecord == 2)
            {
                wp[Reestr[reestrch].LineCount].PointBegin = -1;
                wp[Reestr[reestrch].LineCount].PointEnd = -1;
                wp[Reestr[reestrch].LineCount].Number = -1;
                wp[Reestr[reestrch].LineCount].Name = "";
                LineCount -= 1;

                Reestr[reestrch].TypeRecord = 0;
                Reestr[reestrch].LineCount = -1;
                Reestr[reestrch].PointCount = -1;
                Reestr[reestrch].PaintBegin = -1;
                Reestr[reestrch].PaintEnd = -1;
                reestrch -= 1;
            }
            else if (Reestr[reestrch].TypeRecord == 3)
            {
                for (int i = Reestr[reestrch].PaintBegin; i <= Reestr[reestrch].PaintEnd; i++)
                {
                    gg[i].Color = Color.LightSkyBlue;
                }

                Reestr[reestrch].TypeRecord = 0;
                Reestr[reestrch].LineCount = -1;
                Reestr[reestrch].PointCount = -1;
                Reestr[reestrch].PaintBegin = -1;
                Reestr[reestrch].PaintEnd = -1;
                reestrch -= 1;


            }
            else MessageBox.Show("Ошибка"," ", MessageBoxButtons.OK, MessageBoxIcon.Information);


            for (int i = 0; i < LineCount; i++)
            {
                PointF point1 = new PointF(gg[wp[i].PointBegin].X, gg[wp[i].PointBegin].Y);
                PointF point2 = new PointF(gg[wp[i].PointEnd].X, gg[wp[i].PointEnd].Y);
                g.DrawLine(new Pen(Brushes.Black, 2), point1, point2);
            }
            for (int i = 0; i < PointCount; i++)
            {
                brushcol = gg[i].Color;
                g.DrawEllipse(new Pen(Brushes.Black, 2), gg[i].X - 15, gg[i].Y - 15, 30, 30);
                g.FillEllipse(new SolidBrush(brushcol), gg[i].X - 14, gg[i].Y - 14, 28, 28);
                g.DrawString(Convert.ToString(gg[i].Number), new Font("Arial", 12), Brushes.Black, new PointF(gg[i].X - 5, gg[i].Y - 9));
            }

            if (reestrch == 0)
            {
                radioButton1.Checked = true;
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            Form2 newForm = new Form2(this.button5,this.pictureBox1);
            newForm.Show();
            button5.Enabled = false;
            pictureBox1.Enabled = false;
        }
    }
}
