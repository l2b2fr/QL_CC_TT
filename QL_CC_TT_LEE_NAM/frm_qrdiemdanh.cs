using AForge.Video;
using AForge.Video.DirectShow;
using Aspose.Words.Drawing;
using DevExpress.Utils.MVVM;
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
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ZXing;

namespace QL_CC_TT_LEE_NAM
{
    public partial class frm_qrdiemdanh : Form
    {
        public frm_qrdiemdanh()
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

        public void checknv(string id)
        {
            SqlCommand cmd = new SqlCommand("select count(*) from Nhan_Vien where id_Nhan_Vien = '" + id + "'", c.conn);
            SqlDataReader dr = cmd.ExecuteReader();
            int chek = 0;

            while (dr.Read())
            {
                chek = dr.GetInt32(0);
                if (chek == 0)
                {
                    MessageBox.Show("Bạn không phải là nhân viên công ty", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cknv = false;
                    captureDevice = new VideoCaptureDevice(filter[comboBox1.SelectedIndex].MonikerString);
                    captureDevice.NewFrame += CaptureDevice_NewFrame;
                    captureDevice.Start();
                    timer1.Start();
                }
                else
                {
                    cknv = true;
                }
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
                    captureDevice = new VideoCaptureDevice(filter[comboBox1.SelectedIndex].MonikerString);
                    captureDevice.NewFrame += CaptureDevice_NewFrame;
                    captureDevice.Start();
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
                                    */

                                    diemdanhthanhcong();
                                    //MessageBox.Show("Điểm danh thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    captureDevice = new VideoCaptureDevice(filter[comboBox1.SelectedIndex].MonikerString);
                                    captureDevice.NewFrame += CaptureDevice_NewFrame;
                                    captureDevice.Start();
                                    timer1.Start();

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

                                    /*
                                    tbdiemdanh tbdiemdanh = new tbdiemdanh();
                                    tbdiemdanh.ShowDialog();
                                    */
                                    diemdanhthanhcong_dimuon();
                                    //MessageBox.Show("Điểm danh thành công, Hôm nay bạn đã đi muộn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    captureDevice = new VideoCaptureDevice(filter[comboBox1.SelectedIndex].MonikerString);
                                    captureDevice.NewFrame += CaptureDevice_NewFrame;
                                    captureDevice.Start();
                                    timer1.Start();

                                    pic_thnhcong.Show();
                                    lbl_thanhcong.Show();
                                }
                                else if (DateTime.Now.TimeOfDay > new TimeSpan(11, 30, 0) && DateTime.Now.TimeOfDay < new TimeSpan(22, 0, 0))
                                {
                                    /*
                                    tbdiemdanh tbdiemdanh = new tbdiemdanh();
                                    tbdiemdanh.ShowDialog();
                                    */
                                    diemdanhthatbai_dimuon();
                                    //MessageBox.Show("Điểm danh thất bại , do bạn đi làm quá muộn công ty đã tính là bạn không đi làm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    txt = false;
                                    captureDevice = new VideoCaptureDevice(filter[comboBox1.SelectedIndex].MonikerString);
                                    captureDevice.NewFrame += CaptureDevice_NewFrame;
                                    captureDevice.Start();
                                    timer1.Start();

                                    pic_thatbai.Show();
                                    lbl_thatbai.Show();
                                }
                                else
                                {
                                    /*
                                    tbdiemdanh tbdiemdanh = new tbdiemdanh();
                                    tbdiemdanh.ShowDialog();
                                    */
                                    khongdiemdanhlucnuadem();
                                    //MessageBox.Show("Không ai đi làm vào lúc đêm cả !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    txt = false;
                                    captureDevice = new VideoCaptureDevice(filter[comboBox1.SelectedIndex].MonikerString);
                                    captureDevice.NewFrame += CaptureDevice_NewFrame;
                                    captureDevice.Start();
                                    timer1.Start();

                                    pic_thatbai.Show();
                                    lbl_thatbai.Show();
                                }
                            }
                            else
                            {
                                /*
                                tbdiemdanh tbdiemdanh = new tbdiemdanh();
                                tbdiemdanh.ShowDialog();
                                */
                                homnaybandadiemdanh();
                                //MessageBox.Show("Hôm Nay Bạn Đã Điểm Danh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                captureDevice = new VideoCaptureDevice(filter[comboBox1.SelectedIndex].MonikerString);
                                captureDevice.NewFrame += CaptureDevice_NewFrame;
                                captureDevice.Start();
                                timer1.Start();
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
                captureDevice = new VideoCaptureDevice(filter[comboBox1.SelectedIndex].MonikerString);
                captureDevice.NewFrame += CaptureDevice_NewFrame;
                captureDevice.Start();
                timer1.Start();
                pic_thatbai.Show();
                lbl_thatbai.Show();
            }

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

        private void frm_qrdiemdanh_Load(object sender, EventArgs e)
        {
            btn_ketthuc.Hide();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int daysInMonth = DateTime.DaysInMonth(year, month);


            c.connect();
            filter = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filter)
            {
                comboBox1.Items.Add(filterInfo.Name);
            }
            comboBox1.SelectedIndex = 0;

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

            pic_thatbai.Hide();
            pic_thnhcong.Hide();
            lbl_thanhcong.Hide();
            lbl_thatbai.Hide();

        }



        private void btn_diemdanh_Click(object sender, EventArgs e)
        {
            btn_diemdanh.Hide();
            btn_ketthuc.Show();
            captureDevice = new VideoCaptureDevice(filter[comboBox1.SelectedIndex].MonikerString);
            captureDevice.NewFrame += CaptureDevice_NewFrame;
            captureDevice.Start();
            timer1.Start();
        }

        private void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pic_cam.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            c.connect();
            if (pic_cam.Image != null)
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode((Bitmap)pic_cam.Image);
                if (result != null)
                {
                    id_Nhan_Vien1 = result.ToString();
                    timer1.Stop();
                    if (captureDevice.IsRunning)
                    {
                        captureDevice.Stop();
                    }
                    Diem_Rank(id_Nhan_Vien1);
                }
            }
        }

        private void frm_qrdiemdanh_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (captureDevice is null)
            {

            }
            else
            {
                if (captureDevice.IsRunning)
                {
                    captureDevice.Stop();
                }
            }
        }

        private void btn_diemdanh_Click_1(object sender, EventArgs e)
        {
            btn_diemdanh.Hide();
            btn_ketthuc.Show();
            captureDevice = new VideoCaptureDevice(filter[comboBox1.SelectedIndex].MonikerString);
            captureDevice.NewFrame += CaptureDevice_NewFrame;
            captureDevice.Start();
            timer1.Start();
        }

        private void btn_ketthuc_Click_1(object sender, EventArgs e)
        {
            captureDevice.Stop();
            pic_cam.Image = null;
            btn_diemdanh.Show();
            btn_ketthuc.Hide();
        }

        private void btn_ketthuc_Click(object sender, EventArgs e)
        {
            captureDevice.Stop();
            pic_cam.Image = null;
            btn_diemdanh.Show();
            btn_ketthuc.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
