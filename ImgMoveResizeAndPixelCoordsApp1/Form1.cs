using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgMoveResizeAndPixelCoordsApp1
{
    public partial class Form1 : Form
    {
        int imgX = 0;
        int imgY = 0;
        int dx = 0;
        int dy = 0;
        bool MoveFlag = false;
        Bitmap bmp;
        Graphics g;


        Point mPos = new Point();
        Point pPos = new Point();
        //###############################################################################################################

        public Form1() { InitializeComponent(); }

        private void Form1_Load(object sender, EventArgs e)
        {
            pic.Bounds = ClientRectangle;
            pic.Image = res.KKZ_2_25p_work;

            bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            g = Graphics.FromImage(bmp);


        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dx = e.X - imgX;
                dy = e.Y - imgY;
                MoveFlag = true;
            }
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (MoveFlag)
            {
                if (res.KKZ_2_25p_work.Width > pic.Width)
                {
                    imgX = e.X - dx;
                    if (imgX > 0) imgX = 0;
                    if (imgX + res.KKZ_2_25p_work.Width < pic.Width) imgX = (res.KKZ_2_25p_work.Width - pic.Width) * -1;
                }
                else
                {
                    imgX = 0;
                }

                if (res.KKZ_2_25p_work.Height > pic.Height)
                {
                    imgY = e.Y - dy;
                    if (imgY > 0) imgY = 0;
                    if (imgY + res.KKZ_2_25p_work.Height < pic.Height) imgY = (res.KKZ_2_25p_work.Height - pic.Height) * -1;
                }
                else
                {
                    imgY = 0;
                }


                Text = "imgX = " + imgX + "   imgY = " + imgY;
                if (DateTime.Now.Millisecond % 50 == 1) ShowPic();
            }
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {



            MoveFlag = false;



        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pic.Bounds = ClientRectangle;
        }

        private void ShowPic(bool ShowPos = false)
        {
            g.Clear(Color.Transparent);
            g.DrawImage(res.KKZ_2_25p_work, imgX, imgY, res.KKZ_2_25p_work.Width, res.KKZ_2_25p_work.Height);
            if (ShowPos) CalcPoint();
            pic.Image = bmp;
        }

        private void CalcPoint()
        {
            string s = "";
            s += "Cursor X = " + mPos.X + "\r\n";
            s += "Cursor Y = " + mPos.Y + "\r\n\r\n";

            s += "pic X = " + pPos.X + "\r\n";
            s += "pic Y = " + pPos.Y + "\r\n\r\n";

            Color c = Color.Red;
            if (pPos.X >= 0 && pPos.X < res.KKZ_2_25p_work.Width && pPos.Y >= 0 && pPos.Y < res.KKZ_2_25p_work.Height)
            {
                c = Color.Green;
            }

            g.DrawString(s, new Font("Arial", 12, FontStyle.Bold), new SolidBrush(c), new Point(10, 10));
        }

        private void pic_Click(object sender, EventArgs e)
        {
            mPos = ((MouseEventArgs)e).Location;
            pPos.X = mPos.X - imgX;
            pPos.Y = mPos.Y - imgY;
            ShowPic(true);
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            ShowPic();
        }
    }
}
