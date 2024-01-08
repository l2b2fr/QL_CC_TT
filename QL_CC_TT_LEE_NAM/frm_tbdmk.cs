using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QRCoder.PayloadGenerator;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Data.SqlTypes;

namespace QL_CC_TT_LEE_NAM
{
    public partial class frm_tbdmk : Form
    {
        public static int so, otp;

        public frm_tbdmk()
        {
            InitializeComponent();
        }
        Random rand = new Random();
        ConnectData c = new ConnectData();

        public void doimatkhau()
        {
            c.connect();
            //string sqlString = "update Nguoi_Dung set Mat_Khau = '" + frm_Doi_Mat_Khau.mkm + "' where id_Nhan_Vien = '" + frm_Dang_Nhap.id_Nhan_Vien + "'";
            SqlCommand sqlCommand11 = new SqlCommand("update Nguoi_Dung set Mat_Khau = '" + frm_Doi_Mat_Khau.mkm + "' where id_Nhan_Vien = '" + frm_Dang_Nhap.id_Nhan_Vien + "'", c.conn);
            sqlCommand11.ExecuteNonQuery();

            //ConnectData.ExecuteNonQuery(sqlString);

            DialogResult dialogResult = MessageBox.Show("Bạn Đã Đổi Mật Khẩu Thành Công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.OK)
            {
                this.Close();
                frm_Doi_Mat_Khau frm_Doi_Mat_Khau = new frm_Doi_Mat_Khau();
                frm_Doi_Mat_Khau.ShowDialog();
            }
        }

        private void btn_xacnhan_Click(object sender, EventArgs e)
        {
            so = Convert.ToInt32(txt_mxn.Text);

            if (so == frm_Doi_Mat_Khau.otp || so == frm_tbdmk.otp)
            {
                doimatkhau();
            }
            else
            {
                MessageBox.Show("Vui Lòng Nhập Đúng Mã Xác Nhận", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public void SendEmail(string emailTo)
        {

            otp = rand.Next(100000, 1000000);
            var fromAddress = new MailAddress("leminhnamyb2004@gmail.com", "Công Ty Truyền Thông");
            var toAddress = new MailAddress(emailTo, " Duy Tung");
            const string fromPassword = "vzhoshhotwqitzqh";
            const string subject = "Xác nhận";
            string body = "Xin chào,\n\nMã xác nhận của bạn như sau \nMã xác nhận của bạn là :  " + otp.ToString() + "\n\nCảm ơn bạn đã sử dụng dịch vụ của chúng tôi.\n\nTrân trọng,\nCông Ty Truyền Thông";


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
            smtp.Dispose();
            smtp = null;

        }

        private void btn_guilai_Click(object sender, EventArgs e)
        {
            SendEmail(frm_Doi_Mat_Khau.gmail);
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            this.Close();
            frm_Doi_Mat_Khau frm_Doi_Mat_Khau = new frm_Doi_Mat_Khau();
            frm_Doi_Mat_Khau.ShowDialog();
        }

        private void frm_tbdmk_Load(object sender, EventArgs e)
        {
            SendEmail(frm_Doi_Mat_Khau.gmail);
        }
    }
}
