using AForge.Video.DirectShow;
using FaceRecognition;
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
using ZXing;
using AForge.Video;
using Aspose.Words.Drawing;
using System.IO;
using System.Windows.Media;
using Emgu.CV.Flann;
using System.Media;

namespace QL_CC_TT_LEE_NAM
{
    public partial class frmdiemdanhkhuanmat : Form
    {
        public frmdiemdanhkhuanmat()
        {
            InitializeComponent();
        }
        public static string id_Nhan_Vien1;
        public static bool txt;
        public static bool ckcv, ckcv1;
        public static bool cknv, ktnv;

        FilterInfoCollection filter;
        VideoCaptureDevice captureDevice;
        ConnectData c = new ConnectData();

        FaceRec faceRec = new FaceRec();

        public void ck_cong_viec(string idnv)
        {
            string full = "Full Time";
            string part = "Part Time";
            c.connect();
            SqlCommand sql = new SqlCommand("select count(*) from Cong_Viec where id_Nhan_Vien = '" + idnv + "' and Loai_Cong_Viec = '" + full + "' or Loai_Cong_Viec = '" + part + "'", c.conn);
            SqlDataReader reader = sql.ExecuteReader();
            int ck = 0;
            while (reader.Read())
            {
                ck = reader.GetInt32(0);
                if (ck == 0)
                {
                    MessageBox.Show("Bạn chưa có công việc ở công ty.\nVui lòng gặp quản lý để thiết lập công việc", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ckcv = false;
                    timer1.Start();
                }
                else
                {
                    ckcv = true;
                }
            }

        }


        public void kiemtranv(string idnv)
        {
            c.connect();
            SqlCommand sql = new SqlCommand("select count(*) from Nhan_Vien where id_Nhan_Vien = '" + idnv + "'", c.conn);
            SqlDataReader dr = sql.ExecuteReader();
            int ck = 0;
            while (dr.Read())
            {
                ck = dr.GetInt32(0);
                if (ck == 0)
                {
                    ktnv = false; break;
                }
                else
                {
                    ktnv |= true; break;
                }
            }
            dr.Close();
        }

        public void diemdanhthanhcong()
        {
            //điểm danh thành công
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = "diemdanhthanhcong.WAV";
            player.Play();
        }

        public void homnaybandadiemdanh()
        {
            //hôm nay bạn đã điểm danh
            System.Media.SoundPlayer player1 = new System.Media.SoundPlayer();
            player1.SoundLocation = "homnaybandadiemdanh.WAV";
            player1.Play();
        }

        public void khongdiemdanhlucnuadem()
        {
            //không điểm danh lúc nửa đêm
            System.Media.SoundPlayer player2 = new System.Media.SoundPlayer();
            player2.SoundLocation = "khongdiemdanhlucnuadem.WAV";
            player2.Play();
        }

        public void diemdanhthanhcong_dimuon()
        {
            //điểm danh thành công di muộn
            System.Media.SoundPlayer player3 = new System.Media.SoundPlayer();
            player3.SoundLocation = "diemdanhthanhcong_dimuon.WAV";
            player3.Play();
        }

        public void diemdanhthatbai_dimuon()
        {
            //điểm danh thất bại đi muộn
            System.Media.SoundPlayer player4 = new System.Media.SoundPlayer();
            player4.SoundLocation = "diemdanhthatbai_dodimuon.WAV";
            player4.Play();
        }

        public void Diem_Rank(string id_Nhan_Vien)
        {
            int ck1;
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            c.connect();
            // Kiểm tra nhân viên có tồn tai không
            kiemtranv(id_Nhan_Vien);

            if (ktnv == true)
            {   //Kiểm tra xem có công việc ở công ty hay chưa
                ck_cong_viec(id_Nhan_Vien);
                if (ckcv == true)
                {
                    //Load ngày công với nếu nhân viên là part time thì bắt đang nhập vào công ty để điểm danh

                    LoadNgayCong1(id_Nhan_Vien);
                    if (ckcv1 == true)
                    {
                        int ck = 0;
                        SqlCommand cmd = new SqlCommand("select COUNT(id_Nhan_Vien) from Cham_Cong_Full_Time where id_Nhan_Vien = '" + id_Nhan_Vien + "' and Ngay_Lam = '" + DateTime.Now + "' and Diem_Danh = '" + true + "'", c.conn);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            ck = dr.GetInt32(0);
                            if (ck == 0)
                            {
                                if (DateTime.Now.TimeOfDay < new TimeSpan(8, 30, 0) && DateTime.Now.TimeOfDay > new TimeSpan(5, 0, 0))
                                {
                                    dr.Close();
                                    SqlCommand sqlCommand = new SqlCommand("update Cham_Cong_Full_Time set id_Nhan_Vien = '" + id_Nhan_Vien + "',Diem_Danh = '" + true + "',Gio_Vao = '" + DateTime.Now + "'where id_Nhan_Vien = '" + id_Nhan_Vien + "' and Ngay_Lam = '" + DateTime.Now + "'", c.conn);
                                    SqlDataReader reader = sqlCommand.ExecuteReader();
                                    reader.Close();
                                    txt = true;

                                    DataSet ds = new DataSet();
                                    string query = "select id_Nhan_Vien 'Mã Công Việc', Ngay_Lam 'Ngày Điểm Danh', Gio_Vao 'Giờ Vào',Diem_Danh 'Điểm Danh', Di_Muon 'Đi Làm Muộn', Tang_Ca 'Tăng Ca', Gio_Bat_Dau 'Giờ Bắt Đầu', Gio_Ket_Thuc 'Giờ Kết Thúc' from Cham_Cong_Full_Time where Ngay_Lam = '" + DateTime.Now + "' and Gio_Vao is not null and Diem_Danh = '" + true + "'";
                                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
                                    sqlDataAdapter.Fill(ds);
                                    datagrv_ThongKeFull.DataSource = ds.Tables[0];
                                    datagrv_ThongKeFull.ReadOnly = true;

                                    DataSet ds1 = new DataSet();
                                    string query1 = "select Nhan_Vien.id_Nhan_Vien 'Mã Công Việc', Ho_Ten 'Họ Tên',Ngay_Lam 'Ngày Làm', Diem_Danh 'Điểm Danh' from Cham_Cong_Full_Time, Nhan_Vien where Diem_Danh is null and Ngay_Lam = '" + DateTime.Now + "' and Nhan_Vien.id_Nhan_Vien = Cham_Cong_Full_Time.id_Nhan_Vien";
                                    SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(query1, c.conn);
                                    sqlDataAdapter1.Fill(ds1);
                                    dtgrv_chuadiemdanh.DataSource = ds1.Tables[0];
                                    dtgrv_chuadiemdanh.ReadOnly = true;

                                    /*
                                    tbdiemdanh tbdiemdanh = new tbdiemdanh();
                                    tbdiemdanh.ShowDialog();
                                    MessageBox.Show("Điểm danh thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    timer1.Start();
                                    */
                                    diemdanhthanhcong();
                                    pic_thnhcong.Show();
                                    lbl_thanhcong.Show();
                                }
                                else if (DateTime.Now.TimeOfDay > new TimeSpan(8, 30, 0) && DateTime.Now.TimeOfDay <= new TimeSpan(11, 30, 0))
                                {
                                    dr.Close();
                                    SqlCommand sqlCommand = new SqlCommand("update Cham_Cong_Full_Time set id_Nhan_Vien = '" + id_Nhan_Vien + "',Diem_Danh = '" + true + "',Gio_Vao = '" + DateTime.Now + "',Di_Muon = '" + true + "' where id_Nhan_Vien = '" + id_Nhan_Vien + "' and Ngay_Lam = '" + DateTime.Now + "'", c.conn);
                                    SqlDataReader reader = sqlCommand.ExecuteReader();
                                    reader.Close();
                                    txt = true;

                                    DataSet ds = new DataSet();
                                    string query = "select id_Nhan_Vien 'Mã Công Việc', Ngay_Lam 'Ngày Điểm Danh', Gio_Vao 'Giờ Vào',Diem_Danh 'Điểm Danh', Di_Muon 'Đi Làm Muộn', Tang_Ca 'Tăng Ca', Gio_Bat_Dau 'Giờ Bắt Đầu', Gio_Ket_Thuc 'Giờ Kết Thúc' from Cham_Cong_Full_Time where Ngay_Lam = '" + DateTime.Now + "' and Gio_Vao is not null and Diem_Danh = '" + true + "'";
                                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
                                    sqlDataAdapter.Fill(ds);
                                    datagrv_ThongKeFull.DataSource = ds.Tables[0];
                                    datagrv_ThongKeFull.ReadOnly = true;

                                    DataSet ds1 = new DataSet();
                                    string query1 = "select Nhan_Vien.id_Nhan_Vien 'Mã Công Việc', Ho_Ten 'Họ Tên',Ngay_Lam 'Ngày Làm', Diem_Danh 'Điểm Danh' from Cham_Cong_Full_Time, Nhan_Vien where Diem_Danh is null and Ngay_Lam = '" + DateTime.Now + "' and Nhan_Vien.id_Nhan_Vien = Cham_Cong_Full_Time.id_Nhan_Vien";
                                    SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(query1, c.conn);
                                    sqlDataAdapter1.Fill(ds1);
                                    dtgrv_chuadiemdanh.DataSource = ds1.Tables[0];
                                    dtgrv_chuadiemdanh.ReadOnly = true;

                                    //tbdiemdanh tbdiemdanh = new tbdiemdanh();
                                    //tbdiemdanh.ShowDialog();
                                    //MessageBox.Show("Điểm danh thành công, Hôm nay bạn đã đi muộn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //timer1.Start();
                                    diemdanhthanhcong_dimuon();
                                    pic_thnhcong.Show();
                                    lbl_thanhcong.Show();
                                }
                                else if (DateTime.Now.TimeOfDay > new TimeSpan(11, 30, 0) && DateTime.Now.TimeOfDay < new TimeSpan(22, 0, 0))
                                {
                                    /*
                                    tbdiemdanh tbdiemdanh = new tbdiemdanh();
                                    tbdiemdanh.ShowDialog();
                                    MessageBox.Show("Điểm danh thất bại , do bạn đi làm quá muộn công ty đã tính là bạn không đi làm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    txt = false;
                                    timer1.Start();
                                    */
                                    diemdanhthatbai_dimuon();
                                    pic_thatbai.Show();
                                    lbl_thatbai.Show();
                                }
                                else
                                {
                                    /*
                                    tbdiemdanh tbdiemdanh = new tbdiemdanh();
                                    tbdiemdanh.ShowDialog();
                                    MessageBox.Show("Không ai đi làm vào lúc đêm cả !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    txt = false;
                                    timer1.Start();
                                    */

                                    khongdiemdanhlucnuadem();
                                    pic_thatbai.Show();
                                    lbl_thatbai.Show();
                                }
                            }
                            else
                            {
                                /*
                                tbdiemdanh tbdiemdanh = new tbdiemdanh();
                                tbdiemdanh.ShowDialog();
                                MessageBox.Show("Hôm Nay Bạn Đã Điểm Danh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                timer1.Start();
                                */
                                homnaybandadiemdanh();
                                pic_thnhcong.Show();
                                lbl_thanhcong.Show();
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Bạn Không Phải Nhân Viên Của Công Ty!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pic_thatbai.Show();
                lbl_thatbai.Show();
                timer1.Start();
            }

        }

        public void LoadNgayCong1(string id_Nhan_Vien)
        {
            c.connect();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            string dk = "Full Time";
            SqlCommand sql = new SqlCommand("select count(id_Nhan_Vien) from Cong_Viec where id_Nhan_Vien = '" + id_Nhan_Vien + "' and Loai_Cong_Viec = '" + dk + "'", c.conn);
            SqlDataReader reader = sql.ExecuteReader();
            int ck = 0;
            if (reader.Read())
            {
                ck = reader.GetInt32(0);
                if (ck != 0)
                {
                    reader.Close();
                    SqlCommand cmd = new SqlCommand("select count(id_Nhan_Vien) from Cham_Cong_Full_Time where id_Nhan_Vien = '" + id_Nhan_Vien + "' and year(Ngay_Lam) = '" + year + "' and month(Ngay_Lam) = '" + month + "'", c.conn);
                    SqlDataReader sqlDataReader = cmd.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        int ck1 = sqlDataReader.GetInt32(0);
                        if (ck1 == 0)
                        {
                            sqlDataReader.Close();
                            for (int i = day; i <= daysInMonth; i++)
                            {
                                DateTime dateValue = new DateTime(year, month, i, 8, 0, 0);
                                if (dateValue.DayOfWeek == DayOfWeek.Saturday || dateValue.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    string ngay = dateValue.ToString("yyyy-MM-dd");
                                    SqlCommand cmd1 = new SqlCommand("insert into Cham_Cong_Full_Time (id_Nhan_Vien,Ngay_Lam,Diem_Danh,Gio_Vao) values ('" + id_Nhan_Vien + "','" + ngay + "', '" + true + "', '" + dateValue + "')", c.conn);
                                    cmd1.ExecuteNonQuery();
                                }
                                else
                                {
                                    string ngay = dateValue.ToString("yyyy-MM-dd");
                                    SqlCommand cmd1 = new SqlCommand("insert into Cham_Cong_Full_Time (id_Nhan_Vien,Ngay_Lam) values ('" + id_Nhan_Vien + "','" + ngay + "')", c.conn);
                                    cmd1.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    sqlDataReader.Close();
                    ckcv1 = true;
                }
                else
                {
                    MessageBox.Show("Bạn Là Nhân Viên Part Time Hãy Đăng Nhập Vào Máy Tính Công Ty Để Điểm Danh", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ckcv1 = false;
                }
            }
            reader.Close();

        }

        public void chuyendoi(string id)
        {

            if (id != "")
            {
                // Khai báo một chuỗi string có chứa dấu _
                string s = id;

                // Tìm vị trí đầu tiên của dấu _ trong chuỗi
                int index = s.IndexOf('_');

                // Nếu tìm thấy dấu _, cắt chuỗi từ vị trí 0 đến vị trí trước dấu _
                string result = s.Substring(0, index);

                Diem_Rank(result);
            }

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            chuyendoi(lblidnvmat.Text);
        }

        private void btn_diemdanh_Click_1(object sender, EventArgs e)
        {
            faceRec.openCamera(pic_cam, pickhuanmat);
            faceRec.isTrained = true;
            faceRec.getPersonName(lblidnvmat);
            // Tìm vị trí đầu tiên của dấu _ trong chuỗi
            timer1.Start();
        }

        private void btn_diemdanh_Click(object sender, EventArgs e)
        {
            faceRec.openCamera(pic_cam, pickhuanmat);
            faceRec.isTrained = true;
            faceRec.getPersonName(lblidnvmat);
            // Tìm vị trí đầu tiên của dấu _ trong chuỗi
            timer1.Start();
        }

        private void frmdiemdanhkhuanmat_Load(object sender, EventArgs e)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            c.connect();

            DataSet ds = new DataSet();
            string query = "select id_Nhan_Vien 'Mã Công Việc', Ngay_Lam 'Ngày Điểm Danh', Gio_Vao 'Giờ Vào',Diem_Danh 'Điểm Danh', Di_Muon 'Đi Làm Muộn', Tang_Ca 'Tăng Ca', Gio_Bat_Dau 'Giờ Bắt Đầu', Gio_Ket_Thuc 'Giờ Kết Thúc' from Cham_Cong_Full_Time where Ngay_Lam = '" + DateTime.Now + "' and Gio_Vao is not null and Diem_Danh = '" + true + "'";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, c.conn);
            sqlDataAdapter.Fill(ds);
            datagrv_ThongKeFull.DataSource = ds.Tables[0];
            datagrv_ThongKeFull.DataSource = ds.Tables[0];
            datagrv_ThongKeFull.ReadOnly = true;

            DataSet ds1 = new DataSet();
            string query1 = "select Nhan_Vien.id_Nhan_Vien 'Mã Công Việc', Ho_Ten 'Họ Tên',Ngay_Lam 'Ngày Làm', Diem_Danh 'Điểm Danh' from Cham_Cong_Full_Time, Nhan_Vien where Diem_Danh is null and Ngay_Lam = '" + DateTime.Now + "' and Nhan_Vien.id_Nhan_Vien = Cham_Cong_Full_Time.id_Nhan_Vien";
            SqlDataAdapter sqlDataAdapter1 = new SqlDataAdapter(query1, c.conn);
            sqlDataAdapter1.Fill(ds1);
            dtgrv_chuadiemdanh.DataSource = ds1.Tables[0];
            dtgrv_chuadiemdanh.ReadOnly = true;
            pic_thnhcong.Hide();
            pic_thatbai.Hide();
            lbl_thanhcong.Hide();
            lbl_thatbai.Hide();
        }
    }
}
