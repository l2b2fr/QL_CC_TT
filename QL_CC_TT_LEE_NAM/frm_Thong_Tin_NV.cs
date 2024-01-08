using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace QL_CC_TT_LEE_NAM
{
    public partial class frm_Thong_Tin_NV : Form
    {
        public frm_Thong_Tin_NV()
        {
            InitializeComponent();
        }

        ConnectData c = new ConnectData();

        private void LoadImage()
        {
            ConnectData c = new ConnectData();
            c.connect();
            string query = "select Hinh_Anh from Nhan_Vien where id_Nhan_Vien = '" + frm_Dang_Nhap.id_Nhan_Vien + "'";
            SqlCommand cmd = new SqlCommand(query, c.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (reader["Hinh_Anh"] != DBNull.Value)
                {
                    byte[] imgData = (byte[])reader["Hinh_Anh"];
                    using (MemoryStream ms = new MemoryStream(imgData))
                    {
                        pic_anh.Image = Image.FromStream(ms);
                    }
                }
            }
        }

        private void frm_Thong_Tin_NV_Load(object sender, EventArgs e)
        {
            BarcodeWriter barcodeWriter = new BarcodeWriter() { Format = BarcodeFormat.CODE_128 };
            pic_barcode.Image = barcodeWriter.Write(frm_Dang_Nhap.id_Nhan_Vien);

            c.connect();
            SqlCommand cmd = new SqlCommand("select * from Nhan_Vien where id_Nhan_Vien = '" + frm_Dang_Nhap.id_Nhan_Vien + "'", c.conn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                lbl_hoten.Text = reader["Ho_Ten"].ToString();
                lbl_idnv.Text = reader["id_Nhan_Vien"].ToString();
                lbl_sdt.Text = reader["SDT"].ToString();

                txt_manv.Text = reader["id_Nhan_Vien"].ToString();
                txt_hoten.Text = reader["Ho_Ten"].ToString();
                txt_dantoc.Text = reader["Dan_Toc"].ToString();
                DateTime dateTime = Convert.ToDateTime(reader["Ngay_Sinh"].ToString());
                txt_ngaysinh.Text = dateTime.ToString("dd-MM-yyyy");
                txt_ngoaingu.Text = reader["Ngoai_Ngu"].ToString() + " - " + reader["Point"].ToString() + " Điểm";
                txt_quequan.Text = reader["Que_Quan"].ToString();
                txt_sdt.Text = reader["SDT"].ToString();
                txt_tongiao.Text = reader["Ton_Giao"].ToString();
                txt_trinhdo.Text = reader["Trinh_Do"].ToString();
                txt_diachi.Text = reader["Dia_Chi"].ToString();

                txt_cccd.Text = reader["CCCD"].ToString();
                txt_gmail.Text = reader["GMAIL"].ToString();

            }
            reader.Close();

            SqlCommand cmd1 = new SqlCommand("select * from Nhan_Vien, Chuc_Vu, Phong_Ban, Cong_Viec where Nhan_Vien.id_Nhan_Vien = '" + frm_Dang_Nhap.id_Nhan_Vien + "' and Nhan_Vien.id_Nhan_Vien = Cong_Viec.id_Nhan_Vien and Nhan_Vien.id_Phong_Ban = Phong_Ban.id_Phong_Ban and Nhan_Vien.id_Chuc_Vu = Chuc_Vu.id_Chuc_Vu", c.conn);
            SqlDataReader reader1 = cmd1.ExecuteReader();

            if (reader1.Read())
            {
                lbl_congviec.Text = reader1["Ten_Cong_Viec"].ToString();
                lbl_chuc_vu.Text = reader1["Ten_Chuc_Vu"].ToString();
                lbl_cv.Text = reader1["Ten_Chuc_Vu"].ToString();
                lbl_phongban.Text = reader1["Ten_Phong_Ban"].ToString();
            }
            reader1.Close();

            SqlCommand command = new SqlCommand("select * from Phu_Cap where id_Phu_Cap in (select id_Phu_Cap from Nhan_Vien where id_Nhan_Vien = '" + txt_manv.Text + "')",c.conn);
            SqlDataReader reader2 = command.ExecuteReader();
            if (reader2.Read())
            {
                txt_phucap.Text = reader2["Ten_Phu_Cap"].ToString();
            }

            LoadImage();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void txt_sdt_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void cmb_Phu_Cap_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmb_Chuc_Vu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
