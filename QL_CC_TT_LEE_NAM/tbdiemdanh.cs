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
using System.Media;
using Microsoft.Win32;
using QL_CC_TT_LEE_NAM.Properties;

namespace QL_CC_TT_LEE_NAM
{
    public partial class tbdiemdanh : Form
    {
        public tbdiemdanh()
        {
            InitializeComponent();
        }

        ConnectData c = new ConnectData();

        private void LoadImage()
        {
            ConnectData c = new ConnectData();
            c.connect();
            string query = "select Hinh_Anh from Nhan_Vien where id_Nhan_Vien = '" + frm_qrdiemdanh.id_Nhan_Vien1 + "'";
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void tbdiemdanh_Load(object sender, EventArgs e)
        {
            c.connect();
            SqlCommand sqlCommand2 = new SqlCommand("select * from Nhan_Vien, Phong_Ban, Chuc_Vu where Nhan_Vien.id_Phong_Ban = Phong_Ban.id_Phong_Ban and Nhan_Vien.id_Chuc_Vu = Chuc_Vu.id_Chuc_Vu and id_Nhan_Vien = '" + frm_qrdiemdanh.id_Nhan_Vien1 + "'", c.conn);
            SqlDataReader sqlDataReader2 = sqlCommand2.ExecuteReader();

            while (sqlDataReader2.Read())
            {
                lbl_chucvu.Text = sqlDataReader2["Ten_Chuc_Vu"].ToString();
                lbl_hoten.Text = sqlDataReader2["Ho_Ten"].ToString();
                lbl_phongban.Text = sqlDataReader2["Ten_Phong_Ban"].ToString();
            }
            LoadImage();
            sqlDataReader2.Close();

            if(frm_qrdiemdanh.txt == true)
            {
                lbl_ttdiemdanh.Text = "Thành Công";
            }
            else
            {
                lbl_ttdiemdanh.Text = "Thất Bại";
            }

        }

        private void btn_hoanthanh_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
