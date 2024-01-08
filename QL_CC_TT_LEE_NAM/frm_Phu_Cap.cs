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
    public partial class frm_Phu_Cap : Form
    {
        public frm_Phu_Cap()
        {
            InitializeComponent();
        }

        ConnectData c = new ConnectData();
        public string ckluu;

        public void reset()
        {
            c.connect();
            DataSet ds = new DataSet();
            string query = "select * from Phu_Cap";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
            sqlDataAdapter.Fill(ds);
            dtg_phu_cap.DataSource = ds.Tables[0];
            dtg_phu_cap.ReadOnly = true;

            txt_sotien.Clear();
            txt_mota.Clear();
            txt_tenphucap.Clear();
            txt_idphucap.Clear();
            btn_them.Show();
            btn_sua.Show();
            btn_xoa.Show();
            btn_luu.Hide();
            btn_huy.Hide();
            txt_idphucap.ReadOnly = false;
        }

        private void frm_Phu_Cap_Load(object sender, EventArgs e)
        {
            c.connect();
            DataSet ds = new DataSet();
            string query = "select * from Phu_Cap";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
            sqlDataAdapter.Fill(ds);
            dtg_phu_cap.DataSource = ds.Tables[0];
            dtg_phu_cap.ReadOnly = true;

            txt_sotien.Clear();
            txt_mota.Clear();
            txt_tenphucap.Clear();
            txt_idphucap.Clear();
            btn_them.Show();
            btn_sua.Show();
            btn_xoa.Show();
            btn_luu.Hide();
            btn_huy.Hide();
            txt_idphucap.ReadOnly = false;
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            ckluu = "Thêm";
            btn_them.Hide();
            btn_sua.Hide();
            btn_xoa.Hide();
            btn_luu.Show();
            btn_huy.Show();
            txt_sotien.Clear();
            txt_mota.Clear();
            txt_tenphucap.Clear();
            txt_idphucap.Clear();
            txt_idphucap.ReadOnly = false;
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            
            if (ckluu == "Thêm")
            {
                
                SqlCommand cmd = new SqlCommand("select count(*) from Phu_Cap where id_Phu_Cap = '" + txt_idphucap.Text + "'",c.conn);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.Read())
                {
                    int ck = sqlDataReader.GetInt32(0);
                    sqlDataReader.Close();
                    if(ck == 0)
                    {
                        SqlCommand cmd1 = new SqlCommand("insert into Phu_Cap values('" + txt_idphucap.Text + "',N'"  + txt_tenphucap.Text + "','" + txt_sotien.Text + "',N'" + txt_mota.Text + "')",c.conn);
                        cmd1.ExecuteNonQuery();
                        MessageBox.Show("Thêm Thành Công !","Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        reset();
                    }
                    else
                    {
                        MessageBox.Show("Phụ Cấp Đã Tồn Tại !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        reset();
                    }
                }
            }
            else
            {
                SqlCommand cmd2 = new SqlCommand("update Phu_Cap set Ten_Phu_Cap = N'" + txt_tenphucap.Text + "', So_Tien = '" + txt_sotien.Text + "', Mo_Ta = N'" + txt_mota.Text + "' where id_Phu_Cap = '" + txt_idphucap.Text + "'", c.conn);
                cmd2.ExecuteNonQuery();
                MessageBox.Show("Cập nhật thành công !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                reset();
            }
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            if(txt_idphucap.Text == "")
            {
                MessageBox.Show("Vui lòng chọn phụ cấp muốn sửa ","Thông báo", MessageBoxButtons.OK,MessageBoxIcon.Error); return;
            }
            else
            {
                btn_them.Hide();
                btn_sua.Hide();
                btn_xoa.Hide();
                btn_luu.Show();
                btn_huy.Show();
                ckluu = "Sửa";
                txt_idphucap.ReadOnly = true;
            }
            
        }

        private void btn_huy_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            if (txt_idphucap.Text == "")
            {
                MessageBox.Show("Vui lòng chọn phụ cấp muốn xóa !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                SqlCommand cmd3 = new SqlCommand("delete from Phu_Cap where id_Phu_Cap = '" + txt_idphucap.Text + "'", c.conn);
                cmd3.ExecuteNonQuery();
                MessageBox.Show("Xóa thành công !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                reset();
            }
        }

        private void dtg_phu_cap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Lấy chỉ số của hàng được chọn
            int rowIndex = e.RowIndex;
            // Kiểm tra nếu chỉ số hợp lệ (không âm và không vượt quá số hàng của dgvData)
            if (rowIndex >= 0 && rowIndex < dtg_phu_cap.Rows.Count)
            {
                // Lấy dữ liệu từ cột tương ứng của hàng được chọn và gán cho các TextBox
                txt_idphucap.Text = dtg_phu_cap.Rows[rowIndex].Cells["id_Phu_Cap"].Value.ToString();
                txt_tenphucap.Text = dtg_phu_cap.Rows[rowIndex].Cells["Ten_Phu_Cap"].Value.ToString();
                txt_sotien.Text = dtg_phu_cap.Rows[rowIndex].Cells["So_Tien"].Value.ToString();
                txt_mota.Text = dtg_phu_cap.Rows[rowIndex].Cells["Mo_Ta"].Value.ToString();

            }
        }

        private void txt_sotien_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
