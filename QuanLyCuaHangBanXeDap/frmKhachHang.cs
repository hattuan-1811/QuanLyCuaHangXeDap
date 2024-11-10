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
    public partial class frmKhachHang : Form
    {
        private int selectedKhachHangId;
        SqlConnection conn;
        private string connectSQL = "Data Source=T4DEPTRAI\\SQLEXPRESS;Initial Catalog=QuanLyCuaHangXeDap;Integrated Security=True;TrustServerCertificate=True";
        private void openConnect()
        {
            conn = new SqlConnection(connectSQL);
            conn.Open();
        }
        private void closeConnect()
        {
            conn.Close();
        }
        public frmKhachHang()
        {
            InitializeComponent();
        }

        private void frmKhachHang_Load(object sender, EventArgs e)
        {
            openConnect();
            LoadData();
        }
        private void LoadData()
        {
            string query = "SELECT * FROM KhachHang";
            DataTable dt = new DataTable();

            try
            {
                openConnect();
                using (SqlCommand command = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns["KhachHangID"].HeaderText = "Mã khách hàng";
                    dataGridView1.Columns["HoTen"].HeaderText = "Họ Tên";
                    dataGridView1.Columns["SoDienThoai"].HeaderText = "Số Điện Thoại";
                    dataGridView1.Columns["DiaChi"].HeaderText = "Địa Chỉ";
                    dataGridView1.Columns["Email"].HeaderText = "Email";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                closeConnect();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //thêm
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Họ tên không được để trống!");
                return;
            }
            if (!int.TryParse(textBox2.Text, out _))
            {
                MessageBox.Show("Số điện thoại phải là số!");
                return;
            }
            if (!textBox4.Text.Contains("@"))
            {
                MessageBox.Show("Email không hợp lệ.");
                return;
            }

            string query = "INSERT INTO KhachHang (HoTen, SoDienThoai, DiaChi, Email) " +
                           "VALUES (@HoTen, @SoDienThoai, @DiaChi, @Email)";
            try
            {
                openConnect();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@HoTen", textBox1.Text);
                    command.Parameters.AddWithValue("@SoDienThoai", textBox2.Text);
                    command.Parameters.AddWithValue("@DiaChi", textBox3.Text);
                    command.Parameters.AddWithValue("@Email", textBox4.Text);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm khách hàng thành công!");
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Thêm khách hàng thất bại!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                closeConnect();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //sửa
            if (selectedKhachHangId <= 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để sửa.");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Tên không được để trống!");
                return;
            }
            if (!int.TryParse(textBox2.Text, out _))
            {
                MessageBox.Show("Số điện thoại phải là số!");
                return;
            }
            if (!textBox4.Text.Contains("@"))
            {
                MessageBox.Show("Email không hợp lệ.");
                return;
            }
            string query = "UPDATE KhachHang SET HoTen = @HoTen, SoDienThoai = @SoDienThoai, DiaChi = @DiaChi, Email = @Email " +
                           "WHERE KhachHangID = @id";

            try
            {
                openConnect();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@id", selectedKhachHangId);
                    command.Parameters.AddWithValue("@HoTen", textBox1.Text);
                    command.Parameters.AddWithValue("@SoDienThoai", textBox2.Text);
                    command.Parameters.AddWithValue("@DiaChi", textBox3.Text);
                    command.Parameters.AddWithValue("@Email", textBox4.Text);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Sửa Khách hàng thành công!");
                        selectedKhachHangId = -1;
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Sửa Khách hàng thất bại!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                closeConnect();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //xoá
            if (selectedKhachHangId <= 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để xóa.");
                return;
            }
            string query = "DELETE FROM KhachHang WHERE KhachHangID = @id";

            try
            {
                openConnect();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@id", selectedKhachHangId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xoá khách hàng thành công!");
                        selectedKhachHangId = -1;
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Xoá khách hàng thất bại!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                closeConnect();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //thoát
            this.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedKhachHangId = Convert.ToInt32(row.Cells["KhachHangID"].Value);
                textBox1.Text = row.Cells["HoTen"].Value.ToString();
                textBox2.Text = row.Cells["SoDienThoai"].Value.ToString();
                textBox3.Text = row.Cells["DiaChi"].Value.ToString();
                textBox4.Text = row.Cells["Email"].Value.ToString();
            }
        }
    }
}
