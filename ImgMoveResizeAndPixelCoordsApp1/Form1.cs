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
            //pic.Bounds = ClientRectangle;
            pic.Image = res.KKZ_2_25p_work;

            bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            g = Graphics.FromImage(bmp);

            LoadEqList();

        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mPos = e.Location;
                dx = e.X - imgX;
                dy = e.Y - imgY;
                MoveFlag = true;
            }
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (MoveFlag)
            {
                mPos = e.Location;
            }
        }

        enum Axes { x, y, both }
        private void TestImgPos(Axes ax)
        {
            switch (ax)
            {
                case Axes.x:
                    if (imgX + res.KKZ_2_25p_work.Width < pic.Width) imgX = (res.KKZ_2_25p_work.Width - pic.Width) * -1;
                    break;
                case Axes.y:
                    if (imgY + res.KKZ_2_25p_work.Height < pic.Height) imgY = (res.KKZ_2_25p_work.Height - pic.Height) * -1;
                    break;
                case Axes.both:
                    if (imgX + res.KKZ_2_25p_work.Width < pic.Width) imgX = (res.KKZ_2_25p_work.Width - pic.Width) * -1;
                    if (imgX > 0) imgX = 0;
                    if (imgY + res.KKZ_2_25p_work.Height < pic.Height) imgY = (res.KKZ_2_25p_work.Height - pic.Height) * -1;
                    if (imgY > 0) imgY = 0;
                    break;
            }
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            MoveFlag = false;
            GC.Collect();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //pic.Bounds = ClientRectangle;


            //TestImgPos(Axes.both);
            //ShowPic();
        }

        private void ShowPic(bool ShowPos = false)
        {
            g.Clear(Color.Transparent);
            g.DrawImage(res.KKZ_2_25p_work, imgX, imgY, res.KKZ_2_25p_work.Width, res.KKZ_2_25p_work.Height);

            Rectangle r = new Rectangle();

            if (EQList.Count > 0)
            {
                button1.Text = DateTime.Now.ToString("HH:mm:ss");
                foreach (Eq s in EQList)
                {
                    r = s.rect;
                    r.X += imgX;
                    r.Y += imgY;
                    g.DrawRectangle(new Pen(Brushes.Orange, 3), r);
                    g.DrawString(s.name, new Font("Arial", 8), Brushes.Black, r.Location);
                }



            }
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

        //#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=
        int c = 0, AddEqEtap = 0;
        string EQName = "";
        Rectangle r = new Rectangle();
        private void AddEQBegin()
        {
            c++;
            EQName = "New Eq #" + c;
            AddEqEtap = 1;
        }
        string EqListFn = Application.StartupPath + "\\EQList.txt";
        private void AddEQEnd()
        {
            try { System.IO.File.AppendAllLines(EqListFn, new string[] { "" + c + ";" + EQName + ";" + r.X + ";" + r.Y + ";" + r.Width + ";" + r.Height }); }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            Console.Beep(1000, 200);
        }
        //#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        class Eq
        {
            public int id;
            public string name;
            public Rectangle rect;
            public Eq(int id, string name, Rectangle rect)
            {
                this.id = id;
                this.name = name;
                this.rect = rect;
            }

        }
        List<string> AllLines = new List<string>();
        List<Eq> EQList = new List<Eq>();
        List<string> tmp = new List<string>();
        private void LoadEqList()
        {
            AllLines = System.IO.File.ReadAllLines(EqListFn).ToList();
            foreach (string s in AllLines)
            {
                textBox1.Text += s + "\r\n";
                tmp = s.Split(';').ToList();
                EQList.Add(new Eq(int.Parse(tmp[0]), tmp[1], new Rectangle(int.Parse(tmp[2]), int.Parse(tmp[3]), int.Parse(tmp[4]), int.Parse(tmp[5]))));
                tmp.Clear();
            }

            textBox1.Text += "\r\n";
            foreach (Eq eq in EQList)
            {
                textBox1.Text += eq.name + "   X = " + eq.rect.X + "   Y = " + eq.rect.Y + "   W = " + eq.rect.Width + "   H = " + eq.rect.Height + "\r\n";
            }
        }


        //#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=

        private void pic_Click(object sender, EventArgs e)
        {
            mPos = ((MouseEventArgs)e).Location;
            pPos.X = mPos.X - imgX;
            pPos.Y = mPos.Y - imgY;
            if (AddEqEtap == 1)
            {
                r.X = pPos.X;
                r.Y = pPos.Y;
                AddEqEtap = 2;
            }
            else if (AddEqEtap == 2)
            {
                r.Width = pPos.X - r.X;
                r.Height = pPos.Y - r.Y;
                AddEqEtap = 0;
                AddEQEnd();
            }
            ShowPic(true);
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {

        }

        private void tmrPaint_Tick(object sender, EventArgs e)
        {
            if (MoveFlag)
            {

                if (res.KKZ_2_25p_work.Width > pic.Width)
                {
                    imgX = mPos.X - dx;
                    if (imgX > 0) imgX = 0;
                    TestImgPos(Axes.x);
                }
                else
                {
                    imgX = 0;
                }

                if (res.KKZ_2_25p_work.Height > pic.Height)
                {
                    imgY = mPos.Y - dy;
                    if (imgY > 0) imgY = 0;
                    TestImgPos(Axes.y);
                }
                else
                {
                    imgY = 0;
                }
                ShowPic();
            }
            //Text = "imgX = " + imgX + "   imgY = " + imgY;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddEQBegin();
        }
    }
}
