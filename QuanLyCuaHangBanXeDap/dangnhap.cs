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

namespace QuanLyCuaHangBanXeDap
{
    public partial class dangnhap : Form
    {
        public dangnhap()
        {
            InitializeComponent();
        }
        private ketnoi db = new ketnoi();
        private void linkLabel2_DangKy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dangky f = new dangky();
            f.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tentk = txt_tentaikhoan.Text.Trim();
            string matkhau = txt_matkhau.Text.Trim();

          
            if (string.IsNullOrEmpty(tentk))
            {
                MessageBox.Show("Vui lòng nhập tên tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(matkhau))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

           
            string query = "SELECT COUNT(*) FROM TaiKhoan WHERE TenTaiKhoan = @TenTaiKhoan AND MatKhau = @MatKhau";

            try
            {
                using (SqlConnection conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenTaiKhoan", tentk);
                        cmd.Parameters.AddWithValue("@MatKhau", matkhau);

                        conn.Open();
                        int userCount = (int)cmd.ExecuteScalar();
                        conn.Close();

                        if (userCount > 0)
                        {
                            MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Chuyển đến form chính
                            this.Hide();
                            dangky frm_Main = new dangky();
                            frm_Main.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Tên tài khoản hoặc mật khẩu không chính xác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dangnhap_Load(object sender, EventArgs e)
        {

        }
    }
}
