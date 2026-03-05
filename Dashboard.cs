using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Quản_lý_bán_hàng_cafe
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }
        string connection = $"Data Source=DESKTOP-1JF9G2O;Initial Catalog=QLCF;Integrated Security=True";

        // Trang thai ban
        private void Random_Trangthaiban()
        {
            Random rd = new Random();

            foreach(Control ctr in tableLayoutPanel1.Controls)
            {
                if(ctr is Button btn)
                {
                    int trangThai = rd.Next(1, 4);

                    switch (trangThai)
                    {
                        case 1: //ban trong
                            btn.BackColor = Color.Green;
                            btn.ForeColor = Color.White;
                            break;
                        case 2: // ban phuc vu
                            btn.BackColor = Color.Yellow;
                            btn.ForeColor = Color.White;
                            break;
                        case 3:// ban chua don
                            btn.BackColor = Color.Red;
                            btn.ForeColor = Color.White;
                            break;
                    }
                }
            }
        }


        private void Data_Load()
        {
            using(SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    string query = "SELECT TenMon,TrangThai,SUM(SoLuong * Gia) AS Tong_Tien FROM MonAn" + 
                                    " GROUP BY TenMon,TrangThai";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();

                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thong bao");
                }
            }
        }
        private void Dashboard_Load(object sender, EventArgs e)
        {

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            Random_Trangthaiban();
        }



        //Hien panel chon mon
        private void btn_Themmon_Click(object sender, EventArgs e)
        {
            Them_mon.BringToFront();


        }

        //Them mon
        private void btn_Themmon1_Click(object sender, EventArgs e)
        {
            string monan = box1ChonMon.Text;
            int soLuong = (int)num1_SoLuong.Value;
            using(SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    string query = $"UPDATE MonAn SET SoLuong = @sl WHERE TenMon = @ten";
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@sl", soLuong);
                        comm.Parameters.AddWithValue("@ten", monan);

                        int result = comm.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Da them mon thanh cong");

                            Data_Load();
                        }

                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thong bao");
                }

            }
        }

       
        private void btn_Xoamon_Click(object sender, EventArgs e)
        {
            Xoa_mon.BringToFront();
        }


        //xoa mon an
        private void xoaMon_Click(object sender, EventArgs e)
        {
            string tenMon = box2ChonMon.Text;
            using(SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    string query = $"DELETE FROM MonAn WHERE TenMon = @monan";
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@monan", tenMon);

                        int result = comm.ExecuteNonQuery();

                        if(result > 0)
                        {
                            MessageBox.Show("Da xoa thanh cong");

                            Data_Load();
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thong bao");
                }
            }
        }


        
        private void btn_Guiquay_Click(object sender, EventArgs e)
        {
            GuiQuay.BringToFront();
            
        }

        //Gui quay pha che
        private void GuiQuay_Click(object sender, EventArgs e)
        {
            string tenMon = box3ChonMon.Text;

            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    string query = $"UPDATE MonAn SET TrangThai = N'Chờ pha' WHERE TenMon = @tenMon";
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@tenMon", tenMon);

                        int result = comm.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Da gui yeu cau");

                            Data_Load();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thong bao");
                }
            }
        }

        private void btn_Chuyenmon_Click(object sender, EventArgs e)
        {
            ChuyenMon.BringToFront();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string monCu = dataGridView1.CurrentRow.Cells["TenMon"].Value.ToString();

            string monMoi = box4ChonMon.Text;
            int soLuong = (int)num2_SoLuong.Value;

            using(SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE MonAn SET TenMon = @tenMoi,SoLuong = @sl WHERE TenMon = @tenCu";
                    using(SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tenMoi", monMoi);
                        cmd.Parameters.AddWithValue("@sl", soLuong);
                        cmd.Parameters.AddWithValue("@tenCu", monCu);

                        int result = cmd.ExecuteNonQuery();

                        if(result > 0)
                        {
                            MessageBox.Show("Da chuyen mon thanh cong");

                            Data_Load();
                        }

                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thong bao");
                }
            }
        }
    }
}
