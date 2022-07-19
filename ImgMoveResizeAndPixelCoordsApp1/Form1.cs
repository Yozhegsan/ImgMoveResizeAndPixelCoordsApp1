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
        Point ClickTestBeginPos = new Point();


        int CurEQ = -1;

        //###############################################################################################################

        public Form1() { InitializeComponent(); }

        private void Form1_Load(object sender, EventArgs e)
        {
            pic.Image = res.KKZ_2_25p_work;
            bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            g = Graphics.FromImage(bmp);
            ReLoadEqList();
            Form1_Resize(null, null);
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mPos = e.Location;
                ClickTestBeginPos = e.Location;
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
            pic.Left = lstAllEq.Width;
            pic.Top = 0;
            pic.Width = ClientRectangle.Width - lstAllEq.Width;
            pic.Height = ClientRectangle.Height;
        }

        private void ShowPic(bool MovePicFlag = false)
        {
            Rectangle rect = new Rectangle();
            bool ShowEQ = false;
            if (CurEQ > -1)
            {
                if (EQList.Count > 0)
                {
                    rect = EQList[CurEQ].rect;

                    // move fon tut
                    if (MovePicFlag) MovePic(rect);
                    // move fon tut


                    rect.X += imgX;
                    rect.Y += imgY;
                    ShowEQ = true;
                }
            }
            g.Clear(Color.Transparent);
            g.DrawImage(res.KKZ_2_25p_work, imgX, imgY, res.KKZ_2_25p_work.Width, res.KKZ_2_25p_work.Height);
            if (ShowEQ)
            {
                g.DrawRectangle(new Pen(Brushes.Orange, 3), rect);
                g.DrawString("" + EQList[CurEQ].id + ". " + EQList[CurEQ].name, new Font("Arial", 8, FontStyle.Bold), Brushes.Black, rect.Location);
            }
            pic.Image = bmp;
        }

        private void MovePic(Rectangle rect)
        {
            bool MoveFlag = false;
            if (rect.X < (imgX * -1)) // move right
                MoveFlag = true;
            else if (rect.X > (imgX * -1) + pic.Width - rect.Width) // move left
                MoveFlag = true;

            if (rect.Y < (imgY * -1)) // move down
                MoveFlag = true;
            else if (rect.Y > (imgY * -1) + pic.Height - rect.Height) // move up
                MoveFlag = true;
            //===================================================================================
            if (MoveFlag)
            {
                imgX = rect.X * -1 + ((pic.Width - rect.Width) / 2);
                imgY = rect.Y * -1 + ((pic.Height - rect.Height) / 2);
            }
            TestImgPos(Axes.both);
        }

        //#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=
        int AddEqEtap = 0;
        Rectangle r = new Rectangle();
        string EqListFn = Application.StartupPath + "\\EQList.txt";
        private void AddEQEnd()
        {
            FormEqName frm = new FormEqName();
            DialogResult dr = frm.ShowDialog();
            if (dr == DialogResult.OK)
            {





                try { System.IO.File.AppendAllLines(EqListFn, new string[] { "" + frm.NewNum + ";" + frm.NewName + ";" + r.X + ";" + r.Y + ";" + r.Width + ";" + r.Height }); }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                Console.Beep(1000, 200);
                ReLoadEqList();
                CurEQ = -1;
                ShowPic();
            }
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
        private void ReLoadEqList()
        {
            AllLines.Clear();
            EQList.Clear();
            tmp.Clear();

            lstAllEq.Items.Clear();
            AllLines = System.IO.File.ReadAllLines(EqListFn).ToList();
            foreach (string s in AllLines)
            {
                tmp = s.Split(';').ToList();
                EQList.Add(new Eq(int.Parse(tmp[0]), tmp[1], new Rectangle(int.Parse(tmp[2]), int.Parse(tmp[3]), int.Parse(tmp[4]), int.Parse(tmp[5]))));
                lstAllEq.Items.Add("" + tmp[0] + ". " + tmp[1]);
                tmp.Clear();
            }
        }


        //#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=


        private bool IsClick(Point MouseXY)
        {
            int x1 = MouseXY.X;
            int y1 = MouseXY.Y;
            int x2 = ClickTestBeginPos.X;
            int y2 = ClickTestBeginPos.Y;
            bool fx = false, fy = false;
            //
            if (x1 == x2 && y1 == y2) return true;
            //
            if (x1 > x2) { if (x1 - x2 < 5) fx = true; } else { if (x2 - x1 < 5) fx = true; }
            //
            if (y1 > y2) { if (y1 - y2 < 5) fy = true; } else { if (y2 - y1 < 5) fy = true; }
            //
            if (fx && fy) return true; else return false;
        }


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
                Console.Beep(1000, 200);
            }
            else if (AddEqEtap == 2)
            {
                r.Width = pPos.X - r.X;
                r.Height = pPos.Y - r.Y;
                AddEqEtap = 0;
                Console.Beep(1000, 200);
                AddEQEnd();
            }
            //
            if (IsClick(mPos))
            {
                CurEQ = -1;
                for (int i = 0; i < EQList.Count; i++)
                {
                    if (pPos.X > EQList[i].rect.X && pPos.X < EQList[i].rect.X + EQList[i].rect.Width && pPos.Y > EQList[i].rect.Y && pPos.Y < EQList[i].rect.Y + EQList[i].rect.Height)
                    {
                        CurEQ = i;
                        break;
                    }
                }
            }
            //
            lstAllEq.SelectedIndex = CurEQ;
            //ShowPic();
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

        private void lstAllEq_MouseUp(object sender, MouseEventArgs e)
        {
            lstAllEq.SelectedIndex = lstAllEq.IndexFromPoint(e.Location);
        }

        private void lstAllEq_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurEQ = lstAllEq.SelectedIndex;






            ShowPic(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddEqEtap = 1;

        }
    }
}
