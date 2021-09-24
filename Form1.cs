using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace task2
{
    public partial class Form1 : Form
    {
        double h = 1.0, s = 1.0, v = 1.0;
        Dictionary<int, int> red = new Dictionary<int, int>(255);
        Dictionary<int, int> green = new Dictionary<int, int>(255);
        Dictionary<int, int> blue = new Dictionary<int, int>(255);

        Dictionary<int, int> grey1_map = new Dictionary<int, int>(255);
        Dictionary<int, int> grey2_map = new Dictionary<int, int>(255);

        Color ColorFromHSV(int H, int S, int V)
        {
            int Hi = (int)Math.Floor(H / 60.0) % 6;

            double f = H / 60.0 - Math.Floor(H / 60.0);
            double p = V * (1 - S);
            double q = V * (1 - f * S);
            double t = V * (1 - (1 - f) * S);

            switch (Hi)
            {
                case 0:
                    return Color.FromArgb(255, V, (int)t, (int)p);
                case 1:
                    return Color.FromArgb(255, (int)q, V, (int)p);
                case 2:
                    return Color.FromArgb(255, (int)p, V, (int)t);
                case 3:
                    return Color.FromArgb(255, (int)p, (int)q, V);
                case 4:
                    return Color.FromArgb(255, (int)t, (int)p, V);
                case 5:
                    return Color.FromArgb(255, V, (int)p, (int)q);
                default:
                    return new Color();
            }
        }

        (double, double, double) ColorToHSV(Color c)
        {
            var MAX = Math.Max(Math.Max(c.R, c.G), c.B);
            var MIN = Math.Min(Math.Min(c.R, c.G), c.B);

            var V = MAX;
            var S = MAX == 0 ? 0 : 1 - MIN / MAX;

            var H = 0;
            if (MAX != MIN)
            {
                if (MAX == c.R)
                {
                    if (c.G >= c.B)
                    {
                        H = 60 * (c.G - c.B) / (MAX - MIN);
                    }
                    else
                    {
                        H = 60 * (c.G - c.B) / (MAX - MIN) + 360;
                    }
                }
                else if (MAX == c.G)
                {
                    H = 60 * (c.B - c.R) / (MAX - MIN) + 120;
                }
                else
                {
                    H = 60 * (c.R - c.G) / (MAX - MIN) + 240;
                }
            }
            return (H, S, V);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = Environment.CurrentDirectory + "\\mona_lisa.jpg";
            pictureBox1.Load();

            for (int i = 0; i < 256; i++)
            {
                red[i] = 0;
                green[i] = 0;
                blue[i] = 0;

                grey1_map[i] = 0;
                grey2_map[i] = 0;

            }

            var bitmap1 = new Bitmap(pictureBox1.Image);
            var bitmap2 = new Bitmap(pictureBox1.Image);
            var bitmap3 = new Bitmap(pictureBox1.Image);

            var bitmap_red = new Bitmap(pictureBox1.Image);
            var bitmap_green = new Bitmap(pictureBox1.Image);
            var bitmap_blue = new Bitmap(pictureBox1.Image);

            var bitmap_hsv = new Bitmap(pictureBox1.Image);


            for (var i = 0; i < bitmap1.Width; ++i)
            {
                for (var j = 0; j < bitmap1.Height; ++j)
                {
                    var c = bitmap1.GetPixel(i, j);


                    red[c.R]++;
                    green[c.G]++;
                    blue[c.B]++;

                    var grey1 = (int)(0.3 * c.R + 0.59 * c.G + 0.11 * c.B);
                    grey1_map[grey1]++;
                    var grey2 = (int)(0.21 * c.R + 0.72 * c.G + 0.07 * c.B);
                    grey2_map[grey2]++;

                    Color new_color = Color.FromArgb(c.A, grey1, grey1, grey1);
                    bitmap1.SetPixel(i, j, new_color);

                    new_color = Color.FromArgb(c.A, grey2, grey2, grey2);
                    bitmap2.SetPixel(i, j, new_color);

                    var g = Math.Min(Math.Abs(grey1 - grey2) * 10, 255);
                    new_color = Color.FromArgb(c.A, g, g, g);
                    bitmap3.SetPixel(i, j, new_color);

                    new_color = Color.FromArgb(c.A, c.R, 0, 0);
                    bitmap_red.SetPixel(i, j, new_color);

                    new_color = Color.FromArgb(c.A, 0, c.G, 0);
                    bitmap_green.SetPixel(i, j, new_color);

                    new_color = Color.FromArgb(c.A, 0, 0, c.B);
                    bitmap_blue.SetPixel(i, j, new_color);

                    (var H, var S, var V) = ColorToHSV(c);

                    new_color = ColorFromHSV((int)H, (int)S, (int)V);
                    bitmap_hsv.SetPixel(i, j, new_color);
                }
            }

            pictureBox2.Image = bitmap1;
            pictureBox3.Image = bitmap2;
            pictureBox4.Image = bitmap3;

            pictureBox5.Image = bitmap_red;
            pictureBox6.Image = bitmap_green;
            pictureBox7.Image = bitmap_blue;

            pictureBox8.Image = bitmap_hsv;

            int w = pictureBox_hist.Width, h = pictureBox_hist.Height;
            var p = new Pen(Color.Black, 2.0f);
            var hist_bmp = new Bitmap(w, h);
            var gr = Graphics.FromImage(hist_bmp);
            gr.Clear(Color.White);



            var step = ((w - 40) * 1.0 / 255) * 50;
            var temp = 20;
            for (int i = 0; i < 255; i += 50)
            {
                gr.DrawLine(p, new Point(temp, h - 20), new Point(temp, h - 15));
                gr.DrawLine(new Pen(Color.Gray, 0.5f), new Point(temp, h - 20), new Point(temp, 20));

                gr.DrawString(i.ToString(), SystemFonts.DefaultFont, Brushes.Black, new PointF(temp, h - 15));
                temp += (int)step;
            }

            var mx = Math.Max(Math.Max(red.Values.Max(), green.Values.Max()), blue.Values.Max());
            step = (h - 40) * 1.0 / 4;
            temp = h - 20;

            for (int i = 0; i < mx; i += mx / 4)
            {
                gr.DrawLine(p, new Point(17, temp), new Point(20, temp));
                gr.DrawLine(new Pen(Color.Gray, 0.5f), new Point(20, temp), new Point(w - 20, temp));
                gr.DrawString(i.ToString(), SystemFonts.DefaultFont, Brushes.Black, new PointF(0, temp - 15));

                temp -= (int)step;
            }

            gr.DrawLine(p, new Point(20, h - 20), new Point(20, 20));
            gr.DrawLine(p, new Point(20, h - 20), new Point(w - 20, h - 20));

            var one_h = (h - 40) * 1.0 / mx;
            var one_w = (int)((w - 40) * 1.0 / 256);


            var x = 20;

            for (int i = 1; i < 256; i++)
            {

                gr.DrawLine(new Pen(Color.Red, 1.5f), new Point(x, (int)(h - 20 - red[i - 1] * one_h)), new Point(x + one_w, (int)(h - 20 - red[i] * one_h)));
                gr.DrawLine(new Pen(Color.Green, 1.5f), new Point(x, (int)(h - 20 - green[i - 1] * one_h)), new Point(x + one_w, (int)(h - 20 - green[i] * one_h)));
                gr.DrawLine(new Pen(Color.Blue, 1.5f), new Point(x, (int)(h - 20 - blue[i - 1] * one_h)), new Point(x + one_w, (int)(h - 20 - blue[i] * one_h)));
                gr.DrawLine(new Pen(Color.LightGray, 1.5f), new Point(x, (int)(h - 20 - grey1_map[i - 1] * one_h)), new Point(x + one_w, (int)(h - 20 - grey1_map[i] * one_h)));
                gr.DrawLine(new Pen(Color.DarkGray, 1.5f), new Point(x, (int)(h - 20 - grey2_map[i - 1] * one_h)), new Point(x + one_w, (int)(h - 20 - grey2_map[i] * one_h)));

                x += one_w;
            }

            pictureBox_hist.Image = hist_bmp;

        }

        private void RedrawHSV()
        {
            var bitmap = new Bitmap(pictureBox1.Image);
            for (var i = 0; i < bitmap.Width; ++i)
            {
                for (var j = 0; j < bitmap.Height; ++j)
                {
                    var c = bitmap.GetPixel(i, j);
                    (var H, var S, var V) = ColorToHSV(c);
                    H *= h;
                    S *= s;
                    V *= v;

                    c = ColorFromHSV((int)H, (int)S, (int)V);
                    bitmap.SetPixel(i, j, c);
                }
            }
            pictureBox8.Image = bitmap;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            h = trackBar1.Value * 1.0 / trackBar1.Maximum;

            RedrawHSV();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            s = trackBar2.Value * 1.0 / trackBar2.Maximum;
            RedrawHSV();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            v = trackBar3.Value * 1.0 / trackBar3.Maximum;
            RedrawHSV();
        }
    }
}