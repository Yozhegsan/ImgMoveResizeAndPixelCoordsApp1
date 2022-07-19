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
    public partial class FormEqName : Form
    {
        public string NewNum = "";
        public string NewName = "";
        bool EditFlag = false;

        //################################################################

        public FormEqName()
        {
            InitializeComponent();
        }

        private void FormEqName_Load(object sender, EventArgs e)
        {
            if (!EditFlag)
            {
                txtNum.Text = "0";
                txtName.Text = "Вове обладнання";
            }
        }

        public void SetData(int num, string name)
        {
            EditFlag = true;
            txtName.Text = name;
            txtNum.Text = num.ToString();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            NewNum = txtNum.Text;
            NewName = txtName.Text;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
