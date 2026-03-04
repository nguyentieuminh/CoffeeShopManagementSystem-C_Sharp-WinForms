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

namespace Quản_lý_bán_hàng_cafe
{
    public partial class Form1 : Form
    {
        string connStr = @"Data Source=LAPTOP-S0J0KENB\MSSQLSERVER01;Initial Catalog=QuanLiBanCafe;Integrated Security=True;TrustServerCertificate=True;";
        SqlConnection conn = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string TenDN = guna2TextBox_TenDN.Text;
            string MatKhau = guna2TextBox_MatKhau.Text;
            string sql = "SELECT COUNT(*) FROM DangNhap WHERE TenDN=@ten AND MatKhau=@mk";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ten", TenDN);
            cmd.Parameters.AddWithValue("@mk", MatKhau);
            int count = (int)cmd.ExecuteScalar();
            if (count > 0)
            {
                Dashboard dashboard = new Dashboard();
                dashboard.ShowDialog();
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connStr);
            conn.Open();
        }
    }
}
