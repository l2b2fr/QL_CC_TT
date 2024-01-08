using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Xml.Linq;
using ZXing;

namespace QL_CC_TT_LEE_NAM
{
    public partial class frm_qlTaiKhoan : Form
    {
        public frm_qlTaiKhoan()
        {
            InitializeComponent();
        }

        string ckcn;
        bool ckanh;
        public static string idnv;
        ConnectData c = new ConnectData();

        private void btn_dangkytangca_Click(object sender, EventArgs e)
        {

        }

        private void palMainqltk_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frm_qlTaiKhoan_Load(object sender, EventArgs e)
        {
            c.connect();
            SqlCommand hienthi1 = new SqlCommand("select * from Chuc_Vu", c.conn);
            SqlDataReader reader1 = hienthi1.ExecuteReader();
            while (reader1.Read())
            {
                cmb_Chuc_Vu.Items.Add(reader1["id_Chuc_Vu"].ToString());
                cmb_TKChucVu.Items.Add(reader1["id_Chuc_Vu"].ToString());
            }
            cmb_Chuc_Vu.SelectedIndex = 0;
            cmb_TKChucVu.SelectedIndex = 0;
            reader1.Close();

            SqlCommand hienthi2 = new SqlCommand("select * from Phong_Ban", c.conn);
            SqlDataReader reader2 = hienthi2.ExecuteReader();
            while (reader2.Read())
            {
                cmb_Phong_Ban.Items.Add(reader2["id_Phong_Ban"].ToString());
                cmb_TKPhongBan.Items.Add(reader2["id_Phong_Ban"].ToString());
            }
            cmb_Phong_Ban.SelectedIndex = 0;
            cmb_TKPhongBan.SelectedIndex = 0;
            reader2.Close();

            SqlCommand hienthi3 = new SqlCommand("select * from Phu_Cap", c.conn);
            SqlDataReader reader3 = hienthi3.ExecuteReader();
            while (reader3.Read())
            {
                cmb_Phu_Cap.Items.Add(reader3["id_Phu_Cap"].ToString());
            }
            cmb_Phu_Cap.SelectedIndex = 0;
            reader3.Close();
            cmb_NgoaiNgu.SelectedIndex = 0;



            DataSet ds = new DataSet();
            string query = "select * from Nhan_Vien,Nguoi_Dung where Nhan_Vien.id_Nhan_Vien = Nguoi_Dung.id_Nhan_Vien";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
            sqlDataAdapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.ReadOnly = true;

            cmb_TKPhongBan.SelectedIndex = 0;
            cmb_TKChucVu.SelectedIndex = 0;
            btn_them.Show();
            btn_sua.Show();
            btn_huy.Show();
            btn_luu.Hide();
            btn_huy.Hide();
            button1.Hide();

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            btn_them.Show();
            btn_sua.Show();
            btn_xoa.Show();
            btn_luu.Hide();
            btn_huy.Hide();
        }

        public void luuanh(string idnv)
        {
            if (pic_HinhAnh.Image != null)
            {
                ConnectData c = new ConnectData();
                c.connect();
                SqlCommand th = new SqlCommand("update Nhan_Vien set Hinh_Anh=@img where id_Nhan_Vien = '" + idnv + "'", c.conn);
                MemoryStream ms = new MemoryStream();
                pic_HinhAnh.Image.Save(ms, pic_HinhAnh.Image.RawFormat);
                byte[] img = ms.ToArray();
                th.Parameters.AddWithValue("@img", img);
                int i = th.ExecuteNonQuery();
                c.conn.Close();
            }
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            c.connect();
            if (ckcn == "1")
            {
                if (cknhanvien(txt_IDNV.Text, txt_Gmail.Text, txt_taikhoan.Text, txt_gmailUser.Text) == true)
                {
                    Regex regex = new Regex(@"NV\d{7}");
                    // Biểu thức chính quy để kiểm tra chỉ cho phép nhập chữ cái tiếng Việt và không cho phép nhập số hay ký tự đặc biệt

                    // Kiểm tra xem textbox có chứa ký tự tiếng Việt hay không
                    // Sử dụng biểu thức chính quy để so khớp với các ký tự Unicode tiếng Việt
                    Regex regex1 = new Regex(@"[\u00C0-\u1EF9]");

                    if (txt_IDNV.Text.Length > 50 || !txt_IDNV.Text.All(char.IsLetterOrDigit) || !regex.IsMatch(txt_IDNV.Text))
                    {
                        // Kiểm tra nếu nội dung quá 50 ký tự hoặc có chữ có dấu hoặc ký tự đặc biệt
                        MessageBox.Show("Mã Nhân Viên Chỉ Cho Phép Nhập Theo Dạng NVxxxxxx\nKhông Cho Nhập Ký Tự Đặc Biệt", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txt_IDNV.Clear();
                    }
                    else if (ckgmail(txt_Gmail.Text) == false)
                    {
                        MessageBox.Show("Gmail Của Bạn Nhập Sai Dạng", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txt_Gmail.Clear();
                    }
                    else if (regex.IsMatch(txt_taikhoan.Text))
                    {
                        MessageBox.Show("Tài Khoản Của Bạn Nhập Sai Dạng\nKhông Cho Phép Nhập Tiếng Việt", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txt_taikhoan.Clear();
                    }
                    else if (regex.IsMatch(txt_matkhau.Text))
                    {
                        MessageBox.Show("Mật Khẩu Của Bạn Nhập Sai Dạng\nKhông Cho Phép Nhập Tiếng Việt", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txt_matkhau.Clear();
                    }
                    else if (ckgmail(txt_gmailUser.Text) == false)
                    {
                        MessageBox.Show("Gmail Của Bạn Nhập Sai Dạng", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txt_gmailUser.Clear();
                    }
                    else if (txt_taikhoan.Text == "" || txt_matkhau.Text == "")
                    {
                        MessageBox.Show("Vui Lòng Thiết Lập Tài Khoản Và Mật Khẩu Cho Tài Khoản Này", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {

                        SqlCommand cmd = new SqlCommand("insert into Nhan_Vien(id_Nhan_Vien,Ho_Ten,Gioi_Tinh,Ngay_Sinh,Que_Quan,Dia_Chi,Dan_Toc,CCCD,Gmail,SDT,Ton_Giao,Trinh_Do,Ngoai_Ngu,Point,id_Phong_Ban,id_Chuc_Vu,id_Phu_Cap) values('" + txt_IDNV.Text + "',N'" + txt_HoTen.Text + "',N'" + cmb_GioiTinh.Text + "','" + dtp_NgaySinh.Value + "',N'" + txt_QueQuan.Text + "',N'" + txt_DiaChi.Text + "',N'" + txt_DanToc.Text + "','" + txt_CCCD.Text + "','" + txt_Gmail.Text + "','" + txt_SDT.Text + "',N'" + txt_TonGiao.Text + "',N'" + cmb_TrinhDo.Text + "',N'" + cmb_NgoaiNgu.Text + "','" + txt_Point.Text + "','" + cmb_Phong_Ban.Text + "',N'" + cmb_Chuc_Vu.Text + "',N'" + cmb_Phu_Cap.Text + "')", c.conn);
                        cmd.ExecuteNonQuery();

                        SqlCommand cmd2 = new SqlCommand("insert into Cong_Viec(id_Nhan_Vien) values('" + txt_IDNV.Text + "')", c.conn);
                        cmd2.ExecuteNonQuery();

                        SqlCommand cmd1 = new SqlCommand("insert into Nguoi_Dung values('" + txt_IDNV.Text + "','" + txt_taikhoan.Text + "','" + txt_matkhau.Text + "',N'" + cmb_phanquyen.Text + "','" + txt_gmailUser.Text + "')", c.conn);
                        cmd1.ExecuteNonQuery();

                        luuanh(txt_IDNV.Text);

                        MessageBox.Show("Thêm Thành Công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        reset();
                        btn_them.Show();
                        btn_sua.Show();
                        btn_xoa.Show();
                        btn_luu.Hide();
                        btn_huy.Hide();
                    }
                }
            }
            else if (ckcn == "2")
            {
                SqlCommand cmd = new SqlCommand("update Nhan_Vien set Ho_Ten = N'" + txt_HoTen.Text + "', Gioi_Tinh = N'" + cmb_GioiTinh.Text + "', Ngay_Sinh = '" + dtp_NgaySinh.Value + "', Que_Quan = N'" + txt_QueQuan.Text + "', Dia_Chi = N'" + txt_DiaChi.Text + "', Dan_Toc = N'" + txt_DanToc.Text + "', CCCD = '" + txt_CCCD.Text + "', Gmail = '" + txt_Gmail.Text + "', SDT = '" + txt_SDT.Text + "', Ton_Giao = N'" + txt_TonGiao.Text + "', Trinh_Do = N'" + cmb_TrinhDo.Text + "', Ngoai_Ngu = N'" + cmb_NgoaiNgu.Text + "', Point = '" + txt_Point.Text + "', id_Phong_Ban = '" + cmb_Phong_Ban.Text + "', id_Chuc_Vu = N'" + cmb_Chuc_Vu.Text + "', id_Phu_Cap = N'" + cmb_Phu_Cap.Text + "' where id_Nhan_Vien='" + txt_IDNV.Text + "'", c.conn);
                cmd.ExecuteNonQuery();

                SqlCommand cmd1 = new SqlCommand("update Nguoi_Dung set Tai_Khoan = '" + txt_taikhoan.Text + "',Mat_Khau = '" + txt_matkhau.Text + "', Phan_Quyen = N'" + cmb_phanquyen.Text + "', Gmail = '" + txt_gmailUser.Text + "' where id_Nhan_Vien='" + txt_IDNV.Text + "'", c.conn);
                cmd1.ExecuteNonQuery();

                if (ckanh == true)
                {
                    luuanh(txt_IDNV.Text);
                }



                MessageBox.Show("Sửa Thành Công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                reset();
                btn_them.Show();
                btn_sua.Show();
                btn_xoa.Show();
                btn_luu.Hide();
                btn_huy.Hide();
            }
        }

        public void reset()
        {
            cmb_phanquyen.SelectedIndex = 0;
            txt_gmailUser.Clear();
            txt_matkhau.Clear();
            txt_taikhoan.Clear();
            cmb_Chuc_Vu.SelectedIndex = 0;
            cmb_Phu_Cap.SelectedIndex = 0;
            cmb_NgoaiNgu.SelectedIndex = 0;
            cmb_TrinhDo.SelectedIndex = 0;
            cmb_Phong_Ban.SelectedIndex = 0;
            txt_SDT.Clear();
            txt_Gmail.Clear();
            txt_CCCD.Clear();
            txt_TonGiao.Clear();
            txt_DiaChi.Clear();
            txt_DanToc.Clear();
            txt_QueQuan.Clear();
            cmb_GioiTinh.SelectedIndex = 0;
            txt_HoTen.Clear();
            txt_IDNV.Clear();
            pic_HinhAnh.Image = null;

            DataSet ds = new DataSet();
            string query = "select * from Nhan_Vien,Nguoi_Dung where Nhan_Vien.id_Nhan_Vien = Nguoi_Dung.id_Nhan_Vien";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
            sqlDataAdapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.ReadOnly = true;

        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            reset();
            ckcn = "1";
            btn_them.Hide();
            btn_sua.Hide();
            btn_xoa.Hide();
            btn_luu.Show();
            btn_huy.Show();
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            ckcn = "2";
            btn_them.Hide();
            btn_sua.Hide();
            btn_xoa.Hide();
            btn_luu.Show();
            btn_huy.Show();

        }

        public void khoidongfrom()
        {
            // Duyệt qua tất cả các control trên form
            foreach (Control c in this.Controls)
            {
                // Nếu là TextBox, ComboBox hoặc Label
                if (c is TextBox || c is ComboBox || c is Label)
                {
                    // Reset lại thuộc tính Text
                    c.ResetText();
                }
                // Nếu là CheckBox hoặc RadioButton
                if (c is CheckBox || c is RadioButton)
                {
                    // Reset lại thuộc tính Checked
                    ((CheckBox)c).Checked = false;
                }
                // Nếu là ListBox hoặc DataGridView
                if (c is ListBox || c is DataGridView)
                {
                    // Reset lại thuộc tính DataBindings
                    c.ResetBindings();
                }
            }
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            c.connect();
            DialogResult dialogResult = MessageBox.Show("Bạn Có Muốn Xóa Nhân Viên Này Không ?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                SqlCommand cmd3 = new SqlCommand("delete from Cham_Cong_Full_Time where id_Nhan_Vien = '" + txt_IDNV.Text + "'", c.conn);
                cmd3.ExecuteNonQuery();

                SqlCommand cmd4 = new SqlCommand("delete from Cham_Cong_Part_Time where id_Nhan_Vien = '" + txt_IDNV.Text + "'", c.conn);
                cmd4.ExecuteNonQuery();

                SqlCommand cmd1 = new SqlCommand("delete from Nguoi_Dung where id_Nhan_Vien = '" + txt_IDNV.Text + "'", c.conn);
                cmd1.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand("delete from Cong_Viec where id_Nhan_Vien = '" + txt_IDNV.Text + "'", c.conn);
                cmd2.ExecuteNonQuery();

                SqlCommand cmd5 = new SqlCommand("delete from Khen_Thuong where id_Nhan_Vien = '" + txt_IDNV.Text + "'", c.conn);
                cmd5.ExecuteNonQuery();

                SqlCommand cmd7 = new SqlCommand("delete from Ky_Luat where id_Nhan_Vien = '" + txt_IDNV.Text + "'", c.conn);
                cmd7.ExecuteNonQuery();

                SqlCommand cmd6 = new SqlCommand("delete from Nhan_Vien where id_Nhan_Vien = '" + txt_IDNV.Text + "'", c.conn);
                cmd6.ExecuteNonQuery();

                MessageBox.Show("Xóa Thành Công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                reset();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pic_HinhAnh.Image = new Bitmap(openFileDialog.FileName);
            }
            ckanh = true;
        }

        private void txt_HoTen_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Nếu ký tự vừa nhập là số hoặc ký tự đặc biệt
            if (Char.IsDigit(e.KeyChar) || Char.IsSymbol(e.KeyChar) || Char.IsPunctuation(e.KeyChar))
            {
                // Không cho phép nhập vào textbox
                e.Handled = true;
            }
        }

        private void txt_CCCD_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Nếu ký tự vừa nhập không phải là số hoặc là backspace
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            { // Không cho phép nhập vào textbox
                e.Handled = true;
            }
        }

        private void txt_SDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Nếu ký tự vừa nhập không phải là số hoặc là backspace
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            { // Không cho phép nhập vào textbox
                e.Handled = true;
            }
        }

        public bool ckgmail(string gmail)
        {
            if (gmail == "")
            {
                return true;
            }
            else
            {
                try
                {

                    var email = new MailAddress(gmail);
                    return true;
                }
                catch (System.FormatException)
                {
                    // Handle invalid email format.
                    return false;
                }
            }
        }

        public bool cknhanvien(string ckidnhanvien, string gmail, string cktaikhoan, string ckgmailuser)
        {
            c.connect();
            int ck1 = 0, ck2 = 0, ck3 = 0, ck4 = 0, ck5 = 0;
            SqlCommand sqlCommand = new SqlCommand("select count(id_Nhan_Vien) from Nhan_Vien where id_Nhan_Vien = '" + ckidnhanvien + "'", c.conn);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                ck1 = reader.GetInt32(0);
            }
            reader.Close();

            SqlCommand sqlCommand1 = new SqlCommand("select count(Gmail) from Nhan_Vien where Gmail = '" + gmail + "'", c.conn);
            SqlDataReader reader2 = sqlCommand1.ExecuteReader();
            while (reader2.Read())
            {
                ck2 = reader2.GetInt32(0);
            }
            reader2.Close();

            SqlCommand sqlCommand2 = new SqlCommand("select count(Tai_Khoan) from Nguoi_Dung where Tai_Khoan = '" + cktaikhoan + "'", c.conn);
            SqlDataReader reader3 = sqlCommand2.ExecuteReader();
            while (reader3.Read())
            {
                ck3 = reader3.GetInt32(0);
            }
            reader3.Close();

            SqlCommand sqlCommand3 = new SqlCommand("select count(Gmail) from Nguoi_Dung where Gmail = '" + ckgmailuser + "'", c.conn);
            SqlDataReader reader4 = sqlCommand3.ExecuteReader();
            while (reader4.Read())
            {
                ck4 = reader4.GetInt32(0);
            }
            reader4.Close();

            SqlCommand sqlCommand4 = new SqlCommand("select count(*) from Nhan_Vien where SDT = '" + txt_SDT.Text + "'", c.conn);
            SqlDataReader reader5 = sqlCommand4.ExecuteReader();
            while (reader5.Read())
            {
                ck5 = reader5.GetInt32(0);
            }
            reader5.Close();

            if (ck1 != 0)
            {
                MessageBox.Show("ID Này Đã Được Sử Dụng\nVui Lòng Nhập ID Khác", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_IDNV.Clear();
                return false;
            }
            else if (ck2 != 0)
            {
                MessageBox.Show("Gmail Này Đã Được Sử Dụng\nVui Lòng Nhập Gmail Khác", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_Gmail.Clear();
                return false;
            }
            else if (ck3 != 0)
            {
                MessageBox.Show("Tài Khoản Này Đã Được Sử Dụng\nVui Lòng Nhập Tài Khoản Khác", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_taikhoan.Clear();
                return false;
            }
            else if (ck4 != 0)
            {
                MessageBox.Show("Gmail Này Đã Được Sử Dụng\nVui Lòng Nhập Gmail Khác", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_gmailUser.Clear();
                return false;
            }
            else if (ck5 != 0)
            {
                MessageBox.Show("SDT Này Đã Được Sử Dụng\nVui Lòng Nhập SDT Khác", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_SDT.Clear();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void txt_Point_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Nếu ký tự vừa nhập không phải là số hoặc là backspace
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            { // Không cho phép nhập vào textbox
                e.Handled = true;
            }
        }

        private void txt_Point_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_Point_Click(object sender, EventArgs e)
        {
            txt_Point.Clear();
        }

        public void LoadImage(string idnv)
        {
            ConnectData c = new ConnectData();
            c.connect();
            string query = "select Hinh_Anh from Nhan_Vien where id_Nhan_Vien = '" + idnv + "'";
            SqlCommand cmd = new SqlCommand(query, c.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (reader["Hinh_Anh"] != DBNull.Value)
                {
                    byte[] imgData = (byte[])reader["Hinh_Anh"];
                    using (MemoryStream ms = new MemoryStream(imgData))
                    {
                        pic_HinhAnh.Image = Image.FromStream(ms);
                    }
                }

            }
            ckanh = false;

        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            button1.Show();

            // Lấy chỉ số của hàng được chọn
            int rowIndex = e.RowIndex;
            // Kiểm tra nếu chỉ số hợp lệ (không âm và không vượt quá số hàng của dgvData)
            if (rowIndex >= 0 && rowIndex < dataGridView1.Rows.Count)
            {
                // Lấy dữ liệu từ cột tương ứng của hàng được chọn và gán cho các TextBox
                idnv = dataGridView1.Rows[rowIndex].Cells["id_Nhan_Vien"].Value.ToString();
                txt_IDNV.Text =dataGridView1.Rows[rowIndex].Cells["id_Nhan_Vien"].Value.ToString();
                txt_HoTen.Text = dataGridView1.Rows[rowIndex].Cells["Ho_Ten"].Value.ToString();
                txt_DiaChi.Text = dataGridView1.Rows[rowIndex].Cells["Dia_Chi"].Value.ToString();
                txt_QueQuan.Text = dataGridView1.Rows[rowIndex].Cells["Que_Quan"].Value.ToString();
                txt_SDT.Text = dataGridView1.Rows[rowIndex].Cells["SDT"].Value.ToString();
                txt_taikhoan.Text = dataGridView1.Rows[rowIndex].Cells["Tai_Khoan"].Value.ToString();
                txt_matkhau.Text = dataGridView1.Rows[rowIndex].Cells["Mat_Khau"].Value.ToString();
                txt_Point.Text = dataGridView1.Rows[rowIndex].Cells["Point"].Value.ToString();
                txt_TonGiao.Text = dataGridView1.Rows[rowIndex].Cells["Ton_Giao"].Value.ToString();
                cmb_phanquyen.Text = dataGridView1.Rows[rowIndex].Cells["Phan_Quyen"].Value.ToString();
                cmb_Phong_Ban.Text = dataGridView1.Rows[rowIndex].Cells["id_Phong_Ban"].Value.ToString();
                cmb_Chuc_Vu.Text = dataGridView1.Rows[rowIndex].Cells["id_Chuc_Vu"].Value.ToString();
                cmb_Phu_Cap.Text = dataGridView1.Rows[rowIndex].Cells["id_Phu_Cap"].Value.ToString();
                cmb_GioiTinh.Text = dataGridView1.Rows[rowIndex].Cells["Gioi_Tinh"].Value.ToString();
                dtp_NgaySinh.Text = dataGridView1.Rows[rowIndex].Cells["Ngay_Sinh"].Value.ToString();
                txt_Gmail.Text = dataGridView1.Rows[rowIndex].Cells[8].Value.ToString();
                txt_gmailUser.Text = dataGridView1.Rows[rowIndex].Cells[22].Value.ToString();
                txt_CCCD.Text = dataGridView1.Rows[rowIndex].Cells["CCCD"].Value.ToString();
                txt_DanToc.Text = dataGridView1.Rows[rowIndex].Cells["Dan_Toc"].Value.ToString();
                LoadImage(txt_IDNV.Text);

            }
        }

        private void btn_timkiem_Click(object sender, EventArgs e)
        {
            if (cmb_TKPhongBan.SelectedIndex == 0 && cmb_TKChucVu.SelectedIndex == 0)
            {
                DataSet ds = new DataSet();
                string query = "select * from Nhan_Vien,Nguoi_Dung where Nhan_Vien.id_Nhan_Vien = Nguoi_Dung.id_Nhan_Vien and Nhan_Vien.id_Nhan_Vien like N'%" + txt_TKID.Text + "%'";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
                sqlDataAdapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.ReadOnly = true;
            }
            else if (cmb_TKPhongBan.SelectedIndex != 0 && cmb_TKChucVu.SelectedIndex == 0)
            {
                DataSet ds = new DataSet();
                string query = "select * from Nhan_Vien,Nguoi_Dung where Nhan_Vien.id_Nhan_Vien = Nguoi_Dung.id_Nhan_Vien and Nhan_Vien.id_Nhan_Vien like N'%" + txt_TKID.Text + "%' and id_Phong_Ban like N'%" + cmb_TKPhongBan.Text + "%'";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
                sqlDataAdapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.ReadOnly = true;
            }
            else if (cmb_TKPhongBan.SelectedIndex == 0 && cmb_TKChucVu.SelectedIndex != 0)
            {
                DataSet ds = new DataSet();
                string query = "select * from Nhan_Vien,Nguoi_Dung where Nhan_Vien.id_Nhan_Vien = Nguoi_Dung.id_Nhan_Vien and Nhan_Vien.id_Nhan_Vien like N'%" + txt_TKID.Text + "%' and id_Chuc_Vu like N'%" + cmb_TKChucVu.Text + "%'";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
                sqlDataAdapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.ReadOnly = true;
            }
            else if (cmb_TKPhongBan.SelectedIndex != 0 && cmb_TKChucVu.SelectedIndex != 0)
            {
                DataSet ds = new DataSet();
                string query = "select * from Nhan_Vien,Nguoi_Dung where Nhan_Vien.id_Nhan_Vien = Nguoi_Dung.id_Nhan_Vien and Nhan_Vien.id_Nhan_Vien like N'%" + txt_TKID.Text + "%' and id_Chuc_Vu like N'%" + cmb_TKChucVu.Text + "%' and id_Phong_Ban like N'%" + cmb_TKPhongBan.Text + "%'";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
                sqlDataAdapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.ReadOnly = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmthemkhuanmat frmthemkhuanmat = new frmthemkhuanmat();
            frmthemkhuanmat.ShowDialog();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
