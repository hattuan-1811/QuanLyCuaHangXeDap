using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QuanLyCuaHangBanXeDap
{
    public partial class dangky : Form
    {
        public dangky()
        {
            InitializeComponent();
        }
        private ketnoi db = new ketnoi();
        private void btn_DangKy_Click(object sender, EventArgs e)
        {
            string tentk = txt_TenTaiKhoan.Text;
            string matkhau = txt_MatKhau.Text;
            string xnmatkhau = txt_xacnhanmatkhau.Text;
            string email = txt_email.Text;
            if (matkhau != xnmatkhau)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp, vui lòng kiểm tra lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!CheckEmail(email)) { MessageBox.Show(" Vui lòng nhập đúng định dạng email"); return; }
            string query = "INSERT INTO TaiKhoan (TenTaiKhoan, MatKhau, Email) VALUES (@TenTaiKhoan, @MatKhau, @Email)";
            try
            {
                using (SqlConnection conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenTaiKhoan", tentk);
                        cmd.Parameters.AddWithValue("@MatKhau", matkhau);
                        cmd.Parameters.AddWithValue("@Email", email);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }

                if (MessageBox.Show("Đăng ký thành công! Bạn có muốn đăng nhập luôn không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    this.Close();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    MessageBox.Show("Tên tài khoản này đã được đăng ký, vui lòng chọn tên khác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
    }
        public bool CheckEmail(string em)
        {
            return Regex.IsMatch(em, "^[a-zA-Z0-9_.]{3,20}@gmail.com(.vn|)$");

        }
        private void dangky_Load(object sender, EventArgs e)
        {

        }
    }
}
