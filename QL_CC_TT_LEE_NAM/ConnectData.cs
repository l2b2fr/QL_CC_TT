using DevExpress.ClipboardSource.SpreadsheetML;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_CC_TT_LEE_NAM
{
    public class ConnectData
    {
        public SqlConnection conn;

        public void connect()
        {
            
            String strCon = @"Data Source=DESKTOP-0GBM1DD\LEE_NAM_SQL;Initial Catalog=QL_CC_TT;Integrated Security=False;User ID=dothithom;Password=070804";
            try
            {
                conn = new SqlConnection(strCon);
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void disconnect()
        {
            conn.Close();
            conn.Dispose();
            conn = null;
        }
        public static bool ExecuteNonQuery(string sql)
        {
            string strCon = @"Data Source=DESKTOP-0GBM1DD\LEE_NAM_SQL;Initial Catalog=QL_CC_TT;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(strCon))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
    }
}
