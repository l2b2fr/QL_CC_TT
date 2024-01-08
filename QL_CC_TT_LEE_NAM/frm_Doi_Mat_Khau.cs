using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_CC_TT_LEE_NAM
{
    public partial class frm_Doi_Mat_Khau : Form
    {
        public frm_Doi_Mat_Khau()
        {
            InitializeComponent();
        }

        ConnectData c = new ConnectData();
        Random rand = new Random();
        public static int otp;
        public static string gmail, tk, mkm;


        public void SendEmail(string emailTo)
        {

            tk = txt_taikhoan.Text;
            mkm = txt_matkhaumoi.Text;

            otp = rand.Next(100000, 1000000);
            var fromAddress = new MailAddress("leminhnamyb2004@gmail.com", "Công Ty Truyền Thông");
            var toAddress = new MailAddress(emailTo, " Duy Tung");
            const string fromPassword = "vzhoshhotwqitzqh";
            const string subject = "Xác nhận";
            string body = "Xin chào,\n\nMã xác nhận của bạn như sau \nMã xác nhận của bạn là :  " + otp.ToString() + "\n\nCảm ơn bạn đã sử dụng dịch vụ của chúng tôi.\n\nTrân trọng,\nCông Ty Truyền Thông";

            gmail = emailTo;

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
            frm_tbdmk frm_Tbdmk = new frm_tbdmk();
            frm_Tbdmk.ShowDialog();
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            this.Close();
            frm_Dang_Nhap frm_Dang_Nhap = new frm_Dang_Nhap();
            frm_Dang_Nhap.Show();
        }

        private void frm_Doi_Mat_Khau_Load(object sender, EventArgs e)
        {

        }

        private void btn_doimatkhau_Click(object sender, EventArgs e)
        {
            tk = txt_taikhoan.Text;
            mkm = txt_matkhaumoi.Text;

            c.connect();
            if (txt_matkhaumoi.Text != txt_nhaplai.Text)
            {
                MessageBox.Show("Vui Lòng Nhập Mật Khẩu Mới Và Nhập Lại Mật Khẩu Mới Giống Nhau", "Thông Báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                txt_matkhaumoi.Text = "";
                txt_nhaplai.Text = "";
                txt_matkhaumoi.Focus();
            }
            else if (txt_taikhoan.Text == "" || txt_matkhau.Text == "" || txt_matkhaumoi.Text == "" || txt_nhaplai.Text == "" || txt_gmail.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin !", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    var email = new MailAddress(txt_gmail.Text);
                    SqlCommand cmd = new SqlCommand("select count(*) from Nguoi_Dung where Tai_Khoan = '" + txt_taikhoan.Text + "' and Mat_Khau = '" + txt_matkhau.Text + "' and Gmail = '" + txt_gmail.Text + "'", c.conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int ck = 0;
                    if (reader.Read())
                    {
                        ck = reader.GetInt32(0);
                        reader.Close();
                        if (ck == 0)
                        {
                            MessageBox.Show("Bạn vui lòng nhập đúng thông tin ", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            txt_taikhoan.Text = "";
                            txt_matkhau.Text = "";
                            txt_matkhaumoi.Text = "";
                            txt_nhaplai.Text = "";
                            txt_gmail.Text = "";
                            txt_taikhoan.Focus();
                        }
                        else
                        {
                            //SendEmail(txt_gmail.Text);
                            gmail = txt_gmail.Text;
                            frm_tbdmk frm_Tbdmk = new frm_tbdmk();
                            frm_Tbdmk.ShowDialog();
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txt_matkhau.PasswordChar = '\0';
            }
            else
            {
                txt_matkhau.PasswordChar = '*';
            }
        }
    }
}
