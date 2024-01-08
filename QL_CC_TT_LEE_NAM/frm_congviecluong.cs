using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QL_CC_TT_LEE_NAM
{
    public partial class frm_congviecluong : Form
    {
        public frm_congviecluong()
        {
            InitializeComponent();
        }

        ConnectData c = new ConnectData();

        public string id_Luong_Full;

        private void btn_timkiem_Click(object sender, EventArgs e)
        {
            if (cmb_TKPhongBan.SelectedIndex == 0 && cmb_TKChucVu.SelectedIndex == 0)
            {
                DataSet ds = new DataSet();
                string query = "select * from Nhan_Vien,Cong_Viec where Nhan_Vien.id_Nhan_Vien = Cong_Viec.id_Nhan_Vien and Nhan_Vien.id_Nhan_Vien like N'%" + txt_TKID.Text + "%'";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
                sqlDataAdapter.Fill(ds);
                dtg_TK.DataSource = ds.Tables[0];
                dtg_TK.ReadOnly = true;
            }
            else if (cmb_TKPhongBan.SelectedIndex != 0 && cmb_TKChucVu.SelectedIndex == 0)
            {
                DataSet ds = new DataSet();
                string query = "select * from Nhan_Vien,Cong_Viec where Nhan_Vien.id_Nhan_Vien = Cong_Viec.id_Nhan_Vien and Nhan_Vien.id_Nhan_Vien like N'%" + txt_TKID.Text + "%' and id_Phong_Ban like N'%" + cmb_TKPhongBan.Text + "%'";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
                sqlDataAdapter.Fill(ds);
                dtg_TK.DataSource = ds.Tables[0];
                dtg_TK.ReadOnly = true;
            }
            else if (cmb_TKPhongBan.SelectedIndex == 0 && cmb_TKChucVu.SelectedIndex != 0)
            {
                DataSet ds = new DataSet();
                string query = "select * from Nhan_Vien,Cong_Viec where Nhan_Vien.id_Nhan_Vien = Cong_Viec.id_Nhan_Vien and Nhan_Vien.id_Nhan_Vien like N'%" + txt_TKID.Text + "%' and id_Chuc_Vu like N'%" + cmb_TKChucVu.Text + "%'";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
                sqlDataAdapter.Fill(ds);
                dtg_TK.DataSource = ds.Tables[0];
                dtg_TK.ReadOnly = true;
            }
            else if (cmb_TKPhongBan.SelectedIndex != 0 && cmb_TKChucVu.SelectedIndex != 0)
            {
                DataSet ds = new DataSet();
                string query = "select * from Nhan_Vien,Cong_Viec where Nhan_Vien.id_Nhan_Vien = Cong_Viec.id_Nhan_Vien and Nhan_Vien.id_Nhan_Vien like N'%" + txt_TKID.Text + "%' and id_Chuc_Vu like N'%" + cmb_TKChucVu.Text + "%' and id_Phong_Ban like N'%" + cmb_TKPhongBan.Text + "%'";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
                sqlDataAdapter.Fill(ds);
                dtg_TK.DataSource = ds.Tables[0];
                dtg_TK.ReadOnly = true;
            }
        }


        public void reset()
        {
            c.connect();
            SqlCommand hienthi1 = new SqlCommand("select * from Chuc_Vu", c.conn);
            SqlDataReader reader1 = hienthi1.ExecuteReader();
            while (reader1.Read())
            {
                cmb_TKChucVu.Items.Add(reader1["id_Chuc_Vu"].ToString());
                cmb_chucvulfull.Items.Add(reader1["id_Chuc_Vu"].ToString());
            }
            cmb_TKChucVu.SelectedIndex = 0;
            reader1.Close();

            SqlCommand hienthi2 = new SqlCommand("select * from Phong_Ban", c.conn);
            SqlDataReader reader2 = hienthi2.ExecuteReader();
            while (reader2.Read())
            {
                cmb_TKPhongBan.Items.Add(reader2["id_Phong_Ban"].ToString());
                cmb_phongbanlfull.Items.Add(reader2["id_Phong_Ban"].ToString());
                cmb_phongbanpart.Items.Add(reader2["id_Phong_Ban"].ToString());
            }
            cmb_TKPhongBan.SelectedIndex = 0;
            reader2.Close();


            DataSet ds = new DataSet();
            string query = "select * from Nhan_Vien,Cong_Viec where Nhan_Vien.id_Nhan_Vien = Cong_Viec.id_Nhan_Vien and Nhan_Vien.id_Nhan_Vien like N'%" + txt_TKID.Text + "%'";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
            sqlDataAdapter.Fill(ds);
            dtg_TK.DataSource = ds.Tables[0];
            dtg_TK.ReadOnly = true;

            DataSet ds1 = new DataSet();
            string query1 = "select id_Luong_Full 'Mã Lương Full',id_Chuc_Vu 'Chức Vụ',id_Phong_Ban 'Phòng Ban',Luong_Full 'Lương Cơ Bản' from Luong_Full";
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(query1, c.conn);
            sqlDataAdapter1.Fill(ds1);
            dtg_luongfull.DataSource = ds1.Tables[0];
            dtg_luongfull.ReadOnly = true;

            pal_full.Show();
            pal_part.Hide();
            btn_thietlapcv.Show();
            btn_huycv.Hide();
            btb_luucv.Hide();
            btn_theml.Show();
            btn_huyl.Hide();
            btn_luul.Hide();
            dtg_luongfull.Show();
            dtg_luongpart.Hide();
            ckb_luongfull.Checked = true;
            ckb_luongpart.Checked = false;
        }

        private void frm_congviecluong_Load(object sender, EventArgs e)
        {
            c.connect();
            SqlCommand hienthi1 = new SqlCommand("select * from Chuc_Vu", c.conn);
            SqlDataReader reader1 = hienthi1.ExecuteReader();
            while (reader1.Read())
            {
                cmb_TKChucVu.Items.Add(reader1["id_Chuc_Vu"].ToString());
                cmb_chucvulfull.Items.Add(reader1["id_Chuc_Vu"].ToString());
            }
            cmb_TKChucVu.SelectedIndex = 0;
            reader1.Close();

            SqlCommand hienthi2 = new SqlCommand("select * from Phong_Ban", c.conn);
            SqlDataReader reader2 = hienthi2.ExecuteReader();
            while (reader2.Read())
            {
                cmb_TKPhongBan.Items.Add(reader2["id_Phong_Ban"].ToString());
                cmb_phongbanlfull.Items.Add(reader2["id_Phong_Ban"].ToString());
                cmb_phongbanpart.Items.Add(reader2["id_Phong_Ban"].ToString());
            }
            cmb_TKPhongBan.SelectedIndex = 0;
            reader2.Close();


            DataSet ds = new DataSet();
            string query = "select * from Nhan_Vien,Cong_Viec where Nhan_Vien.id_Nhan_Vien = Cong_Viec.id_Nhan_Vien and Nhan_Vien.id_Nhan_Vien like N'%" + txt_TKID.Text + "%'";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
            sqlDataAdapter.Fill(ds);
            dtg_TK.DataSource = ds.Tables[0];
            dtg_TK.ReadOnly = true;

            DataSet ds1 = new DataSet();
            string query1 = "select id_Luong_Full 'Mã Lương Full',id_Chuc_Vu 'Chức Vụ',id_Phong_Ban 'Phòng Ban',Luong_Full 'Lương Cơ Bản' from Luong_Full";
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(query1, c.conn);
            sqlDataAdapter1.Fill(ds1);
            dtg_luongfull.DataSource = ds1.Tables[0];
            dtg_luongfull.ReadOnly = true;

            pal_full.Show();
            pal_part.Hide();
            btn_thietlapcv.Show();
            btn_huycv.Hide();
            btb_luucv.Hide();
            btn_theml.Show();
            btn_huyl.Hide();
            btn_luul.Hide();
            dtg_luongfull.Show();
            dtg_luongpart.Hide();
            ckb_luongfull.Checked = true;
            ckb_luongpart.Checked = false;
        }


        private void dtg_TK_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Lấy chỉ số của hàng được chọn
            int rowIndex = e.RowIndex;
            // Kiểm tra nếu chỉ số hợp lệ (không âm và không vượt quá số hàng của dgvData)
            if (rowIndex >= 0 && rowIndex < dtg_TK.Rows.Count)
            {
                // Lấy dữ liệu từ cột tương ứng của hàng được chọn và gán cho các TextBox
                txt_TKID.Text = dtg_TK.Rows[rowIndex].Cells["id_Nhan_Vien"].Value.ToString();
                txt_tencongviec.Text = dtg_TK.Rows[rowIndex].Cells["Ten_Cong_Viec"].Value.ToString();
                txt_hesoluong.Text = dtg_TK.Rows[rowIndex].Cells["He_So_Luong"].Value.ToString();
                txt_mota.Text = dtg_TK.Rows[rowIndex].Cells["Mo_Ta"].Value.ToString();
                cmb_loaicongviec.Text = dtg_TK.Rows[rowIndex].Cells["Loai_Cong_Viec"].Value.ToString();
            }
        }

        //nút sửa
        private void button3_Click(object sender, EventArgs e)
        {
            btn_huycv.Show();
            btb_luucv.Show();
            btn_thietlapcv.Hide();
            txt_tencongviec.ReadOnly = false;
            txt_mota.ReadOnly = false;



        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btn_huy_Click(object sender, EventArgs e)
        {
            btn_thietlapcv.Show();
            btn_huycv.Hide();
            btb_luucv.Hide();
        }

        public void resetfrm()
        {
            c.connect();
            SqlCommand hienthi1 = new SqlCommand("select * from Chuc_Vu", c.conn);
            SqlDataReader reader1 = hienthi1.ExecuteReader();
            while (reader1.Read())
            {
                cmb_TKChucVu.Items.Add(reader1["id_Chuc_Vu"].ToString());
                cmb_chucvulfull.Items.Add(reader1["id_Chuc_Vu"].ToString());
            }
            cmb_TKChucVu.SelectedIndex = 0;
            reader1.Close();

            SqlCommand hienthi2 = new SqlCommand("select * from Phong_Ban", c.conn);
            SqlDataReader reader2 = hienthi2.ExecuteReader();
            while (reader2.Read())
            {
                cmb_TKPhongBan.Items.Add(reader2["id_Phong_Ban"].ToString());
                cmb_phongbanlfull.Items.Add(reader2["id_Phong_Ban"].ToString());
                cmb_phongbanpart.Items.Add(reader2["id_Phong_Ban"].ToString());
            }
            cmb_TKPhongBan.SelectedIndex = 0;
            reader2.Close();

            pal_full.Show();
            pal_part.Hide();
            btn_huycv.Hide();
            btb_luucv.Hide();
            btn_thietlapcv.Show();
            txt_tencongviec.Clear();
            txt_hesoluong.Text = "";
            cmb_loaicongviec.Text = "";
            txt_mota.Clear();
            txt_TKID.Clear();

            DataSet ds = new DataSet();
            string query = "select * from Nhan_Vien,Cong_Viec where Nhan_Vien.id_Nhan_Vien = Cong_Viec.id_Nhan_Vien and Nhan_Vien.id_Nhan_Vien like N'%" + txt_TKID.Text + "%'";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
            sqlDataAdapter.Fill(ds);
            dtg_TK.DataSource = ds.Tables[0];
            dtg_TK.ReadOnly = true;

        }

        //Nút lưu cv
        private void btb_luu_Click(object sender, EventArgs e)
        {
            btn_thietlapcv.Show();
            btn_huycv.Hide();
            btb_luucv.Hide();

            c.connect();
            DialogResult = MessageBox.Show("Bạn có muốn lưu không", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DialogResult == DialogResult.Yes)
            {
                SqlCommand cmd = new SqlCommand("update Cong_Viec set Ten_Cong_Viec = N'" + txt_tencongviec.Text + "', He_So_Luong = '" + txt_hesoluong.Text + "', Loai_Cong_Viec = '" + cmb_loaicongviec.Text + "', Mo_Ta = N'" + txt_mota.Text + "' where id_Nhan_Vien = '" + txt_TKID.Text + "'", c.conn);
                cmd.ExecuteNonQuery();
                resetfrm();
            }

        }

        private void btn_theml_Click(object sender, EventArgs e)
        {
            btn_theml.Hide();
            btn_huyl.Show();
            btn_luul.Show();
        }

        //Nút lưu
        private void btn_luul_Click(object sender, EventArgs e)
        {
            btn_theml.Show();
            btn_huyl.Hide();
            btn_luul.Hide();

            if (ckb_luongfull.Checked)
            {
                cmb_chucvulfull.SelectedIndex = 0;
                cmb_phongbanlfull.SelectedIndex = 0;

                SqlCommand cmd = new SqlCommand("select count(id_Luong_Full) from Luong_Full where id_Chuc_Vu = '" + cmb_chucvulfull.Text + "' and id_Phong_Ban = '" + cmb_phongbanlfull.Text + "'", c.conn);
                SqlDataReader reader = cmd.ExecuteReader();
                int ckfull = 0;
                if (reader.Read())
                {
                    ckfull = reader.GetInt32(0);
                    reader.Close();
                    if (ckfull == 0)
                    {
                        DialogResult dialogResult = MessageBox.Show("Bạn có muốn thiết lưu thiết lập không", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            SqlCommand sqldata = new SqlCommand("insert into Luong_Full(id_Chuc_Vu,id_Phong_Ban,Luong_Full) values ('" + cmb_chucvulfull.Text + "', '" + cmb_phongbanlfull.Text + "', '" + txt_luongfull.Text + "')", c.conn);
                            sqldata.ExecuteNonQuery();
                            resetfull();
                        }
                    }
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show("Bạn có muốn thiết lưu thiết lập không", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            SqlCommand sqldata = new SqlCommand("update Luong_Full set id_Chuc_Vu = '" + cmb_chucvulfull.Text + "', id_Phong_Ban = '" + cmb_phongbanlfull.Text + "', Luong_Full = '" + txt_luongfull.Text + "' where id_Chuc_Vu = '" + cmb_chucvulfull.Text + "' and id_Phong_Ban = '" + cmb_phongbanlfull.Text + "'", c.conn);
                            sqldata.ExecuteNonQuery();
                            resetfull();
                        }
                    }
                }
            }
            else if (ckb_luongpart.Checked)
            {
                SqlCommand cmd = new SqlCommand("select count(*) from Luong_Part where id_Phong_Ban = '" + cmb_phongbanpart.Text + "'", c.conn);
                SqlDataReader reader = cmd.ExecuteReader();
                int ckfull = 0;
                if (reader.Read())
                {
                    ckfull = reader.GetInt32(0);
                    reader.Close();
                    if (ckfull == 0)
                    {
                        DialogResult dialogResult = MessageBox.Show("Bạn có muốn thiết lưu thiết lập không", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            SqlCommand sqldata = new SqlCommand("insert into Luong_Part(id_Phong_Ban,Luong_Part) values ('" + cmb_phongbanpart.Text + "', '" + txt_luongpart.Text + "')", c.conn);
                            sqldata.ExecuteNonQuery();
                            resetpart();
                        }
                    }
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show("Bạn có muốn thiết lưu thiết lập không", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            SqlCommand sqldata = new SqlCommand("update Luong_Part set Luong_Part = '" + txt_luongpart.Text + "' where id_Phong_Ban = '" + cmb_phongbanpart.Text + "'", c.conn);
                            sqldata.ExecuteNonQuery();
                            resetpart();
                        }
                    }
                }
            }
        }

        private void btn_huyl_Click(object sender, EventArgs e)
        {
            btn_theml.Show();
            btn_huyl.Hide();
            btn_luul.Hide();
        }

        private void dtg_luong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Lấy chỉ số của hàng được chọn
            int rowIndex = e.RowIndex;
            // Kiểm tra nếu chỉ số hợp lệ (không âm và không vượt quá số hàng của dgvData)
            if (rowIndex >= 0 && rowIndex < dtg_luongfull.Rows.Count)
            {
                // Lấy dữ liệu từ cột tương ứng của hàng được chọn và gán cho các TextBox
                txt_luongfull.Text = dtg_luongfull.Rows[rowIndex].Cells["Lương Cơ Bản"].Value.ToString();
                cmb_phongbanlfull.Text = dtg_luongfull.Rows[rowIndex].Cells["Phòng Ban"].Value.ToString();
                cmb_chucvulfull.Text = dtg_luongfull.Rows[rowIndex].Cells["Chức Vụ"].Value.ToString();
            }
        }

        public void resetfull()
        {
            cmb_chucvulfull.SelectedIndex = 0;
            cmb_phongbanlfull.SelectedIndex = 0;
            if (ckb_luongfull.Checked)
            {
                c.connect();
                pal_full.Show();
                pal_part.Hide();
                dtg_luongfull.Show();
                dtg_luongpart.Hide();
                ckb_luongpart.Checked = false;

                DataSet ds1 = new DataSet();
                string query1 = "select id_Luong_Full 'Mã Lương Full',id_Chuc_Vu 'Chức Vụ',id_Phong_Ban 'Phòng Ban',Luong_Full 'Lương Cơ Bản' from Luong_Full";
                SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(query1, c.conn);
                sqlDataAdapter1.Fill(ds1);
                dtg_luongfull.DataSource = ds1.Tables[0];
                dtg_luongfull.ReadOnly = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            cmb_chucvulfull.SelectedIndex = 0;
            cmb_phongbanlfull.SelectedIndex = 0;
            if (ckb_luongfull.Checked)
            {
                c.connect();
                pal_full.Show();
                pal_part.Hide();
                dtg_luongfull.Show();
                dtg_luongpart.Hide();
                ckb_luongpart.Checked = false;

                DataSet ds1 = new DataSet();
                string query1 = "select id_Luong_Full 'Mã Lương Full',id_Chuc_Vu 'Chức Vụ',id_Phong_Ban 'Phòng Ban',Luong_Full 'Lương Cơ Bản' from Luong_Full";
                SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(query1, c.conn);
                sqlDataAdapter1.Fill(ds1);
                dtg_luongfull.DataSource = ds1.Tables[0];
                dtg_luongfull.ReadOnly = true;
            }

        }



        public void resetpart()
        {
            pal_part.Show();
            pal_full.Hide();
            dtg_luongpart.Show();
            dtg_luongfull.Hide();
            ckb_luongfull.Checked = false;
            cmb_phongbanpart.SelectedIndex = 0;

            DataSet ds1 = new DataSet();
            string query1 = "select id_Luong_Part 'Mã Lương Part',id_Phong_Ban 'Phòng Ban',Luong_Part 'Lương Part' from Luong_Part";
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(query1, c.conn);
            sqlDataAdapter1.Fill(ds1);
            dtg_luongpart.DataSource = ds1.Tables[0];
            dtg_luongpart.ReadOnly = true;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_luongpart.Checked)
            {
                pal_part.Show();
                pal_full.Hide();
                dtg_luongpart.Show();
                dtg_luongfull.Hide();
                ckb_luongfull.Checked = false;
                cmb_phongbanpart.SelectedIndex = 0;

                DataSet ds1 = new DataSet();
                string query1 = "select id_Luong_Part 'Mã Lương Part',id_Phong_Ban 'Phòng Ban',Luong_Part 'Lương Part' from Luong_Part";
                SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(query1, c.conn);
                sqlDataAdapter1.Fill(ds1);
                dtg_luongpart.DataSource = ds1.Tables[0];
                dtg_luongpart.ReadOnly = true;
            }

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
