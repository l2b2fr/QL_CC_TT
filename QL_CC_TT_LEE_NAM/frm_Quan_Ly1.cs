using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace QL_CC_TT_LEE_NAM
{
    public partial class frm_Quan_Ly1 : Form
    {
        public frm_Quan_Ly1()
        {
            InitializeComponent();
        }
        /*
        private void ButtonColorReset(System.Windows.Controls.Button button)
        {
            Color activeColor = Color.CornflowerBlue;
            Color btnColor = Color.SteelBlue;
            btn_thongtin.BackColor = btnColor;
            btn_thoat.BackColor = btnColor;
            btn_dãnguat.BackColor = btnColor;
            btn_doimatkhau.BackColor = btnColor;
            btn_diemdanh.BackColor = btnColor;
            button.BackColor = activeColor;
        }
        */

        private void button4_Click(object sender, EventArgs e)
        {
            frm_qrdiemdanh frm_Qrfull = new frm_qrdiemdanh();
            frm_Qrfull.AutoScroll = true;
            frm_Qrfull.TopLevel = false;

            closefrm();

            this.pal_mainql.Controls.Clear();
            this.pal_mainql.Controls.Add(frm_Qrfull);
            frm_Qrfull.Show();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn Có Muốn Thoát !", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn Có Muốn Đăng Xuất", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
                frm_Dang_Nhap frm_Dang_Nhap = new frm_Dang_Nhap();
                frm_Dang_Nhap.Show();
            }
        }

        public void closefrm()
        {
            // Lấy tất cả các form con trong panel
            var forms = pal_mainql.Controls.OfType<Form>().ToList();
            // Duyệt qua từng form và đóng nó
            foreach (var form in forms)
            {
                form.Close();
            }

        }

        private void btn_taikhoan_Click(object sender, EventArgs e)
        {
            frm_qlTaiKhoan frm_Qrfull = new frm_qlTaiKhoan();
            frm_Qrfull.AutoScroll = true;
            frm_Qrfull.TopLevel = false;

            closefrm();

            this.pal_mainql.Controls.Clear();
            this.pal_mainql.Controls.Add(frm_Qrfull);
            frm_Qrfull.Show();
        }

        private void frm_Quan_Ly1_Load(object sender, EventArgs e)
        {
            frm_qlTaiKhoan frm_Qrfull = new frm_qlTaiKhoan();
            frm_Qrfull.AutoScroll = true;
            frm_Qrfull.TopLevel = false;

            closefrm();

            this.pal_mainql.Controls.Clear();
            this.pal_mainql.Controls.Add(frm_Qrfull);
            frm_Qrfull.Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Phiên Bản 1.0\nĐược phát triển bởi Lee Nam", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_chamcong_Click(object sender, EventArgs e)
        {
            frm_Cham_Cong_Ql frm_Cham_Cong_Ql = new frm_Cham_Cong_Ql();
            frm_Cham_Cong_Ql.AutoScroll = true;
            frm_Cham_Cong_Ql.TopLevel = false;

            closefrm();

            this.pal_mainql.Controls.Clear();
            this.pal_mainql.Controls.Add(frm_Cham_Cong_Ql);
            frm_Cham_Cong_Ql.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frm_congviecluong frm_Cham_Cong_Ql = new frm_congviecluong();
            frm_Cham_Cong_Ql.AutoScroll = true;
            frm_Cham_Cong_Ql.TopLevel = false;

            closefrm();

            this.pal_mainql.Controls.Clear();
            this.pal_mainql.Controls.Add(frm_Cham_Cong_Ql);
            frm_Cham_Cong_Ql.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frm_khenthuong_kyluat frm_khenthuong = new frm_khenthuong_kyluat();
            frm_khenthuong.AutoScroll = true;
            frm_khenthuong.TopLevel = false;

            closefrm();

            this.pal_mainql.Controls.Clear();
            this.pal_mainql.Controls.Add(frm_khenthuong);
            frm_khenthuong.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            //phongban
            frm_Phong_Ban frm_khenthuong = new frm_Phong_Ban();
            frm_khenthuong.AutoScroll = true;
            frm_khenthuong.TopLevel = false;

            closefrm();

            this.pal_mainql.Controls.Clear();
            this.pal_mainql.Controls.Add(frm_khenthuong);
            frm_khenthuong.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //chucvu
            frm_Chuc_Vu frm_khenthuong = new frm_Chuc_Vu();
            frm_khenthuong.AutoScroll = true;
            frm_khenthuong.TopLevel = false;

            closefrm();

            this.pal_mainql.Controls.Clear();
            this.pal_mainql.Controls.Add(frm_khenthuong);
            frm_khenthuong.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //phucap
            frm_Phu_Cap frm_khenthuong = new frm_Phu_Cap();
            frm_khenthuong.AutoScroll = true;
            frm_khenthuong.TopLevel = false;

            closefrm();

            this.pal_mainql.Controls.Clear();
            this.pal_mainql.Controls.Add(frm_khenthuong);
            frm_khenthuong.Show();
        }

        private void pal_mainql_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_khuanmat_Click(object sender, EventArgs e)
        {
            //diemdanhkhuanmat
            frmdiemdanhkhuanmat frmdiemdanhkhuanmat = new frmdiemdanhkhuanmat();
            frmdiemdanhkhuanmat.AutoScroll = true;
            frmdiemdanhkhuanmat.TopLevel = false;

            closefrm();

            this.pal_mainql.Controls.Clear();
            this.pal_mainql.Controls.Add(frmdiemdanhkhuanmat);
            frmdiemdanhkhuanmat.Show();
        }
    }
}
