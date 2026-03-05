using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Quản_lý_bán_hàng_cafe
{
    public partial class Form1 : Form
    {
        string connStr = @"Data Source=DESKTOP-1JF9G2O;Initial Catalog=QuanLiBanCafe;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
        }

        // Hàm mã hóa SHA256
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

        // Nút đăng nhập
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string TenDN = guna2TextBox_TenDN.Text.Trim();
            string MatKhauText = guna2TextBox_MatKhau.Text.Trim();

            label_warnTenDN.Visible = false;

            // Kiểm tra input
            if (TenDN == "")
            {
                MessageBox.Show("Không được để trống tên đăng nhập");
                return;
            }

            if (MatKhauText == "")
            {
                MessageBox.Show("Không được để trống mật khẩu");
                return;
            }

            // Mã hóa mật khẩu
            string MatKhau = MaHoaSHA256(MatKhauText);

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql = "SELECT COUNT(*) FROM DangNhap WHERE TenDN=@ten AND MatKhau=@mk";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ten", TenDN);
                        cmd.Parameters.AddWithValue("@mk", MatKhau);

                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            Dashboard dashboard = new Dashboard();
                            this.Hide();
                            dashboard.ShowDialog();
                            this.Show();
                        }
                        else
                        {
                            label_warnTenDN.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu:\n" + ex.Message);
            }

            Dashboard db = new Dashboard();
            db.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label_warnTenDN.Visible = false;
        }
    }
}
