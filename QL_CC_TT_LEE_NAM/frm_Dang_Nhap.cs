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
    public partial class frm_Dang_Nhap : Form
    {
        public frm_Dang_Nhap()
        {
            InitializeComponent();
        }

        ConnectData c = new ConnectData();
        public static string id_Nhan_Vien;

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txt_matkhau.PasswordChar = '\0';
            }
            else
            {
                txt_matkhau.PasswordChar = '*';
            }
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            DialogResult dt = MessageBox.Show("Bạn có muốn thoát !", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dt == DialogResult.Yes)
            {
                Application.Exit();
            }

        }

        private void btn_dangnhap_Click(object sender, EventArgs e)
        {
            c.connect();
            SqlCommand com = new SqlCommand("select * from Nhan_Vien, Nguoi_Dung where Nhan_Vien.id_Nhan_Vien = Nguoi_Dung.id_Nhan_Vien and Tai_Khoan = '" + txt_taikhoan.Text + "' and Mat_Khau = '" + txt_matkhau.Text + " ' and Phan_Quyen = N'" + cmb_chucvu.Text + "'", c.conn);
            id_Nhan_Vien = Convert.ToString(com.ExecuteScalar());
            SqlDataReader reader = com.ExecuteReader();
            if (reader.Read() == false)
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu \nVui lòng chọn đúng chức vụ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_taikhoan.Text = "";
                txt_matkhau.Text = "";
                txt_taikhoan.Focus();
            }
            else if (cmb_chucvu.Text == "Nhân Viên" || cmb_chucvu.Text == "Trưởng Nhóm" || cmb_chucvu.Text == "Trưởng Phòng")
            {
                this.Hide();
                frm_Nhan_Vien frm_Nhan_Vien = new frm_Nhan_Vien();
                frm_Nhan_Vien.Show();
            }
            else if (cmb_chucvu.Text == "Quản Lý")
            {
                this.Hide();
                frm_Quan_Ly1 frm_Quan_Ly = new frm_Quan_Ly1();
                frm_Quan_Ly.Show();
            }
        }

        private void btn_quenmatkhau_Click(object sender, EventArgs e)
        {
            this.Hide();
            frm_Quen_Mat_Khau frm_Quen_Mat_Khau = new frm_Quen_Mat_Khau();
            frm_Quen_Mat_Khau.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string sqlString = "update Nguoi_Dung set Mat_Khau = 'nam123' where id_Nhan_Vien = 'NV0000001'";

            ConnectData.ExecuteNonQuery(sqlString);
        }

        private void frm_Dang_Nhap_Load(object sender, EventArgs e)
        {

        }
    }
}
