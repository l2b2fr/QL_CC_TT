using AForge.Video.DirectShow;
using Aspose.Words;
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
using Aspose.Words.Tables;
using ThuVienWinform.Report.AsposeWordExtension;
using static System.Net.Mime.MediaTypeNames;
using ZXing;
using System.Globalization;

namespace QL_CC_TT_LEE_NAM
{
    public partial class frm_khenthuong_kyluat : Form
    {
        public frm_khenthuong_kyluat()
        {
            InitializeComponent();
        }
        ConnectData c = new ConnectData();
        public string ckkt,ckkl;
        public string id_khen_thuong;

        private void frm_khenthuong_kyluat_Load(object sender, EventArgs e)
        {
            c.connect();

            SqlCommand hienthi1 = new SqlCommand("select * from Chuc_Vu", c.conn);
            SqlDataReader reader1 = hienthi1.ExecuteReader();
            while (reader1.Read())
            {
                cmb_TKChucVu.Items.Add(reader1["id_Chuc_Vu"].ToString());

            }
            cmb_TKChucVu.SelectedIndex = 0;
            reader1.Close();

            SqlCommand hienthi2 = new SqlCommand("select * from Phong_Ban", c.conn);
            SqlDataReader reader2 = hienthi2.ExecuteReader();
            while (reader2.Read())
            {
                cmb_TKPhongBan.Items.Add(reader2["id_Phong_Ban"].ToString());

            }
            cmb_TKPhongBan.SelectedIndex = 0;
            reader2.Close();

            DataSet ds = new DataSet();
            string query = "select * from Nhan_Vien,Cong_Viec where Nhan_Vien.id_Nhan_Vien = Cong_Viec.id_Nhan_Vien and Nhan_Vien.id_Nhan_Vien like N'%" + txt_TKID.Text + "%'";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
            sqlDataAdapter.Fill(ds);
            dtg_TK.DataSource = ds.Tables[0];
            dtg_TK.ReadOnly = true;

        }

        public void loadktkl(string idnv)
        {
            DataSet ds1 = new DataSet();
            string query1 = "select id_Nhan_Vien 'Mã Nhân Viên', Ngay_Khen_Thuong 'Ngày Khen Thưởng', So_Tien 'Số Tiền', Ly_Do_KT 'Lý Do' from Khen_Thuong where id_Nhan_Vien = '" + idnv + "'";
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(query1, c.conn);
            sqlDataAdapter1.Fill(ds1);
            dtg_khenthuong.DataSource = ds1.Tables[0];
            dtg_khenthuong.ReadOnly = true;

            DataSet ds = new DataSet();
            string query = "select id_Nhan_Vien 'Mã Nhân Viên', Ngay_Ky_Luat 'Ngày Kỷ Luật', So_Tien 'Số Tiền', Ly_Do_KL 'Lý Do' from Ky_Luat where id_Nhan_Vien = '" + idnv + "'";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
            sqlDataAdapter.Fill(ds);
            dtg_kyluat.DataSource = ds.Tables[0];
            dtg_kyluat.ReadOnly = true;
        }

        private void dtg_TK_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            c.connect();
            int rowIndex = e.RowIndex;
            // Kiểm tra nếu chỉ số hợp lệ (không âm và không vượt quá số hàng của dgvData)
            if (rowIndex >= 0 && rowIndex < dtg_TK.Rows.Count)
            {
                // Lấy dữ liệu từ cột tương ứng của hàng được chọn và gán cho các TextBox
                txt_TKID.Text = dtg_TK.Rows[rowIndex].Cells["id_Nhan_Vien"].Value.ToString();

                loadktkl(dtg_TK.Rows[rowIndex].Cells["id_Nhan_Vien"].Value.ToString());
            }

            
        }

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

        private void btn_themkt_Click(object sender, EventArgs e)
        {
            txt_lydokt.Clear();
            txt_sotienkt.Clear();
            dtp_ngaykt.Value = DateTime.Now;

            ckkt = "them";

            btn_huykt.Show();
            btn_themkt.Hide();
            btn_suakt.Hide();
            btn_xoakt.Hide();
            btn_inkt.Hide();
        }

        private void btn_suakt_Click(object sender, EventArgs e)
        {
            ckkt = "sua";

            btn_huykt.Show();
            btn_themkt.Hide();
            btn_suakt.Hide();
            btn_xoakt.Hide();
            btn_inkt.Hide();
        }

        private void btn_xoakt_Click(object sender, EventArgs e)
        {
            c.connect();
            DialogResult dg = MessageBox.Show("Bạn có muốn xóa khen thưởng !", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.Yes)
            {

                SqlCommand cmd = new SqlCommand("DELETE FROM Khen_Thuong WHERE id_Nhan_Vien = '" + txt_TKID.Text + "' and Ly_Do_KT = N'" + txt_lydokt.Text + "' and So_Tien = '" + txt_sotienkt.Text + "'and Ngay_Khen_Thuong = '" + dtp_ngaykt.Value + "'", c.conn);
                cmd.ExecuteNonQuery();

                loadktkl(txt_TKID.Text);
                btn_huykt.Hide();
                btn_themkt.Show();
                btn_suakt.Show();
                btn_xoakt.Show();
                btn_inkt.Show();
            }
        }

        private void btn_inkt_Click(object sender, EventArgs e)
        {
            if(txt_lydokt.Text == "" || txt_TKID.Text == "")
            {
                MessageBox.Show("Vui lòng chọn nhân viên vói cột khen thưởng","Thông Báo",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            }
            else
            {
                var homNay = DateTime.Now;
                //Bước 1: Nạp file mẫu
                Document baoCao = new Document("DOC_BAO_CAO_KHEN_THUONG.doc");
                string id_truong_phong = "";

                //Bước 2: Điền các thông tin cố định
                SqlCommand cmd = new SqlCommand("select * from Nhan_Vien,Phong_Ban,Chuc_Vu where id_Nhan_Vien = '" + txt_TKID.Text + "' and Nhan_Vien.id_Phong_Ban = Phong_Ban.id_Phong_Ban and Chuc_Vu.id_Chuc_Vu = Nhan_Vien.id_Chuc_Vu", c.conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    DateTime ngaysinh = (DateTime)reader["Ngay_Sinh"];
                    baoCao.MailMerge.Execute(new[] { "Ngay_Thang_Nam_BC" }, new[] { string.Format("Hà Nội, ngày {0} tháng {1} năm {2}", homNay.Day, homNay.Month, homNay.Year) });
                    baoCao.MailMerge.Execute(new[] { "id_Nhan_Vien" }, new[] { reader["id_Nhan_Vien"].ToString() });
                    baoCao.MailMerge.Execute(new[] { "Ho_Ten" }, new[] { reader["Ho_Ten"].ToString() });
                    baoCao.MailMerge.Execute(new[] { "Ngay_Sinh" }, new[] { ngaysinh.ToString("dd/MM/yyyy") });
                    baoCao.MailMerge.Execute(new[] { "SDT" }, new[] { reader["SDT"].ToString() });
                    baoCao.MailMerge.Execute(new[] { "Que_Quan" }, new[] { reader["Que_Quan"].ToString() });
                    baoCao.MailMerge.Execute(new[] { "Gmail" }, new[] { reader["Gmail"].ToString() });
                    baoCao.MailMerge.Execute(new[] { "Phong_Ban" }, new[] { reader["Ten_Phong_Ban"].ToString() });
                    baoCao.MailMerge.Execute(new[] { "Chuc_Vu" }, new[] { reader["Ten_Chuc_Vu"].ToString() });
                    id_truong_phong = reader["id_Phong_Ban"].ToString();
                }
                reader.Close();


                SqlCommand sqlCommand = new SqlCommand("select * from Khen_Thuong where id_Nhan_Vien = '" + txt_TKID.Text + "' and Ly_Do_KT = N'" + txt_lydokt.Text + "' and So_Tien = '" + txt_sotienkt.Text + "'", c.conn);
                SqlDataReader reader1 = sqlCommand.ExecuteReader();
                if (reader1.Read())
                {
                    //double monney = (double)reader1["So_Tien"];
                    double monney = Convert.ToDouble(reader1["So_Tien"].ToString());
                    string formattedMoney = monney.ToString("N0");

                    baoCao.MailMerge.Execute(new[] { "id_khen_thuong" }, new[] { reader1["id_Khen_Thuong"].ToString() });
                    baoCao.MailMerge.Execute(new[] { "So_Tien" }, new[] { formattedMoney });
                    baoCao.MailMerge.Execute(new[] { "Ly_Do_Khen_Thuong" }, new[] { reader1["Ly_Do_KT"].ToString() });
                    
                }
                reader1.Close();

                string C3 = "CV3";
                SqlCommand sqlCommand1 = new SqlCommand("select * from Nhan_Vien, Phong_Ban where Nhan_Vien.id_Phong_Ban = '" + id_truong_phong + "' and id_Chuc_Vu = '" + C3 +"'",c.conn);
                SqlDataReader sqlDataReader = sqlCommand1.ExecuteReader();
                if (sqlDataReader.Read())
                {
                    baoCao.MailMerge.Execute(new[] { "Ten_Truong_Phong" }, new[] { sqlDataReader["Ho_Ten"].ToString() });
                }
                sqlDataReader.Close();


                /*
                    //Bước 3: Điền thông tin lên bảng
                    Table bangThongTinGiaDinh = baoCao.GetChild(NodeType.Table, 1, true) as Table;//Lấy bảng thứ 2 trong file mẫu
                    int hangHienTai = 1;
                    bangThongTinGiaDinh.InsertRows(hangHienTai, hangHienTai, day);
                    reader.Close();
                    for (int i = 1; i <= day; i++)
                    {

                        SqlCommand sqlCommand1 = new SqlCommand("select * from Cham_Cong_Full_Time where id_Nhan_Vien = '" + txt_TKID.Text + "' and day(Ngay_Lam) = '" + i + "' and month(Ngay_Lam) = '" + month + "' and year(Ngay_Lam) = '" + year + "'", c.conn);
                        SqlDataReader sqlDataReader = sqlCommand1.ExecuteReader();
                        if (sqlDataReader.Read())
                        {
                            DateTime ngayLam = (DateTime)sqlDataReader["Ngay_Lam"];
                            string ngayLamChuoi = ngayLam.ToString("dd/MM/yyyy");
                            bangThongTinGiaDinh.PutValue(hangHienTai, 0, i.ToString());//Cột STT
                            bangThongTinGiaDinh.PutValue(hangHienTai, 1, ngayLamChuoi);//Cột Ngày Công
                            bangThongTinGiaDinh.PutValue(hangHienTai, 2, sqlDataReader["Diem_Danh"].ToString());//Cột Điểm Danh
                            bangThongTinGiaDinh.PutValue(hangHienTai, 3, sqlDataReader["Gio_Vao"].ToString());//Cột Giờ Vào
                            bangThongTinGiaDinh.PutValue(hangHienTai, 4, sqlDataReader["Di_Muon"].ToString());//Cột Đi Muộn
                            bangThongTinGiaDinh.PutValue(hangHienTai, 5, sqlDataReader["Nghi_Phep"].ToString());//Cột Nghi Phep
                            bangThongTinGiaDinh.PutValue(hangHienTai, 6, sqlDataReader["Tang_Ca"].ToString());//Cột Tăng Ca
                            bangThongTinGiaDinh.PutValue(hangHienTai, 7, sqlDataReader["Gio_Bat_Dau"].ToString());//Cột Bắt Đầu
                            bangThongTinGiaDinh.PutValue(hangHienTai, 8, sqlDataReader["Gio_Ket_Thuc"].ToString());//Cột Kêt Thúc
                            hangHienTai++;
                        }
                        sqlDataReader.Close();

                    }
                */

                //Bước 4: Lưu và mở file
                baoCao.SaveAndOpenFile("BaoCao.doc");
            }
        }

        private void btn_huy_Click(object sender, EventArgs e)
        {
            btn_huykt.Hide();
            btn_themkt.Show();
            btn_suakt.Show();
            btn_xoakt.Show();
            btn_inkt.Show();
        }

        private void btn_huykt_Click(object sender, EventArgs e)
        {
            btn_huykt.Hide();
            btn_themkt.Show();
            btn_suakt.Show();
            btn_xoakt.Show();
            btn_inkt.Show();
        }

        private void btn_themkl_Click(object sender, EventArgs e)
        {
            txt_lydokl.Clear();
            txt_sotienkl.Clear();
            dtp_ngaykl.Value = DateTime.Now;

            ckkl = "them";

            btn_luukl.Show();
            btn_huykt.Show();
            btn_themkl.Hide();
            btn_suakl.Hide();
            btn_xoakl.Hide();
            btn_inkl.Hide();
        }

        private void btn_suakl_Click(object sender, EventArgs e)
        {
            ckkl = "sua";

            btn_luukl.Show();
            btn_huykt.Show();
            btn_themkl.Hide();
            btn_suakl.Hide();
            btn_xoakl.Hide();
            btn_inkl.Hide();
        }

        private void btn_xoakl_Click(object sender, EventArgs e)
        {
            c.connect();
            DialogResult dg = MessageBox.Show("Bạn có muốn xóa kỷ luật !", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.Yes)
            {

                SqlCommand cmd = new SqlCommand("DELETE FROM Ky_Luat WHERE id_Nhan_Vien = '" + txt_TKID.Text + "' and Ly_Do_KL = N'" + txt_lydokl.Text + "' and So_Tien = '" + txt_sotienkl.Text + "'and Ngay_Ky_Luat = '" + dtp_ngaykl.Value + "'", c.conn);
                cmd.ExecuteNonQuery();

                loadktkl(txt_TKID.Text);
                btn_huykt.Hide();
                btn_themkt.Show();
                btn_suakt.Show();
                btn_xoakt.Show();
                btn_inkt.Show();
            }
        }

        private void btn_inkl_Click(object sender, EventArgs e)
        {
            if (txt_lydokl.Text == "" || txt_TKID.Text == "")
            {
                MessageBox.Show("Vui lòng chọn nhân viên với cột kỷ luật", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else
            {
                var homNay = DateTime.Now;
                //Bước 1: Nạp file mẫu
                Document baoCao = new Document("DOC_BAO_CAO_KY_LUAT.doc");
                string id_truong_phong = "";

                //Bước 2: Điền các thông tin cố định
                SqlCommand cmd = new SqlCommand("select * from Nhan_Vien,Phong_Ban,Chuc_Vu,Cong_Viec where Nhan_Vien.id_Nhan_Vien = '" + txt_TKID.Text + "' and Nhan_Vien.id_Phong_Ban = Phong_Ban.id_Phong_Ban and Chuc_Vu.id_Chuc_Vu = Nhan_Vien.id_Chuc_Vu and Nhan_Vien.id_Nhan_Vien = Cong_Viec.id_Nhan_Vien", c.conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    DateTime ngaysinh = (DateTime)reader["Ngay_Sinh"];
                    baoCao.MailMerge.Execute(new[] { "Ngay_Thang_Nam_BC" }, new[] { string.Format("Hà Nội, ngày {0} tháng {1} năm {2}", homNay.Day, homNay.Month, homNay.Year) });
                    baoCao.MailMerge.Execute(new[] { "id_Nhan_Vien" }, new[] { reader["id_Nhan_Vien"].ToString() });
                    baoCao.MailMerge.Execute(new[] { "Ho_Ten" }, new[] { reader["Ho_Ten"].ToString() });
                    baoCao.MailMerge.Execute(new[] { "Ngay_Sinh" }, new[] { ngaysinh.ToString("dd/MM/yyyy") });
                    baoCao.MailMerge.Execute(new[] { "SDT" }, new[] { reader["SDT"].ToString() });
                    baoCao.MailMerge.Execute(new[] { "Que_Quan" }, new[] { reader["Que_Quan"].ToString() });
                    baoCao.MailMerge.Execute(new[] { "Gmail" }, new[] { reader["Gmail"].ToString() });
                    baoCao.MailMerge.Execute(new[] { "Phong_Ban" }, new[] { reader["Ten_Phong_Ban"].ToString() });
                    baoCao.MailMerge.Execute(new[] { "Chuc_Vu" }, new[] { reader["Ten_Chuc_Vu"].ToString() });
                    baoCao.MailMerge.Execute(new[] { "Ten_Cong_Viec" }, new[] { reader["Ten_Cong_Viec"].ToString() });
                    id_truong_phong = reader["id_Phong_Ban"].ToString();
                }
                reader.Close();


                SqlCommand sqlCommand = new SqlCommand("select * from KY_Luat where id_Nhan_Vien = '" + txt_TKID.Text + "' and Ly_Do_KL = N'" + txt_lydokl.Text + "' and So_Tien = '" + txt_sotienkl.Text + "'", c.conn);
                SqlDataReader reader1 = sqlCommand.ExecuteReader();
                if (reader1.Read())
                {
                    //double monney = (double)reader1["So_Tien"];
                    double monney = Convert.ToDouble(reader1["So_Tien"].ToString());
                    string formattedMoney = monney.ToString("N0");

                    baoCao.MailMerge.Execute(new[] { "So_Tien" }, new[] { formattedMoney });
                    baoCao.MailMerge.Execute(new[] { "Ly_Do_KY_Luat" }, new[] { reader1["Ly_Do_KL"].ToString() });

                }
                reader1.Close();

                string C3 = "CV3";
                SqlCommand sqlCommand1 = new SqlCommand("select * from Nhan_Vien, Phong_Ban where Nhan_Vien.id_Phong_Ban = '" + id_truong_phong + "' and id_Chuc_Vu = '" + C3 + "'", c.conn);
                SqlDataReader sqlDataReader = sqlCommand1.ExecuteReader();
                if (sqlDataReader.Read())
                {
                    baoCao.MailMerge.Execute(new[] { "Ten_Truong_Phong" }, new[] { sqlDataReader["Ho_Ten"].ToString() });
                }
                sqlDataReader.Close();


                /*
                    //Bước 3: Điền thông tin lên bảng
                    Table bangThongTinGiaDinh = baoCao.GetChild(NodeType.Table, 1, true) as Table;//Lấy bảng thứ 2 trong file mẫu
                    int hangHienTai = 1;
                    bangThongTinGiaDinh.InsertRows(hangHienTai, hangHienTai, day);
                    reader.Close();
                    for (int i = 1; i <= day; i++)
                    {

                        SqlCommand sqlCommand1 = new SqlCommand("select * from Cham_Cong_Full_Time where id_Nhan_Vien = '" + txt_TKID.Text + "' and day(Ngay_Lam) = '" + i + "' and month(Ngay_Lam) = '" + month + "' and year(Ngay_Lam) = '" + year + "'", c.conn);
                        SqlDataReader sqlDataReader = sqlCommand1.ExecuteReader();
                        if (sqlDataReader.Read())
                        {
                            DateTime ngayLam = (DateTime)sqlDataReader["Ngay_Lam"];
                            string ngayLamChuoi = ngayLam.ToString("dd/MM/yyyy");
                            bangThongTinGiaDinh.PutValue(hangHienTai, 0, i.ToString());//Cột STT
                            bangThongTinGiaDinh.PutValue(hangHienTai, 1, ngayLamChuoi);//Cột Ngày Công
                            bangThongTinGiaDinh.PutValue(hangHienTai, 2, sqlDataReader["Diem_Danh"].ToString());//Cột Điểm Danh
                            bangThongTinGiaDinh.PutValue(hangHienTai, 3, sqlDataReader["Gio_Vao"].ToString());//Cột Giờ Vào
                            bangThongTinGiaDinh.PutValue(hangHienTai, 4, sqlDataReader["Di_Muon"].ToString());//Cột Đi Muộn
                            bangThongTinGiaDinh.PutValue(hangHienTai, 5, sqlDataReader["Nghi_Phep"].ToString());//Cột Nghi Phep
                            bangThongTinGiaDinh.PutValue(hangHienTai, 6, sqlDataReader["Tang_Ca"].ToString());//Cột Tăng Ca
                            bangThongTinGiaDinh.PutValue(hangHienTai, 7, sqlDataReader["Gio_Bat_Dau"].ToString());//Cột Bắt Đầu
                            bangThongTinGiaDinh.PutValue(hangHienTai, 8, sqlDataReader["Gio_Ket_Thuc"].ToString());//Cột Kêt Thúc
                            hangHienTai++;
                        }
                        sqlDataReader.Close();

                    }
                */

                //Bước 4: Lưu và mở file
                baoCao.SaveAndOpenFile("BaoCao.doc");
            }
        }

        private void btn_huykl_Click(object sender, EventArgs e)
        {
            btn_luukl.Hide();
            btn_huykt.Hide();
            btn_themkl.Show();
            btn_suakl.Show();
            btn_xoakl.Show();
            btn_inkl.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void btn_luukt_Click(object sender, EventArgs e)
        {
            c.connect();
            DialogResult dg = MessageBox.Show("Bạn có muốn lưu khen thưởng !","Thông Báo",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(dg == DialogResult.Yes)
            {

                if(ckkt == "them")
                {
                    SqlCommand cmd = new SqlCommand("insert into Khen_Thuong values ('" + txt_TKID.Text + "', N'" + txt_lydokt.Text + "', '" + txt_sotienkt.Text + "', '" + dtp_ngaykt.Value + "')",c.conn);
                    cmd.ExecuteNonQuery();
                }else if(ckkt == "sua")
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Khen_Thuong SET Ly_Do_KT = N'" + txt_lydokt.Text + "', So_Tien = '" + txt_sotienkt.Text + "', Ngay_Khen_Thuong = '" + dtp_ngaykt.Value + "' WHERE id_Nhan_Vien = '" + txt_TKID.Text + "'", c.conn);
                    cmd.ExecuteNonQuery();
                }

                loadktkl(txt_TKID.Text);
                btn_huykt.Hide();
                btn_themkt.Show();
                btn_suakt.Show();
                btn_xoakt.Show();
                btn_inkt.Show();
            }
        }

        private void btn_luukl_Click(object sender, EventArgs e)
        {


            c.connect();
            DialogResult dg = MessageBox.Show("Bạn có muốn lưu kỷ luật !", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.Yes)
            {

                if (ckkl == "them")
                {
                    SqlCommand cmd = new SqlCommand("insert into Ky_Luat values ('" + txt_TKID.Text + "', N'" + txt_lydokl.Text + "', '" + txt_sotienkl.Text + "', '" + dtp_ngaykl.Value + "')", c.conn);
                    cmd.ExecuteNonQuery();
                }
                else if (ckkl == "sua")
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Ky_Luat SET Ly_Do_KL = N'" + txt_lydokl.Text + "', So_Tien = '" + txt_sotienkl.Text + "', Ngay_Ky_Luat = '" + dtp_ngaykl.Value + "' WHERE id_Nhan_Vien = '" + txt_TKID.Text + "'", c.conn);
                    cmd.ExecuteNonQuery();
                }

                loadktkl(txt_TKID.Text);
                btn_luukl.Hide();
                btn_huykt.Hide();
                btn_themkl.Show();
                btn_suakl.Show();
                btn_xoakl.Show();
                btn_inkl.Show();
            }
            
        }

        

        private void dtg_khenthuong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            c.connect();
            int rowIndex = e.RowIndex;
            // Kiểm tra nếu chỉ số hợp lệ (không âm và không vượt quá số hàng của dgvData)
            if (rowIndex >= 0 && rowIndex < dtg_khenthuong.Rows.Count)
            {
                // Lấy dữ liệu từ cột tương ứng của hàng được chọn và gán cho các TextBox
                txt_lydokt.Text = dtg_khenthuong.Rows[rowIndex].Cells["Lý Do"].Value.ToString();
                dtp_ngaykt.Text = dtg_khenthuong.Rows[rowIndex].Cells["Ngày Khen Thưởng"].Value.ToString();
                txt_sotienkt.Text = dtg_khenthuong.Rows[rowIndex].Cells["Số Tiền"].Value.ToString();

                
            }
        }

        private void dtg_kyluat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            c.connect();
            int rowIndex = e.RowIndex;
            // Kiểm tra nếu chỉ số hợp lệ (không âm và không vượt quá số hàng của dgvData)
            if (rowIndex >= 0 && rowIndex < dtg_kyluat.Rows.Count)
            {
                // Lấy dữ liệu từ cột tương ứng của hàng được chọn và gán cho các TextBox
                txt_lydokl.Text = dtg_kyluat.Rows[rowIndex].Cells["Lý Do"].Value.ToString();
                dtg_kyluat.Text = dtg_kyluat.Rows[rowIndex].Cells["Ngày Kỷ Luật"].Value.ToString();
                txt_sotienkl.Text = dtg_kyluat.Rows[rowIndex].Cells["Số Tiền"].Value.ToString();
            }
        }

        private void dtg_TK_Click(object sender, EventArgs e)
        {

        }
    }
}
