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
    public partial class frmNhanVien : Form
    {
        private int selectedNhanVienId;
        SqlConnection conn;
        private string connectSQL = "Data Source=LEVANQUYEN\\SQLEXPRESS;Initial Catalog=QuanLyCuaHangXeDap;Integrated Security=True;";
        private void openConnect()
        {
            conn = new SqlConnection(connectSQL);
            conn.Open();
        }
        private void closeConnect()
        {
            conn.Close();
        }
        public frmNhanVien()
        {
            InitializeComponent();
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            openConnect();
            LoadData();
        }
        void LoadData()
        {
            string query = "SELECT * FROM NhanVien";
            DataTable dt = new DataTable();

            try
            {
                openConnect();
                using (SqlCommand command = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
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
            string query = "INSERT INTO NhanVien (HoTen, GioiTinh, NgaySinh, SoDienThoai, DiaChi, Email, ChucVu, HinhAnh) " +
                           "VALUES (@HoTen, @GioiTinh, @NgaySinh, @SoDienThoai, @DiaChi, @Email, @ChucVu, @HinhAnh)";
            try
            {
                openConnect();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@HoTen", textBox1.Text);
                    command.Parameters.AddWithValue("@GioiTinh", comboBox1.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@NgaySinh", dateTimePicker1.Value);
                    command.Parameters.AddWithValue("@SoDienThoai", textBox2.Text);
                    command.Parameters.AddWithValue("@DiaChi", textBox3.Text);
                    command.Parameters.AddWithValue("@Email", textBox4.Text);
                    command.Parameters.AddWithValue("@ChucVu", textBox5.Text);
                    command.Parameters.AddWithValue("@HinhAnh", pictureBox1.ImageLocation);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm nhân viên thành công!");
                        LoadData(); // Tải lại dữ liệu
                    }
                    else
                    {
                        MessageBox.Show("Thêm nhân viên thất bại!");
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
            // sửa
            if (selectedNhanVienId <= 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên để sửa.");
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
            string query = "UPDATE NhanVien SET HoTen = @HoTen, GioiTinh = @GioiTinh, NgaySinh = @NgaySinh, " +
                           "SoDienThoai = @SoDienThoai, DiaChi = @DiaChi, Email = @Email, ChucVu = @ChucVu, HinhAnh = @HinhAnh " +
                           "WHERE NhanVienID = @id";

            try
            {
                openConnect();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@id", selectedNhanVienId);
                    command.Parameters.AddWithValue("@HoTen", textBox1.Text);
                    command.Parameters.AddWithValue("@GioiTinh", comboBox1.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@NgaySinh", dateTimePicker1.Value);
                    command.Parameters.AddWithValue("@SoDienThoai", textBox2.Text);
                    command.Parameters.AddWithValue("@DiaChi", textBox3.Text);
                    command.Parameters.AddWithValue("@Email", textBox4.Text);
                    command.Parameters.AddWithValue("@ChucVu", textBox5.Text);
                    command.Parameters.AddWithValue("@HinhAnh", pictureBox1.ImageLocation);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Sửa nhân viên thành công!");
                        selectedNhanVienId = -1;
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Sửa nhân viên thất bại!");
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
            if (selectedNhanVienId <= 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên để xóa.");
                return;
            }
            string query = "DELETE FROM NhanVien WHERE NhanVienID = @id";

            try
            {
                openConnect();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@id", selectedNhanVienId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xoá nhân viên thành công!");
                        ClearFields();
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Xoá nhân viên thất bại!");
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
            this.Close();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Hiển thị ảnh trong PictureBox
                pictureBox1.ImageLocation = openFileDialog.FileName;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                selectedNhanVienId = Convert.ToInt32(row.Cells["NhanVienID"].Value);
                textBox1.Text = row.Cells["HoTen"].Value.ToString();
                comboBox1.SelectedItem = row.Cells["GioiTinh"].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                textBox2.Text = row.Cells["SoDienThoai"].Value.ToString();
                textBox3.Text = row.Cells["DiaChi"].Value.ToString();
                textBox4.Text = row.Cells["Email"].Value.ToString();
                textBox5.Text = row.Cells["ChucVu"].Value.ToString();
                string imagePath = row.Cells["HinhAnh"].Value.ToString();
                pictureBox1.ImageLocation = imagePath;
            }
        }
        private void ClearFields()
        {
            textBox1.Clear();
            comboBox1.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            pictureBox1.Image = null;
            selectedNhanVienId = -1;
        }
    }
}
