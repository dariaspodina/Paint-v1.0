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
        Bitmap snapshot, tempDraw;
        int width;



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
            Graphics g = picture.CreateGraphics();
            g.Clear(Color.White);
            picture.Image = null;
            snapshot = new Bitmap(picture.Width, picture.Height);
            tempDraw = (Bitmap)snapshot.Clone();
            g.Dispose();
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


            // открытие файла из меню
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                picture.Image = Image.FromFile(openFileDialog1.FileName);
                snapshot = (Bitmap)picture.Image;
                tempDraw = (Bitmap)snapshot.Clone();
            }
        }

        // закрытие программы в меню
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Выбор пользователем цвета
        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult D = colorDialog1.ShowDialog();
            if (D == DialogResult.OK)
            {
                CurrentColor = colorDialog1.Color;
            }
        }
        //Изменение толщины кисти
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            width = int.Parse(comboBox1.SelectedItem.ToString());
        }

        // сохранение рисунка
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
             if (saveFileDialog1.ShowDialog() == DialogResult.OK)
             {
                 snapshot.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
             }
        }

        // меню - NEW
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Graphics g = picture.CreateGraphics();
            g.Clear(Color.White);
            picture.Image = null;
            snapshot = new Bitmap(picture.Width, picture.Height);
            tempDraw = (Bitmap)snapshot.Clone();
            g.Dispose();
        }

        //Info menu
        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Создана в 2018 году. Спасибо за использование!", "Info", MessageBoxButtons.OK);
        }

        //рисование фигур или карандаш
        private void picture_Paint(object sender, PaintEventArgs e)
        {
            if (!radioButton4.Checked) tempDraw = (Bitmap)snapshot.Clone();

            Graphics g = Graphics.FromImage(tempDraw);
            Pen pen = new Pen(Color.Black, width);

            if (radioButton1.Checked == true) // paint rectangle
            {
                if (firstX > secondX && firstY > secondY)
                {
                    // квадрат снизу-вверх справа-налево
                    Rectangle rect = new Rectangle(secondX, secondY, firstX - secondX, firstY - secondY);
                    g.DrawRectangle(pen, rect);
                }
                else if (secondX > firstX && secondY > firstY)
                {
                    // квадрат сверху-вниз слева-направо
                    Rectangle rect = new Rectangle(firstX, firstY, secondX - firstX, secondY - firstY);
                    g.DrawRectangle(pen, rect);
                }
                else if (firstX > secondX && firstY < secondY)
                {
                    // квадрат сверху-вниз справа-налево
                    Rectangle rect = new Rectangle(secondX, firstY, Math.Abs(firstX - secondX), Math.Abs(firstY - secondY));
                    g.DrawRectangle(pen, rect);
                }
                else if (secondX > firstX && secondY < firstY)
                {
                    // квадрат снизу вверх слева-направо
                    Rectangle rect = new Rectangle(firstX, secondY, Math.Abs(secondX - firstX), Math.Abs(secondY - firstY));
                    g.DrawRectangle(pen, rect);
                }
            }
            else if (radioButton4.Checked == true) // paint pen
            {
                g.DrawLine(pen, firstX, firstY, secondX, secondY);
                 firstX = secondX;
                 firstY = secondY;
            }
            else if (radioButton3.Checked == true) // paint line
            {
                g.DrawLine(pen, firstX, firstY, secondX, secondY);

            }
            else if (radioButton2.Checked == true) // paint circle
            {
                g.DrawEllipse(pen, firstX, firstY, secondX - firstX, secondY - firstY);
            }
            pen.Dispose();
            e.Graphics.DrawImageUnscaled(tempDraw, 0, 0);
            g.Dispose();
        }

    }
}
