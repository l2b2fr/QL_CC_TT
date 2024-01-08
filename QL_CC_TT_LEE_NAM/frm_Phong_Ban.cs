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
    public partial class frm_Phong_Ban : Form
    {
        public frm_Phong_Ban()
        {
            InitializeComponent();
        }


        ConnectData c = new ConnectData();
        public string ckluu;

        public void reset()
        {
            c.connect();
            DataSet ds = new DataSet();
            string query = "select * from Phong_Ban";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
            sqlDataAdapter.Fill(ds);
            dtg_phonng_ban.DataSource = ds.Tables[0];
            dtg_phonng_ban.ReadOnly = true;

            txt_diachi.Clear();
            txt_lienhe.Clear();
            txt_tenphongban.Clear();
            txt_idphonngban.Clear();
            btn_them.Show();
            btn_sua.Show();
            btn_xoa.Show();
            btn_luu.Hide();
            btn_huy.Hide();
            txt_idphonngban.ReadOnly = false;
        }

        private void frm_Phong_Ban_Load(object sender, EventArgs e)
        {

            c.connect();
            DataSet ds = new DataSet();
            string query = "select * from Phong_Ban";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
            sqlDataAdapter.Fill(ds);
            dtg_phonng_ban.DataSource = ds.Tables[0];
            dtg_phonng_ban.ReadOnly = true;

            btn_them.Show();
            btn_sua.Show();
            btn_xoa.Show();
            btn_luu.Hide();
            btn_huy.Hide();
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            reset();

            btn_them.Hide();
            btn_sua.Hide();
            btn_xoa.Hide();
            btn_luu.Show();
            btn_huy.Show();
            ckluu = "Thêm";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txt_idphonngban.Text == "")
            {
                MessageBox.Show("Bạn phải nhập id phòng ban", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (ckluu == "Thêm")
                {
                    SqlCommand sqlCommand = new SqlCommand("select count(*) from Phong_Ban where id_Phong_Ban = '" + txt_idphonngban.Text + "'", c.conn);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        reader.Close();
                        if (id == 0)
                        {
                            SqlCommand cmd = new SqlCommand("insert into Phong_Ban values('" + txt_idphonngban.Text + "', N'" + txt_tenphongban.Text + "', N'" + txt_diachi.Text + "', '" + txt_lienhe.Text + "') ", c.conn);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            MessageBox.Show("Phòng Ban Đã Tồn Tại !", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            reset();
                        }
                    }
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("update Phong_Ban set Ten_Phong_Ban = N'" + txt_tenphongban.Text + "', Dia_Chi = N'" + txt_diachi.Text + "', Lien_He = '" + txt_lienhe.Text + "' where id_Phong_Ban = '" + txt_idphonngban.Text + "'", c.conn);
                    cmd.ExecuteNonQuery();
                    reset();
                }
            }
            btn_them.Show();
            btn_sua.Show();
            btn_xoa.Show();
            btn_luu.Hide();
            btn_huy.Hide();
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            txt_idphonngban.ReadOnly = true;
            btn_them.Hide();
            btn_sua.Hide();
            btn_xoa.Hide();
            btn_luu.Show();
            btn_huy.Show();
            ckluu = "Sửa";
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            c.connect();
            if (txt_idphonngban.Text == "")
            {
                MessageBox.Show("Vui lòng chọn phòng ban muốn xóa", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có muốn xóa chứ", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("delete from Phong_Ban where id_Phong_Ban = '" + txt_idphonngban.Text + "'", c.conn);
                    cmd.ExecuteNonQuery();
                }

            }
            reset();
        }

        private void btn_huy_Click(object sender, EventArgs e)
        {
            btn_them.Show();
            btn_sua.Show();
            btn_xoa.Show();
            btn_luu.Hide();
            btn_huy.Hide();
        }

        private void dtg_phonng_ban_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Lấy chỉ số của hàng được chọn
            int rowIndex = e.RowIndex;
            // Kiểm tra nếu chỉ số hợp lệ (không âm và không vượt quá số hàng của dgvData)
            if (rowIndex >= 0 && rowIndex < dtg_phonng_ban.Rows.Count)
            {
                // Lấy dữ liệu từ cột tương ứng của hàng được chọn và gán cho các TextBox
                txt_idphonngban.Text = dtg_phonng_ban.Rows[rowIndex].Cells["id_Phong_Ban"].Value.ToString();
                txt_tenphongban.Text = dtg_phonng_ban.Rows[rowIndex].Cells["Ten_Phong_Ban"].Value.ToString();
                txt_diachi.Text = dtg_phonng_ban.Rows[rowIndex].Cells["Dia_Chi"].Value.ToString();
                txt_lienhe.Text = dtg_phonng_ban.Rows[rowIndex].Cells["Lien_He"].Value.ToString();

            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }
    }
}
