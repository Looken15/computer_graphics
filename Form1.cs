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
        double h, s, v;
        Color ColorFromHSV(int H, int S, int V)
        {
            int Hi = (int)Math.Floor(H / 60.0) % 6;

            double f = H / 60.0 - Math.Floor(H / 60.0);
            int p = (int)(V * (1 - S));
            int q = (int)(V * (1 - f * S));
            int t = (int)(V * (1 - (1 - f) * S));

            switch (Hi)
            {
                case 0:
                    return Color.FromArgb(255, V, t, p);
                case 1:
                    return Color.FromArgb(255, q, V, p);
                case 2:
                    return Color.FromArgb(255, p, V, t);
                case 3:
                    return Color.FromArgb(255, p, q, V);
                case 4:
                    return Color.FromArgb(255, t, p, V);
                case 5:
                    return Color.FromArgb(255, V, p, q);
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

                    var grey1 = (int)(0.3 * c.R + 0.59 * c.G + 0.11 * c.B);
                    var grey2 = (int)(0.21 * c.R + 0.72 * c.G + 0.07 * c.B);

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
            h = trackBar1.Value / trackBar1.Maximum;
            RedrawHSV();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            s = trackBar2.Value / trackBar2.Maximum;
            RedrawHSV();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            v = trackBar3.Value / trackBar3.Maximum;
            RedrawHSV();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RedrawHSV();
        }
    }
}
