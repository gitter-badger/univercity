using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace LabGraph1
{
    public partial class Form1 : Form
    {
        private Graphics _graph;
        private Pen _penSt, _penNd, _penLn;
        private Point _stCoord, _ndCoord;
        public Form1()
        {
            _stCoord = new Point(100, 100);
            _ndCoord = new Point(300, 300);
            InitializeComponent();
        }

        public void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            #region "Локальные переменные"
            _penSt.Width = 1;
            _penNd.Width = 1;
            _penLn.Width = 1;
            _graph.Clear(Color.FromArgb(240,240,240));
            var stRad = Convert.ToSingle(st_circle_rad.Value) * 2;
            var ndRad = Convert.ToSingle(nd_circle_rad.Value) * 2;
            #endregion
            #region "Отрисовка окружностей"
            _graph.DrawEllipse(_penSt, _stCoord.X - stRad/2, _stCoord.Y - stRad/2, stRad, stRad);
            _graph.DrawEllipse(_penNd, _ndCoord.X - ndRad/2, _ndCoord.Y - ndRad/2, ndRad, ndRad);
            #endregion
            /* Матрица - объект*/
            var objec = new Matrix(2, 3,
                                   _stCoord.X, _stCoord.Y, 1,
                                   _ndCoord.X, _ndCoord.Y, 1);
            /* Смещаем СК в центр первой окружности*/
            objec.Equally(objec*new Matrix(3, 3,
                                           1,         0,     0,
                                           0,         1,     0,
                                       -_stCoord.X, -_stCoord.Y, 1));
            /* Угол между окружностями*/
            var alpha = Math.Atan2((_ndCoord.X - _stCoord.X),(_ndCoord.Y - _stCoord.Y));
            /* Поворот на расчитанный угол*/
            objec.Equally(objec*new Matrix(3, 3,
                                           Math.Cos(alpha), Math.Sin(alpha), 0,
                                          -Math.Sin(alpha), Math.Cos(alpha), 0,
                                                 0,               0,         1));
            /* Угол между касательной и осевой линией*/
            var betta = Math.Atan( (ndRad/2-stRad/2) / (objec.GetItem(1, 1)));
            if (objec.GetItem(1, 1) < Math.Abs(stRad/2 - ndRad/2 ))
                _graph.DrawString("Построить внешнюю касательную нельзя".ToString(CultureInfo.InvariantCulture),
                                  new Font(FontFamily.GenericSansSerif, 15),
                                  new SolidBrush(Color.Red), 70, 200);
            else
            {
                /* Поворот на расчитанный угол*/
                objec.Equally(objec * new Matrix(3, 3,
                                               Math.Cos(betta), Math.Sin(betta), 0,
                                               -Math.Sin(betta), Math.Cos(betta), 0,
                                               0, 0, 1));
                /* Вычисление точек касания*/
                objec.Equally(objec + new Matrix(2, 3,
                                                 stRad / 2, 0, 0,
                                                 ndRad / 2, 0, 0));
                /* Обратный поворот*/
                objec.Equally(objec * new Matrix(3, 3,
                                               Math.Cos(betta), -Math.Sin(betta), 0,
                                               Math.Sin(betta), Math.Cos(betta), 0,
                                               0, 0, 1));
                /* Обратный поворот*/
                objec.Equally(objec * new Matrix(3, 3,
                                               Math.Cos(alpha), -Math.Sin(alpha), 0,
                                               Math.Sin(alpha), Math.Cos(alpha), 0,
                                               0, 0, 1));
                /* Перемещение СК в исходное положение*/
                objec.Equally(objec * new Matrix(3, 3,
                                               1, 0, 0,
                                               0, 1, 0,
                                               _stCoord.X, _stCoord.Y, 1));
                _graph.DrawLine(_penLn, objec.GetItem(0, 0), objec.GetItem(0, 1), objec.GetItem(1, 0),
                                objec.GetItem(1, 1));
            }
            _graph.DrawLine(_penLn, _stCoord.X, _stCoord.Y, _ndCoord.X, _ndCoord.Y);             //Осевая линия
        }

        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            _penSt.Color = colorDialog1.Color;
            numericUpDown1_ValueChanged(this, EventArgs.Empty);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _penSt = new Pen(new SolidBrush(Color.Black), 1);
            _penNd = new Pen(new SolidBrush(Color.Black), 1);
            _penLn = new Pen(new SolidBrush(Color.Black), 1);
            _graph = CreateGraphics();
            _graph.SmoothingMode = SmoothingMode.HighQuality;
            numericUpDown1_ValueChanged(this, EventArgs.Empty);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            _penNd.Color = colorDialog1.Color;
            numericUpDown1_ValueChanged(this, EventArgs.Empty);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            _penLn.Color = colorDialog1.Color;
            numericUpDown1_ValueChanged(this, EventArgs.Empty);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.X <= _stCoord.X + 15 &&
                e.X >= _stCoord.X - 15 &&
                e.Y <= _stCoord.Y + 15 &&
                e.Y >= _stCoord.Y - 15)
            {
                _stCoord = e.Location;
                numericUpDown1_ValueChanged(this, null);
            }
            if (e.Button == MouseButtons.Right && e.X <= _ndCoord.X + 15 && 
                e.X >= _ndCoord.X - 15 && 
                e.Y <= _ndCoord.Y + 15 && 
                e.Y >= _ndCoord.Y - 15)
            {
                _ndCoord = e.Location;
                numericUpDown1_ValueChanged(this, null);
            }
        }
    }
}
