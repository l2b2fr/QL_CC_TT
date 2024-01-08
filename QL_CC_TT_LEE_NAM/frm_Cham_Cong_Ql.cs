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


namespace QL_CC_TT_LEE_NAM
{
    public partial class frm_Cham_Cong_Ql : Form
    {
        public frm_Cham_Cong_Ql()
        {
            InitializeComponent();
        }

        ConnectData c = new ConnectData();
        public static bool cknv, ckcv;
        public string loaicongviec = "no";
        public string chucvu, phongban;

        //FullTime
        public float ngaydiemdanhfull, ngaydimuonfull, ngaynghiphepfull, ngaynghikhongphepfull, giotangcafull;

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
                        pic_HinhAnh.Image = System.Drawing.Image.FromStream(ms);
                    }
                }
                else pic_HinhAnh.Image = null;
            }

        }

        public void loadpart(string idnv)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            c.connect();
            int gio_lam_day = 0;
            int gio_lam_month = 0;
            for (int i = 1; i < daysInMonth; i++)
            {
                //Tính số giờ Đã làm trên tháng
                SqlCommand cmd = new SqlCommand("select DATEDIFF(hour, Gio_Bat_Dau, Gio_Ket_Thuc) AS DateDiff FROM Cham_Cong_Part_Time, Cong_Viec " +
                    "where Cham_Cong_Part_Time.id_Nhan_Vien = Cong_Viec.id_Nhan_Vien " +
                    "and Gio_Bat_Dau IS NOT NULL " +
                    "and Gio_Ket_Thuc IS NOT NULL " +
                    "and DAY(Ngay_Lam) = '" + i + "' " +
                    "and MONTH(Ngay_Lam) = '" + month + "' " +
                    "and YEAR(Ngay_Lam) = '" + year + "' " +
                    "and Cong_Viec.id_Nhan_Vien = '" + idnv + "'", c.conn);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    gio_lam_day = sqlDataReader.GetInt32(0);
                    gio_lam_month = gio_lam_month + gio_lam_day;
                    lbl_sogiolam.Text = gio_lam_month.ToString();
                }
                else
                {

                }
                sqlDataReader.Close();
            }

            SqlCommand sqlCommand = new SqlCommand("select count(*) from Cham_Cong_Part_Time where id_Nhan_Vien = '" + idnv + "' and Ngay_Lam <= '" + DateTime.Now + "'", c.conn);
            SqlDataReader sqlDataReader1 = sqlCommand.ExecuteReader();
            int ngay = 0;
            while (sqlDataReader1.Read())
            {
                ngay = sqlDataReader1.GetInt32(0);
                lbl_ngaylam.Text = ngay.ToString();
            }
            sqlDataReader1.Close();
        }

        public void loadfull(string idnv)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int ddmuon = 0;
            int ddtangca = 0;
            int np = 0;
            int nkp = 0;


            SqlCommand cmd2 = new SqlCommand("select COUNT(Ngay_Lam) from Cham_Cong_Full_Time where Cham_Cong_Full_Time.id_Nhan_Vien = '" + idnv + "' and Ngay_Lam <= '" + DateTime.Now + "' and Di_Muon = '" + true + "'", c.conn);
            SqlDataReader dr2 = cmd2.ExecuteReader();

            while (dr2.Read())
            {

                ddmuon = dr2.GetInt32(0);
                lbl_ddmuon.Text = ddmuon.ToString();
                ngaydimuonfull = ddmuon;
            }
            dr2.Close();

            SqlCommand cmd3 = new SqlCommand("select COUNT(Ngay_Lam) from Cham_Cong_Full_Time where Cham_Cong_Full_Time.id_Nhan_Vien = '" + idnv + "' and Ngay_Lam <= '" + DateTime.Now + "' and Tang_Ca = '" + true + "'", c.conn);
            SqlDataReader dr3 = cmd3.ExecuteReader();

            while (dr3.Read())
            {

                ddtangca = dr3.GetInt32(0);
                lbl_ddtangca.Text = ddtangca.ToString();
            }
            dr3.Close();

            SqlCommand cmd4 = new SqlCommand("select COUNT(Ngay_Lam) from Cham_Cong_Full_Time where Cham_Cong_Full_Time.id_Nhan_Vien = '" + idnv + "' and Ngay_Lam <= '" + DateTime.Now + "' and Nghi_Phep = '" + true + "'", c.conn);
            SqlDataReader dr4 = cmd4.ExecuteReader();

            while (dr4.Read())
            {

                np = dr4.GetInt32(0);
                lbl_np.Text = np.ToString();
                ngaynghiphepfull = dr4.GetInt32(0);
            }
            dr4.Close();


            SqlCommand cmd5 = new SqlCommand("select COUNT(Ngay_Lam) from Cham_Cong_Full_Time where Cham_Cong_Full_Time.id_Nhan_Vien = '" + idnv + "' and Ngay_Lam <= '" + DateTime.Now + "' and Diem_Danh is null", c.conn);
            SqlDataReader dr5 = cmd5.ExecuteReader();

            while (dr5.Read())
            {
                nkp = dr5.GetInt32(0);
                lbl_nkp.Text = nkp.ToString();
                ngaynghikhongphepfull = dr5.GetInt32(0);
            }
            dr5.Close();
        }

        public void cheknv(string idnv)
        {
            SqlCommand sqlCommand = new SqlCommand("select count(id_Nhan_Vien) from Nhan_Vien where id_Nhan_Vien = '" + idnv + "'", c.conn);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            int ck = 0;
            while (reader.Read())
            {
                ck = reader.GetInt32(0);
                if (ck == 0)
                {
                    MessageBox.Show("Không có nhân viên này!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cknv = false;
                }
                else
                {
                    cknv = true;
                }
            }
            reader.Close();
        }

        public void ck_cong_viec(string idnv)
        {
            string full = "Full Time";
            string part = "Part Time";
            c.connect();
            SqlCommand sql = new SqlCommand("select count(Loai_Cong_Viec) from Cong_Viec where id_Nhan_Vien = '" + idnv + "' and Loai_Cong_Viec = '" + full + "' or Loai_Cong_Viec = '" + part + "'", c.conn);
            SqlDataReader reader = sql.ExecuteReader();
            int ck = 0;
            while (reader.Read())
            {
                ck = reader.GetInt32(0);
                if (ck == 0)
                {
                    MessageBox.Show("Nhân viên này chưa được thiết lập công việc.\nVui lòng thiết lập công việc", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cknv = false;
                }
                else
                {
                    ckcv = true;
                }
            }
            reader.Close();

            SqlCommand sql1 = new SqlCommand("select * from Cong_Viec where id_Nhan_Vien = '" + idnv + "'", c.conn);
            SqlDataReader reader1 = sql1.ExecuteReader();

            while (reader1.Read())
            {
                loaicongviec = reader1["Loai_Cong_Viec"].ToString();

            }
            reader1.Close();
        }

        private void btn_xem_Click(object sender, EventArgs e)
        {


            c.connect();
            cheknv(txt_IDNV.Text);
            if (cknv == true)
            {
                ck_cong_viec(txt_IDNV.Text);
                if (ckcv == true)
                {
                    SqlCommand sql = new SqlCommand("select * from Nhan_Vien, Cong_Viec, Phong_Ban where Nhan_Vien.id_Nhan_Vien = '" + txt_IDNV.Text + "' and Nhan_Vien.id_Nhan_Vien = Cong_Viec.id_Nhan_Vien and Nhan_Vien.id_Phong_Ban = Phong_Ban.id_Phong_Ban", c.conn);
                    SqlDataReader reader = sql.ExecuteReader();
                    while (reader.Read())
                    {
                        txt_Ho_Ten.Text = reader["Ho_Ten"].ToString();
                        txt_Cong_Viec.Text = reader["Ten_Cong_Viec"].ToString();
                        txt_DiaChi.Text = reader["Dia_Chi"].ToString();
                        txt_Phong_Ban.Text = reader["Ten_Phong_Ban"].ToString();
                        LoadImage(txt_IDNV.Text);
                    }
                    reader.Close();
                }
            }

            if (loaicongviec == "Part Time")
            {


                pal_tk_part.Show();
                pal_tk_full.Hide();
                lbl_thongke.Text = "Thống Kê Part Time";

                loadpart(txt_IDNV.Text);

                DataSet ds = new DataSet();
                string query = "select Ten_Cong_Viec 'Tên Công Việc', Ten_Ca 'Tên Ca', Ngay_Lam 'Ngày Làm', Cham_Cong_Part_Time.Gio_Bat_Dau 'Giờ Bắt Đầu Làm', Cham_Cong_Part_Time.Gio_Ket_Thuc 'Giờ Kết Thúc' from  Cong_Viec, Cham_Cong_Part_Time, Ca_Lam where Cham_Cong_Part_Time.id_Nhan_Vien = Cong_Viec.id_Nhan_Vien and Cham_Cong_Part_Time.id_Ca_Lam = Ca_Lam.id_Ca_Lam and Cong_Viec.id_Nhan_Vien = '" + txt_IDNV.Text + "' and Ngay_Lam <= '" + DateTime.Now + "'";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
                sqlDataAdapter.Fill(ds);
                dtg_bangcong.DataSource = ds.Tables[0];
            }
            else
            {
                pal_tk_full.Show();
                pal_tk_part.Hide();
                lbl_thongke.Text = "Thống Kê Full Time";

                loadfull(txt_IDNV.Text);

                DataSet ds = new DataSet();
                string query = "select id_Nhan_Vien 'Mã Công Việc', Ngay_Lam 'Ngày Điểm Danh', Diem_Danh 'Điểm Danh' ,Gio_Vao 'Giờ Vào', Di_Muon 'Đi Làm Muộn', Nghi_phep 'Nghỉ Phép', Tang_Ca 'Tăng Ca' from Cham_Cong_Full_Time where id_Nhan_Vien = '" + txt_IDNV.Text + "' and Ngay_Lam <= '" + DateTime.Now + "'";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
                sqlDataAdapter.Fill(ds);
                dtg_bangcong.DataSource = ds.Tables[0];
                dtg_bangcong.ReadOnly = true;
            }
        }

        private void btn_xem_Click_1(object sender, EventArgs e)
        {
            c.connect();
            cheknv(txt_IDNV.Text);
            if (cknv == true)
            {
                ck_cong_viec(txt_IDNV.Text);
                if (ckcv == true)
                {
                    SqlCommand sql = new SqlCommand("select * from Nhan_Vien, Cong_Viec, Phong_Ban where Nhan_Vien.id_Nhan_Vien = '" + txt_IDNV.Text + "' and Nhan_Vien.id_Nhan_Vien = Cong_Viec.id_Nhan_Vien and Nhan_Vien.id_Phong_Ban = Phong_Ban.id_Phong_Ban", c.conn);
                    SqlDataReader reader = sql.ExecuteReader();
                    while (reader.Read())
                    {
                        txt_Ho_Ten.Text = reader["Ho_Ten"].ToString();
                        txt_Cong_Viec.Text = reader["Ten_Cong_Viec"].ToString();
                        txt_DiaChi.Text = reader["Dia_Chi"].ToString();
                        txt_Phong_Ban.Text = reader["Ten_Phong_Ban"].ToString();
                        LoadImage(txt_IDNV.Text);
                    }
                    reader.Close();
                }
            }

            if (loaicongviec == "Part Time")
            {


                pal_tk_part.Show();
                pal_tk_full.Hide();
                lbl_thongke.Text = "Thống Kê Part Time";

                loadpart(txt_IDNV.Text);

                DataSet ds = new DataSet();
                string query = "select Ten_Cong_Viec 'Tên Công Việc', Ten_Ca 'Tên Ca', Ngay_Lam 'Ngày Làm', Cham_Cong_Part_Time.Gio_Bat_Dau 'Giờ Bắt Đầu Làm', Cham_Cong_Part_Time.Gio_Ket_Thuc 'Giờ Kết Thúc' from  Cong_Viec, Cham_Cong_Part_Time, Ca_Lam where Cham_Cong_Part_Time.id_Nhan_Vien = Cong_Viec.id_Nhan_Vien and Cham_Cong_Part_Time.id_Ca_Lam = Ca_Lam.id_Ca_Lam and Cong_Viec.id_Nhan_Vien = '" + txt_IDNV.Text + "' and Ngay_Lam <= '" + DateTime.Now + "'";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
                sqlDataAdapter.Fill(ds);
                dtg_bangcong.DataSource = ds.Tables[0];
                label6.Show();
            }
            else
            {
                pal_tk_full.Show();
                pal_tk_part.Hide();
                lbl_thongke.Text = "Thống Kê Full Time";

                loadfull(txt_IDNV.Text);

                DataSet ds = new DataSet();
                string query = "select id_Nhan_Vien 'Mã Công Việc', Ngay_Lam 'Ngày Điểm Danh', Diem_Danh 'Điểm Danh' ,Gio_Vao 'Giờ Vào', Di_Muon 'Đi Làm Muộn', Nghi_phep 'Nghỉ Phép', Tang_Ca 'Tăng Ca' from Cham_Cong_Full_Time where id_Nhan_Vien = '" + txt_IDNV.Text + "' and Ngay_Lam <= '" + DateTime.Now + "'";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
                sqlDataAdapter.Fill(ds);
                dtg_bangcong.DataSource = ds.Tables[0];
                dtg_bangcong.ReadOnly = true;
                label6.Show();
            }
        }

        private void txt_IDNV_Click(object sender, EventArgs e)
        {
            txt_IDNV.Text = "";
            txt_IDNV.ForeColor = System.Drawing.Color.Black;
        }

        private void txt_IDNV_TextChanged(object sender, EventArgs e)
        {

        }

        public void Print_Part(string idnv)
        {
            c.connect();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int gio_lam_day = 0;
            int gio_lam_month = 0;
            int day = DateTime.Now.Day;
            for (int i = 1; i < daysInMonth; i++)
            {
                //Tính số giờ Đã làm trên tháng
                SqlCommand cmd = new SqlCommand("select DATEDIFF(hour, Gio_Bat_Dau, Gio_Ket_Thuc) AS DateDiff FROM Cham_Cong_Part_Time, Cong_Viec " +
                    "where Cham_Cong_Part_Time.id_Nhan_Vien = Cong_Viec.id_Nhan_Vien " +
                    "and Gio_Bat_Dau IS NOT NULL " +
                    "and Gio_Ket_Thuc IS NOT NULL " +
                    "and DAY(Ngay_Lam) = '" + i + "' " +
                    "and MONTH(Ngay_Lam) = '" + month + "' " +
                    "and YEAR(Ngay_Lam) = '" + year + "' " +
                    "and Cong_Viec.id_Nhan_Vien = '" + idnv + "';", c.conn);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    gio_lam_day = sqlDataReader.GetInt32(0);
                    gio_lam_month = gio_lam_month + gio_lam_day;
                    lbl_sogiolam.Text = gio_lam_month.ToString();
                }
                else
                {

                }
                sqlDataReader.Close();
            }

            SqlCommand sqlCommand = new SqlCommand("select * from Nhan_Vien, Phong_Ban, Chuc_Vu where Nhan_Vien.id_Nhan_Vien = '" + idnv + "' and Nhan_Vien.id_Chuc_Vu = Chuc_Vu.id_Chuc_Vu and Nhan_Vien.id_Phong_Ban = Phong_Ban.id_Phong_Ban", c.conn);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            string id_phongban;
            if (reader.Read())
            {
                id_phongban = reader["id_Phong_Ban"].ToString();

                var homNay = DateTime.Now;
                //Bước 1: Nạp file mẫu
                Document baoCao = new Document("DOC_BAO_CAO_LUONG_PART.doc");

                //Bước 2: Điền các thông tin cố định


                baoCao.MailMerge.Execute(new[] { "Ngay_Thang_Nam_BC" }, new[] { string.Format("Hà Nội, ngày {0} tháng {1} năm {2}", homNay.Day, homNay.Month, homNay.Year) });
                baoCao.MailMerge.Execute(new[] { "id_Nhan_Vien" }, new[] { reader["id_Nhan_Vien"].ToString() });
                baoCao.MailMerge.Execute(new[] { "Ho_Ten" }, new[] { txt_Ho_Ten.Text });
                baoCao.MailMerge.Execute(new[] { "Ngay_Sinh" }, new[] { dtp_NgaySinh.Value.ToString("dd/MM/yyyy") });
                baoCao.MailMerge.Execute(new[] { "SDT" }, new[] { reader["SDT"].ToString() });
                baoCao.MailMerge.Execute(new[] { "Que_Quan" }, new[] { reader["Que_Quan"].ToString() });
                baoCao.MailMerge.Execute(new[] { "Gmail" }, new[] { reader["Gmail"].ToString() });
                baoCao.MailMerge.Execute(new[] { "Phong_Ban" }, new[] { reader["Ten_Phong_Ban"].ToString() });
                baoCao.MailMerge.Execute(new[] { "Diem_Danh_days" }, new[] { day.ToString() });
                baoCao.MailMerge.Execute(new[] { "Di_Muon_days" }, new[] { lbl_ddmuon.Text });
                baoCao.MailMerge.Execute(new[] { "Nghi_Phep_days" }, new[] { lbl_np.Text });
                baoCao.MailMerge.Execute(new[] { "Tang_Ca_days" }, new[] { lbl_ddtangca.Text });
                baoCao.MailMerge.Execute(new[] { "Tang_Ca_hours" }, new[] { lbl_giotangca.Text });
                baoCao.MailMerge.Execute(new[] { "Nghi_Khong_Phep_days" }, new[] { lbl_nkp.Text });
                baoCao.MailMerge.Execute(new[] { "Chuc_Vu" }, new[] { reader["Ten_Chuc_Vu"].ToString() });
                baoCao.MailMerge.Execute(new[] { "Gio_Lam_hours" }, new[] { lbl_sogiolam.Text });

                //Bước 3: Điền thông tin lên bảng
                Table bangThongTinGiaDinh = baoCao.GetChild(NodeType.Table, 1, true) as Table;//Lấy bảng thứ 2 trong file mẫu
                int hangHienTai = 1;
                bangThongTinGiaDinh.InsertRows(hangHienTai, hangHienTai, day);
                reader.Close();
                for (int i = 1; i <= day; i++)
                {

                    SqlCommand sqlCommand1 = new SqlCommand("select * from Cham_Cong_Part_Time where id_Nhan_Vien = '" + idnv + "' and day(Ngay_Lam) = '" + i + "' and month(Ngay_Lam) = '" + month + "' and year(Ngay_Lam) = '" + year + "'", c.conn);
                    SqlDataReader sqlDataReader = sqlCommand1.ExecuteReader();
                    if (sqlDataReader.Read())
                    {
                        DateTime ngayLam = (DateTime)sqlDataReader["Ngay_Lam"];
                        string ngayLamChuoi = ngayLam.ToString("dd/MM/yyyy");
                        bangThongTinGiaDinh.PutValue(hangHienTai, 0, i.ToString());//Cột STT
                        bangThongTinGiaDinh.PutValue(hangHienTai, 1, ngayLamChuoi);//Cột Ngày Công
                        //bangThongTinGiaDinh.PutValue(hangHienTai, 2, sqlDataReader["Ten_Ca"].ToString());//Cột Tên Ca
                        bangThongTinGiaDinh.PutValue(hangHienTai, 2, sqlDataReader["Gio_Bat_Dau"].ToString());//Cột Bắt Đầu
                        bangThongTinGiaDinh.PutValue(hangHienTai, 3, sqlDataReader["Gio_Ket_Thuc"].ToString());//Cột Kêt Thúc

                        hangHienTai++;
                    }
                    sqlDataReader.Close();

                }

                // Lấy lương part
                SqlCommand sqlCommand2 = new SqlCommand("select Luong_Part from Luong_Part where id_Phong_Ban = '" + id_phongban + "'", c.conn);
                SqlDataReader sqlDataReader1 = sqlCommand2.ExecuteReader();
                double luong_part = 0;
                if (sqlDataReader1.Read())
                {
                    luong_part = sqlDataReader1.GetInt32(0);
                }
                sqlDataReader1.Close();
                baoCao.MailMerge.Execute(new[] { "Luong_Tren_Gio" }, new[] { luong_part.ToString("N0") });

                // Tính lương

                double luong_nhan = gio_lam_month * luong_part;
                baoCao.MailMerge.Execute(new[] { "Luong_Nhan" }, new[] { luong_nhan.ToString("N0") });

                //Bước 4: Lưu và mở file
                baoCao.SaveAndOpenFile("BaoCao.doc");
            }
        }

        public void Print_Full(string idnv)
        {
            c.connect();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int gio_lam_day = 0;
            int gio_lam_month = 0;
            int day = DateTime.Now.Day;


            for (int i = 1; i < daysInMonth; i++)
            {
                //Tính số giờ Đã làm trên tháng
                SqlCommand cmd = new SqlCommand("select DATEDIFF(hour, Gio_Bat_Dau, Gio_Ket_Thuc) AS DateDiff FROM Cham_Cong_Full_Time, Cong_Viec " +
                    "where Cham_Cong_Full_Time.id_Nhan_Vien = Cong_Viec.id_Nhan_Vien " +
                    "and Gio_Bat_Dau IS NOT NULL " +
                    "and Gio_Ket_Thuc IS NOT NULL " +
                    "and DAY(Ngay_Lam) = '" + i + "' " +
                    "and MONTH(Ngay_Lam) = '" + month + "' " +
                    "and YEAR(Ngay_Lam) = '" + year + "' " +
                    "and Cong_Viec.id_Nhan_Vien = '" + idnv + "';", c.conn);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    gio_lam_day = sqlDataReader.GetInt32(0);
                    gio_lam_month = gio_lam_month + gio_lam_day;
                    lbl_giotangca.Text = gio_lam_month.ToString();
                    giotangcafull = gio_lam_day;
                }
                else
                {

                }
                sqlDataReader.Close();
            }


            c.connect();
            SqlCommand sqlCommand = new SqlCommand("select * from Nhan_Vien, Phong_Ban, Chuc_Vu where Nhan_Vien.id_Nhan_Vien = '" + idnv + "' and Nhan_Vien.id_Chuc_Vu = Chuc_Vu.id_Chuc_Vu and Nhan_Vien.id_Phong_Ban = Phong_Ban.id_Phong_Ban", c.conn);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {


                var homNay = DateTime.Now;
                //Bước 1: Nạp file mẫu
                Document baoCao = new Document("DOC_BAO_CAO_LUONG_FULL.doc");

                //Bước 2: Điền các thông tin cố định


                baoCao.MailMerge.Execute(new[] { "Ngay_Thang_Nam_BC" }, new[] { string.Format("Hà Nội, ngày {0} tháng {1} năm {2}", homNay.Day, homNay.Month, homNay.Year) });
                baoCao.MailMerge.Execute(new[] { "id_Nhan_Vien" }, new[] { reader["id_Nhan_Vien"].ToString() });
                baoCao.MailMerge.Execute(new[] { "Ho_Ten" }, new[] { txt_Ho_Ten.Text });
                baoCao.MailMerge.Execute(new[] { "Ngay_Sinh" }, new[] { dtp_NgaySinh.Value.ToString("dd/MM/yyyy") });
                baoCao.MailMerge.Execute(new[] { "SDT" }, new[] { reader["SDT"].ToString() });
                baoCao.MailMerge.Execute(new[] { "Que_Quan" }, new[] { reader["Que_Quan"].ToString() });
                baoCao.MailMerge.Execute(new[] { "Gmail" }, new[] { reader["Gmail"].ToString() });
                baoCao.MailMerge.Execute(new[] { "Phong_Ban" }, new[] { reader["Ten_Phong_Ban"].ToString() });
                baoCao.MailMerge.Execute(new[] { "Diem_Danh_days" }, new[] { day.ToString() });
                baoCao.MailMerge.Execute(new[] { "Di_Muon_days" }, new[] { lbl_ddmuon.Text });
                baoCao.MailMerge.Execute(new[] { "Nghi_Phep_days" }, new[] { lbl_np.Text });
                baoCao.MailMerge.Execute(new[] { "Tang_Ca_days" }, new[] { lbl_ddtangca.Text });
                baoCao.MailMerge.Execute(new[] { "Tang_Ca_hours" }, new[] { lbl_giotangca.Text });
                baoCao.MailMerge.Execute(new[] { "Nghi_Khong_Phep_days" }, new[] { lbl_nkp.Text });
                baoCao.MailMerge.Execute(new[] { "Chuc_Vu" }, new[] { reader["Ten_Chuc_Vu"].ToString() });
                ngaydiemdanhfull = day;



                //Bước 3: Điền thông tin lên bảng
                Table bangThongTinGiaDinh = baoCao.GetChild(NodeType.Table, 1, true) as Table;//Lấy bảng thứ 2 trong file mẫu
                int hangHienTai = 1;
                bangThongTinGiaDinh.InsertRows(hangHienTai, hangHienTai, day);
                reader.Close();
                for (int i = 1; i <= day; i++)
                {

                    SqlCommand sqlCommand1 = new SqlCommand("select * from Cham_Cong_Full_Time where id_Nhan_Vien = '" + idnv + "' and day(Ngay_Lam) = '" + i + "' and month(Ngay_Lam) = '" + month + "' and year(Ngay_Lam) = '" + year + "'", c.conn);
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

                //Tính Lương Full Time

                //Đi muộn
                baoCao.MailMerge.Execute(new[] { "Ngay_Di_Muon" }, new[] { ngaydimuonfull.ToString() });
                double dm = (ngaydimuonfull * (1.65));
                baoCao.MailMerge.Execute(new[] { "Phan_Tram_Di_Muon" }, new[] { dm.ToString() });

                //Nghỉ Không Phép
                baoCao.MailMerge.Execute(new[] { "Ngay_Nghi_Khong_Phep" }, new[] { ngaynghikhongphepfull.ToString() });
                double kp = (ngaynghikhongphepfull * (3.3));
                baoCao.MailMerge.Execute(new[] { "Phan_Tram_Khong_Phep" }, new[] { kp.ToString() });
                
                //Phần Trăm Trừ
                double tru = 100 - (dm + kp);
                baoCao.MailMerge.Execute(new[] { "Phan_Tram_Tru" }, new[] { tru.ToString() });

                //Lấy lương
                SqlCommand sqlCommand2 = new SqlCommand("select * from Nhan_Vien where id_Nhan_Vien = '" + txt_IDNV.Text + "'", c.conn);
                SqlDataReader sqlDataReader2 = sqlCommand2.ExecuteReader();
                if (sqlDataReader2.Read())
                {
                    chucvu = sqlDataReader2["id_Chuc_Vu"].ToString();
                    phongban = sqlDataReader2["id_Phong_Ban"].ToString();
                }
                sqlDataReader2.Close();

                SqlCommand sqlCommand3 = new SqlCommand("select Luong_Full from Luong_Full where id_Phong_Ban = '" + phongban + "' and id_Chuc_Vu = '" + chucvu + "'", c.conn);
                SqlDataReader sqlDataReader1 = sqlCommand3.ExecuteReader();
                double luong = 0;
                if (sqlDataReader1.Read())
                {
                    luong = sqlDataReader1.GetInt32(0);
                    baoCao.MailMerge.Execute(new[] { "Luong_Co_Ban" }, new[] { luong.ToString("N0") });
                }
                sqlDataReader1.Close();
                double luongcoban1 = luong * (tru / 100);
                baoCao.MailMerge.Execute(new[] { "Luong_Co_Ban_1" }, new[] { luongcoban1.ToString("N0") });

                SqlCommand sqlCommand4 = new SqlCommand("select * from Cong_Viec where id_Nhan_Vien = '" + txt_IDNV.Text + "'", c.conn);
                SqlDataReader sqlDataReader3 = sqlCommand4.ExecuteReader();
                double hesoluong = 0;
                if (sqlDataReader3.Read())
                {
                    hesoluong = (double)sqlDataReader3["He_So_Luong"];
                    baoCao.MailMerge.Execute(new[] { "He_So_Luong" }, new[] { hesoluong.ToString("N0") });
                }
                double luong1 = luongcoban1 * hesoluong;
                baoCao.MailMerge.Execute(new[] { "Luong_1" }, new[] { luong1.ToString("N0") });

                //Tiền tăng ca

                double tien_tang_ca = gio_lam_month * 50000;
                baoCao.MailMerge.Execute(new[] { "Tien_Tang_Ca" }, new[] { tien_tang_ca.ToString("N0") });

                // Tổng lương được nhận

                double tong_luong = luong1 + tien_tang_ca;
                baoCao.MailMerge.Execute(new[] { "Luong_Nhan_Dong" }, new[] { tong_luong.ToString("N0") });

                //Bước 4: Lưu và mở file
                baoCao.SaveAndOpenFile("BaoCao.doc");
            }
        }

        public void InNhanVien()
        {
            c.connect();
            SqlCommand sqlCommand = new SqlCommand("select * from Nhan_Vien where id_Nhan_Vien = '" + txt_IDNV.Text + "'", c.conn);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.Read())
            {
                txt_Cong_Viec.Text = sqlDataReader["Cong_Viec"].ToString();
                txt_DiaChi.Text = sqlDataReader["Dia_Chi"].ToString();
                txt_Ho_Ten.Text = sqlDataReader["Ho_Ten"].ToString();

            }
            sqlDataReader.Close();
        }

        private void btn_in_Click(object sender, EventArgs e)
        {
            if (txt_IDNV.Text == "")
            {
                MessageBox.Show("Vui lòng nhập ID Nhân Viên để in lương","Thông Báo",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            }
            else
            {
                c.connect();
                SqlCommand cmd = new SqlCommand("select * from Cong_Viec where id_Nhan_Vien = '" + txt_IDNV.Text + "'", c.conn);
                SqlDataReader reader = cmd.ExecuteReader();
                string ckcv = "";
                if (reader.Read())
                {

                    ckcv = reader["Loai_Cong_Viec"].ToString();


                }
                reader.Close();

                if (ckcv == "Full Time")
                {
                    Print_Full(txt_IDNV.Text);
                }
                else if (ckcv == "Part Time")
                {
                    Print_Part(txt_IDNV.Text);
                }
            }

        }

        private void dtg_bangcong_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frm_Cham_Cong_Ql_Load(object sender, EventArgs e)
        {
            pal_tk_full.Show();
            pal_tk_part.Hide();
            label6.Hide();
        }
    }
}
