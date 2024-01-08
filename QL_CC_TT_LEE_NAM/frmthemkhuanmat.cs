using AForge.Video.DirectShow;
using FaceRecognition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_CC_TT_LEE_NAM
{
    public partial class frmthemkhuanmat : Form
    {
        public frmthemkhuanmat()
        {
            InitializeComponent();
        }

        FaceRec faceRec = new FaceRec();

        private void button1_Click(object sender, EventArgs e)
        {
            faceRec.openCamera(pictureBox1, pictureBox2);
            faceRec.isTrained = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string id = frm_qlTaiKhoan.idnv + "_" + txt_IDNV.Text;
            faceRec.Save_IMAGE(id);
            txt_IDNV.Text = "";
        }

        private void frmthemkhuanmat_Load(object sender, EventArgs e)
        {
            //faceRec.getPersonName(lbl_idnv);
            lbl_idnv.Text = frm_qlTaiKhoan.idnv;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogresult = MessageBox.Show("Bạn có muốn thoát!", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogresult == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void txt_IDNV_MouseClick(object sender, MouseEventArgs e)
        {
            txt_IDNV.ForeColor = Color.Black;
            txt_IDNV.Text = "";
        }
    }
}
