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
    public partial class frm_Chuc_Vu : Form
    {
        public frm_Chuc_Vu()
        {
            InitializeComponent();
        }

        ConnectData c = new ConnectData();
        public string ckluu;

        public void reset()
        {
            c.connect();
            DataSet ds = new DataSet();
            string query = "select * from Chuc_Vu";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
            sqlDataAdapter.Fill(ds);
            dtg_chuc_vu.DataSource = ds.Tables[0];
            dtg_chuc_vu.ReadOnly = true;

            txt_chucvu.ReadOnly = false;
            txt_chucvu.Clear();
            txt_tenchucvu.Clear();
            btn_them.Show();
            btn_sua.Show();
            btn_xoa.Show();
            btn_luu.Hide();
            btn_huy.Hide();
            txt_chucvu.Clear();
            txt_tenchucvu.Clear();
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

        private void frm_Chuc_Vu_Load(object sender, EventArgs e)
        {
            c.connect();
            DataSet ds = new DataSet();
            string query = "select * from Chuc_Vu";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
            sqlDataAdapter.Fill(ds);
            dtg_chuc_vu.DataSource = ds.Tables[0];
            dtg_chuc_vu.ReadOnly = true;

            txt_chucvu.ReadOnly = false;
            txt_chucvu.Clear();
            txt_tenchucvu.Clear();
            btn_them.Show();
            btn_sua.Show();
            btn_xoa.Show();
            btn_luu.Hide();
            btn_huy.Hide();

        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            c.connect();
            if (ckluu == "Thêm")
            {
                SqlCommand cmd = new SqlCommand("select count(*) from Chuc_Vu where id_Chuc_Vu = '" + txt_chucvu.Text + "' and Ten_Chuc_Vu = N'" + txt_tenchucvu.Text + "'", c.conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    int ck = rdr.GetInt32(0);
                    rdr.Close();
                    if (ck == 0)
                    {
                        SqlCommand cmd1 = new SqlCommand("insert into Chuc_Vu values ('" + txt_chucvu.Text + "', '" + txt_tenchucvu.Text + "')", c.conn);
                        cmd1.ExecuteNonQuery();
                        MessageBox.Show("Thêm Thành Công !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        reset();
                    }
                    else
                    {
                        MessageBox.Show("Chức Vụ Đã Tồn Tại !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        reset();
                    }
                }
            }
            else
            {
                if (txt_chucvu.Text == "" || txt_tenchucvu.Text == "")
                {
                    MessageBox.Show("Vui lòng chọn chức vụ muốn cập nhật !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                    SqlCommand cmd2 = new SqlCommand("update Chuc_Vu set Ten_Chuc_Vu = N'" + txt_tenchucvu.Text + "' where id_Chuc_Vu = '" + txt_chucvu.Text + "'", c.conn);
                    cmd2.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật thành công !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reset();
                }


            }
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            txt_chucvu.ReadOnly = true;
            btn_them.Hide();
            btn_sua.Hide();
            btn_xoa.Hide();
            btn_luu.Show();
            btn_huy.Show();
            ckluu = "Sửa";
        }

        private void btn_huy_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            if (txt_chucvu.Text == "" || txt_tenchucvu.Text == "")
            {
                MessageBox.Show("Vui lòng chọn chức vụ muốn xóa !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có muốn xóa !", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(dialogResult == DialogResult.Yes)
                {
                    SqlCommand cmd3 = new SqlCommand("delete from Chuc_Vu where id_Chuc_Vu = '" + txt_chucvu.Text + "'", c.conn);
                    cmd3.ExecuteNonQuery();
                    MessageBox.Show("Xóa thành công !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reset();
                }
            }

        }

        private void dtg_chuc_vu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Lấy chỉ số của hàng được chọn
            int rowIndex = e.RowIndex;
            // Kiểm tra nếu chỉ số hợp lệ (không âm và không vượt quá số hàng của dgvData)
            if (rowIndex >= 0 && rowIndex < dtg_chuc_vu.Rows.Count)
            {
                // Lấy dữ liệu từ cột tương ứng của hàng được chọn và gán cho các TextBox
                txt_tenchucvu.Text = dtg_chuc_vu.Rows[rowIndex].Cells["Ten_Chuc_Vu"].Value.ToString();
                txt_chucvu.Text = dtg_chuc_vu.Rows[rowIndex].Cells["id_Chuc_Vu"].Value.ToString();


            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
