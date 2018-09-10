using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp2
{

    public partial class Form1 : Form
    {
        Color CurrentColor = Color.Black;
        int firstX;
        int firstY;
        int secondX;
        int secondY;
        bool moving = false;
        Graphics g;
        Bitmap snapshot, tempDraw, bmp;



        public Form1()
        {
            InitializeComponent();
            g = picture.CreateGraphics();
            snapshot = new Bitmap(picture.ClientRectangle.Width, picture.ClientRectangle.Height);
            tempDraw = (Bitmap)snapshot.Clone();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        // button CLEAR
        private void button5_Click(object sender, EventArgs e)
        {
            bmp = new Bitmap(picture.Width, picture.Height);
            picture.Image = bmp;
            Graphics g = Graphics.FromImage(bmp);
        }

        private void picture_MouseDown(object sender, MouseEventArgs e)
        {
            moving = true;
            firstX = e.X;
            firstY = e.Y;
            tempDraw = (Bitmap)snapshot.Clone();
        }

        private void picture_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics g = picture.CreateGraphics();
            if (moving)
            {
                secondX = e.X;
                secondY = e.Y;
                picture.Invalidate();
                picture.Update();
            }
        }

        private void picture_MouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
            snapshot = (Bitmap)tempDraw.Clone();
        }

        //change colllors

        //private void pictureBox4_Click(object sender, EventArgs e)
        //{
        //    PictureBox p = (PictureBox)sender;
        //    CurrentColor = p.BackColor;
        //}

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "jpg-файлы|*.jpg|gif-файлы|*.gif|png-файлы|*.png|bmp-файлы|*.bmp";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PictureBox pb = new PictureBox();
                pb.Location = new Point(0, 0);
                pb.Size = ClientSize;
                pb.Image = new Bitmap(dlg.FileName);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                Controls.Add(pb);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult D = colorDialog1.ShowDialog();
            if (D == DialogResult.OK)
            {
                CurrentColor = colorDialog1.Color;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (picture.Image != null)
            {
                snapshot = new Bitmap(picture.ClientRectangle.Width, picture.ClientRectangle.Height);
                //Bitmap bmpSave = (Bitmap)picture.Image;
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = "bmp";
                sfd.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    snapshot.Save(sfd.FileName);
                }
            }
        }

        private void picture_Paint(object sender, PaintEventArgs e)
        {
            if (!radioButton4.Checked) tempDraw = (Bitmap)snapshot.Clone();

            Graphics g = Graphics.FromImage(tempDraw);
            Pen pen = new Pen(Color.Black, 5);

            if (radioButton1.Checked == true) // paint rectangle
            {
                pen = new Pen(CurrentColor, 5);
                if (firstX > secondX && firstY > secondY)
                {
                    // квадрат снизу-вверх справа-налево
                    Rectangle rect = new Rectangle(secondX, secondY, firstX - secondX, firstY - secondY);
                    g.DrawRectangle(pen, rect);
                }
                else if (secondX > firstX && secondY > firstY)
                {
                    // сверху-вниз слева-направо
                    Rectangle rect = new Rectangle(firstX, firstY, secondX - firstX, secondY - firstY);
                    g.DrawRectangle(pen, rect);
                }
                else if (firstX > secondX && firstY < secondY)
                {
                    // сверху-вниз справа-налево
                    Rectangle rect = new Rectangle(secondX, firstY, Math.Abs(firstX - secondX), Math.Abs(firstY - secondY));
                    g.DrawRectangle(pen, rect);
                }
                else if (secondX > firstX && secondY < firstY)
                {
                    // снизу вверх слева-направо
                    Rectangle rect = new Rectangle(firstX, secondY, Math.Abs(secondX - firstX), Math.Abs(secondY - firstY));
                    g.DrawRectangle(pen, rect);
                }
            }
            else if (radioButton4.Checked == true) // paint pen
            {
                pen = new Pen(CurrentColor, 5);
                g.DrawLine(pen, firstX, firstY, secondX, secondY);
                 firstX = secondX;
                 firstY = secondY;
            }
            else if (radioButton3.Checked == true) // paint line
            {
                pen = new Pen(CurrentColor, 5);
                g.DrawLine(pen, firstX, firstY, secondX, secondY);
            }
            else if (radioButton2.Checked == true) // paint circle
            {
                pen = new Pen(CurrentColor, 5);
                g.DrawEllipse(pen, firstX, firstY, secondX - firstX, secondY - firstY);
            }
            //pen.Dispose();
            e.Graphics.DrawImageUnscaled(tempDraw, 0, 0);
            //g.Dispose();
        }

    }
}
