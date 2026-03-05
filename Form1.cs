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
using System.Security.Cryptography;

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
        private string MaHoaSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string TenDN = guna2TextBox_TenDN.Text;
            string MatKhau = MaHoaSHA256(guna2TextBox_MatKhau.Text);


            if (TenDN == "")
            {
                MessageBox.Show("Không được để trống tên đăng nhập");
            }

            else if (MatKhau == "")
            {
                MessageBox.Show("Không được để trống mật khẩu");
            }


            string sql = "SELECT COUNT(*) FROM DangNhap WHERE TenDN=@ten AND MatKhau=@mk";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ten", TenDN);
            cmd.Parameters.AddWithValue("@mk", MatKhau);
            int count = (int)cmd.ExecuteScalar();
            if (count > 0)
            {
                label_warnTenDN.Visible = false;
                Dashboard dashboard = new Dashboard();
                dashboard.ShowDialog();
            }
            else
            {
                label_warnTenDN.Visible = true;
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label_warnTenDN.Visible = false;
            conn = new SqlConnection(connStr);
            conn.Open();
        }
    }
}