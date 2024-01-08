using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QL_CC_TT_LEE_NAM
{
    public partial class frm_Quen_Mat_Khau : Form
    {
        public frm_Quen_Mat_Khau()
        {
            InitializeComponent();
        }

        Random rand = new Random();
        public static int otp;
        ConnectData c = new ConnectData();

        public void SendEmail(string emailTo)
        {
            otp = rand.Next(100000, 1000000);
            var fromAddress = new MailAddress("leminhnamyb2004@gmail.com", "Công Ty Truyền Thông");
            var toAddress = new MailAddress(emailTo, " Duy Tung");
            const string fromPassword = "vzhoshhotwqitzqh";
            const string subject = "Xác nhận";
            string body = "Xin chào,\n\nMật Khẩu của bạn như sau \nMật Khẩu của bạn là :  " + otp.ToString() + "\n\nCảm ơn bạn đã sử dụng dịch vụ của chúng tôi.\n\nTrân trọng,\nCông Ty Truyền Thông";
            SqlCommand sqlCommand1 = new SqlCommand("update Nguoi_Dung set Mat_Khau = '" + otp.ToString() + "' where id_Nhan_Vien = '" + txt_manv.Text + "'", c.conn); ;
            SqlDataReader sqlDataReader1 = sqlCommand1.ExecuteReader();
            sqlDataReader1.Close();
            //ối dồi ôis
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            DialogResult dt = MessageBox.Show("Bạn có muốn quay lại !", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dt == DialogResult.Yes)
            {
                this.Close();
                frm_Dang_Nhap frm_Dang_Nhap = new frm_Dang_Nhap();
                frm_Dang_Nhap.ShowDialog();
            }
        }

        private void btn_dangnhap_Click(object sender, EventArgs e)
        {

            c.connect();

            if (txt_taikhoan.Text == "" || txt_manv.Text == "" || txt_gmail.Text == "")
            {
                MessageBox.Show("Vui Lòng Nhập Đầy Đủ Thông Tin", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    var email = new MailAddress(txt_gmail.Text);
                    SqlCommand sqlCommand = new SqlCommand("select COUNT(id_Nhan_Vien) from Nguoi_Dung where id_Nhan_Vien = '" + txt_manv.Text + "' and Tai_Khoan = '" + txt_taikhoan.Text + "' and Gmail = '" + txt_gmail.Text + "'", c.conn);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    int ck = 0;
                    if (reader.Read())
                    {
                        ck = reader.GetInt32(0);
                        if (ck == 0)
                        {
                            MessageBox.Show("Vui Lòng Nhập Đúng Thông Tin Để Lấy Mật Khẩu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txt_manv.Text = "";
                            txt_taikhoan.Text = "";
                            txt_gmail.Text = "";
                            txt_taikhoan.Focus();
                        }
                        else
                        {
                            reader.Close();
                            SendEmail(txt_gmail.Text);
                            MessageBox.Show("Mật khẩu đã gửi vào Gmail của bạn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btn_laymatkhau.Enabled = false;
                        }
                    }
                }
                catch (FormatException)
                {
                    // Handle invalid email format.
                    MessageBox.Show("Bạn nhập định dạng gmail không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
