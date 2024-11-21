using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyCuaHangBanXeDap
{
    public partial class frmTimKiem : Form
    {
        private ketnoi db = new ketnoi();

        public frmTimKiem()
        {
            InitializeComponent();
        }

      

      

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!");
                return;
            }

            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn bảng cần tìm kiếm!");
                return;
            }

            if (!radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("Vui lòng chọn loại tìm kiếm (ID hoặc Tên)!");
                return;
            }

            string selectedTable = comboBox1.SelectedItem.ToString().Trim();
            string searchText = textBox1.Text.Trim();
            string query = "";

            // Xác định tìm theo ID hay Tên
            if (radioButton1.Checked) // Tìm theo ID
            {
                switch (selectedTable)
                {
                    case "Sản phẩm":
                        query = "SELECT * FROM SanPham WHERE SanPhamID = @SearchText";
                        break;
                    case "Nhân viên":
                        query = "SELECT * FROM NhanVien WHERE NhanVienID = @SearchText";
                        break;
                    case "Khách hàng":
                        query = "SELECT * FROM KhachHang WHERE KhachHangID = @SearchText";
                        break;
                    case "Kho hàng":
                        query = "SELECT * FROM KhoHang WHERE KhoHangID = @SearchText";
                        break;
                    default:
                        MessageBox.Show($"Không thể tìm bảng: {selectedTable}");
                        return;
                }
            }
            else // Tìm theo Tên
            {
                switch (selectedTable)
                {
                    case "Sản phẩm":
                        query = "SELECT * FROM SanPham WHERE TenSanPham LIKE @SearchText";
                        break;
                    case "Nhân viên":
                        query = "SELECT * FROM NhanVien WHERE HoTen LIKE @SearchText";
                        break;
                    case "Khách hàng":
                        query = "SELECT * FROM KhachHang WHERE HoTen LIKE @SearchText";
                        break;
                    case "Kho hàng":
                        query = "SELECT * FROM KhoHang WHERE TenKho LIKE @SearchText";
                        break;
                    default:
                        MessageBox.Show($"Không thể tìm bảng: {selectedTable}");
                        return;
                }
            }

            try
            {
                DataTable dt;
                using (SqlConnection conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (radioButton2.Checked)
                        {
                            cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@SearchText", searchText);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        dt = new DataTable();
                        adapter.Fill(dt);
                    }
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy kết quả nào!");
                }

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
            }
        }
    }
}
