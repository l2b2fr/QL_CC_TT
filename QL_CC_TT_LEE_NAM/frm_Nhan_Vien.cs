using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_CC_TT_LEE_NAM
{
    public partial class frm_Nhan_Vien : Form
    {
        public frm_Nhan_Vien()
        {
            InitializeComponent();
        }

        ConnectData c =new ConnectData();

        private void ButtonColorReset(Button button)
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

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn Có Muốn Thoát !","Thông báo",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void frm_Nhan_Vien_Load(object sender, EventArgs e)
        {

            frm_Thong_Tin_NV frm_Thong_Tin_NV_New = new frm_Thong_Tin_NV();
            frm_Thong_Tin_NV_New.TopLevel = false;
            frm_Thong_Tin_NV_New.AutoScroll = true;

            this.pal_Main_Nv.Controls.Clear();
            this.pal_Main_Nv.Controls.Add(frm_Thong_Tin_NV_New);
            frm_Thong_Tin_NV_New.Show();

            ButtonColorReset(btn_thongtin);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ButtonColorReset(btn_thoat);

            DialogResult dialogResult = MessageBox.Show("Bạn Có Muốn Thoát !", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btn_doimatkhau_Click(object sender, EventArgs e)
        {
            ButtonColorReset(btn_doimatkhau);

            DialogResult dialogResult = MessageBox.Show("Bạn Có muốn Đổi Mật Khẩu!", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
                frm_Doi_Mat_Khau frm_Doi_Mat_Khau = new frm_Doi_Mat_Khau();
                frm_Doi_Mat_Khau.Show();
            }
        }

        private void btn_dãnguat_Click(object sender, EventArgs e)
        {
            ButtonColorReset(btn_dãnguat);

            DialogResult dialogResult = MessageBox.Show("Bạn Có Đăng Xuất !", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
                frm_Dang_Nhap frm_Dang_Nhap = new frm_Dang_Nhap();
                frm_Dang_Nhap.Show();
            }
        }

        private void pal_Main_Nv_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_diemdanh_Click(object sender, EventArgs e)
        {
            ButtonColorReset(btn_diemdanh);

            c.connect();
            SqlCommand sqlCommand = new SqlCommand("select * from Cong_Viec where id_Nhan_Vien = '" + frm_Dang_Nhap.id_Nhan_Vien + "'",c.conn);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            string ck;
            while (reader.Read())
            {
                ck = reader["Loai_Cong_Viec"].ToString();

                if (ck == "Full Time")
                {
                    frm_Diem_Danh_Full frm_Diem_Danh_Full = new frm_Diem_Danh_Full();
                    frm_Diem_Danh_Full.TopLevel = false;
                    frm_Diem_Danh_Full.AutoScroll = true;

                    this.pal_Main_Nv.Controls.Clear();
                    this.pal_Main_Nv.Controls.Add(frm_Diem_Danh_Full);
                    frm_Diem_Danh_Full.Show();
                }
                else if (ck == "Part Time")
                {
                    frm_Diem_Danh_Part frm_Diem_Danh_Part = new frm_Diem_Danh_Part();
                    frm_Diem_Danh_Part.TopLevel = false;
                    frm_Diem_Danh_Part.AutoScroll = true;

                    this.pal_Main_Nv.Controls.Clear();
                    this.pal_Main_Nv.Controls.Add(frm_Diem_Danh_Part);
                    frm_Diem_Danh_Part.Show();
                }

            }   
        }
        private void btn_thongtin_Click(object sender, EventArgs e)
        {
            frm_Thong_Tin_NV frm_Thong_Tin_NV_New = new frm_Thong_Tin_NV();
            frm_Thong_Tin_NV_New.TopLevel = false;
            frm_Thong_Tin_NV_New.AutoScroll = true;

            this.pal_Main_Nv.Controls.Clear();
            this.pal_Main_Nv.Controls.Add(frm_Thong_Tin_NV_New);
            frm_Thong_Tin_NV_New.Show();
            ButtonColorReset(btn_thongtin);

        }
    }
}
