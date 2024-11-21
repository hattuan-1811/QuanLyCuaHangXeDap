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
    public partial class HoaDon : Form
    {
        public HoaDon()
        {
            InitializeComponent();
        }
        private ketnoi kn = new ketnoi();
        private void LoadData()
        {
           
            try
            {
                kn.openConnect();
                                string query = @"
                              SELECT 
                    h.HoaDonID, 
                    k.HoTen AS KhachHang, 
                    nv.HoTen AS NhanVien, 
                    h.NgayBan, 
                    ISNULL(SUM(cthd.SoLuong * cthd.GiaBan), 0) AS TongTien
                FROM 
                    HoaDon h
                JOIN 
                    KhachHang k ON h.KhachHangID = k.KhachHangID
                JOIN 
                    NhanVien nv ON h.NhanVienID = nv.NhanVienID
                LEFT JOIN 
                    ChiTietHoaDon cthd ON h.HoaDonID = cthd.HoaDonID
                GROUP BY 
                    h.HoaDonID, k.HoTen, nv.HoTen, h.NgayBan
                ORDER BY 
                    h.NgayBan DESC;";

                dataGridView1.DataSource = kn.ExecuteQuery(query);


              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                kn.closeConnect();
            }

        }
        private void AddButtonColumns()
        {
            // Thêm cột "Chi tiết"
            DataGridViewButtonColumn btnChiTiet = new DataGridViewButtonColumn();
            btnChiTiet.Name = "ChiTiet";
            btnChiTiet.HeaderText = "Chi tiết";
            btnChiTiet.Text = "Chi tiết";
            btnChiTiet.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(btnChiTiet);

          
        }
        private void HoaDon_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            LoadData();
            loadcomboboxkhachhang();
            loadcomboboxnhanvien();
            AddButtonColumns();
           
        }
      
       
        void loadcomboboxkhachhang()
        {
            string query = "SELECT KhachHangID, HoTen FROM KhachHang";
            DataTable khachHangTable = kn.ExecuteQuery(query);
            comboBox1.DataSource = khachHangTable;
            comboBox1.DisplayMember = "HoTen"; 
            comboBox1.ValueMember = "KhachHangID";
        }
        void loadcomboboxnhanvien()
        {
            string query = "SELECT NhanVienID, HoTen FROM NhanVien";
            DataTable nhanVienTable = kn.ExecuteQuery(query);
            comboBox2.DataSource = nhanVienTable;
            comboBox2.DisplayMember = "HoTen"; 
            comboBox2.ValueMember = "NhanVienID";
        }
     
        

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
                int hoaDonID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["HoaDonID"].Value);

                if (columnName == "ChiTiet")
                {
                    // Xử lý khi nhấn nút "Chi tiết"
                    Chitiethoadon chiTietForm = new Chitiethoadon(hoaDonID);
                    chiTietForm.ShowDialog();
                }
              
            }
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

               
                string khachHang = row.Cells["KhachHang"].Value.ToString();
                comboBox1.Text = khachHang;

                string nhanVien = row.Cells["NhanVien"].Value.ToString();
                comboBox2.Text = nhanVien;

            
                if (DateTime.TryParse(row.Cells["NgayBan"].Value.ToString(), out DateTime ngayBan))
                {
                    dateTimePicker1.Value = ngayBan;
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void ClearInputs()
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                kn.openConnect();
                int khachHangID = Convert.ToInt32(comboBox1.SelectedValue);
                int nhanVienID = Convert.ToInt32(comboBox2.SelectedValue);
                string ngayBan = dateTimePicker1.Value.ToString("yyyy-MM-dd");

                string query = $@"INSERT INTO HoaDon (KhachHangID, NhanVienID, NgayBan, TongTien)
                      VALUES ({khachHangID}, {nhanVienID}, '{ngayBan}', 0)";

                kn.ExecuteNonQuery(query);
                MessageBox.Show("Thêm hóa đơn thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm hóa đơn: {ex.Message}", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                kn.closeConnect();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                kn.openConnect();
                if (dataGridView1.SelectedCells.Count <=0)
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn cần sửa!", "Thông báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int hoaDonID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["HoaDonID"].Value);
                int khachHangID = Convert.ToInt32(comboBox1.SelectedValue);
                int nhanVienID = Convert.ToInt32(comboBox2.SelectedValue);
                string ngayBan = dateTimePicker1.Value.ToString("yyyy-MM-dd");

                string query = $@"UPDATE HoaDon 
                            SET KhachHangID = {khachHangID},
                                NhanVienID = {nhanVienID},
                                NgayBan = '{ngayBan}'
                            WHERE HoaDonID = {hoaDonID}";

                kn.ExecuteNonQuery(query);
                MessageBox.Show("Cập nhật hóa đơn thành công!", "Thông báo",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật hóa đơn: {ex.Message}", "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                kn.closeConnect();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                kn.openConnect();
                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn cần xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int hoaDonID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["HoaDonID"].Value);

                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa hóa đơn này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {

                    string deleteChiTietQuery = $"DELETE FROM ChiTietHoaDon WHERE HoaDonID = {hoaDonID}";
                    kn.ExecuteNonQuery(deleteChiTietQuery);


                    string deleteHoaDonQuery = $"DELETE FROM HoaDon WHERE HoaDonID = {hoaDonID}";
                    kn.ExecuteNonQuery(deleteHoaDonQuery);

                    MessageBox.Show("Xóa hóa đơn  thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadData();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa hóa đơn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                kn.closeConnect();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Lấy khoảng thời gian từ 2 DateTimePicker
            DateTime startDate = dateTimePicker2.Value.Date;
            DateTime endDate = dateTimePicker3.Value.Date;

            // Kiểm tra ngày bắt đầu và ngày kết thúc
            if (startDate > endDate)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Câu truy vấn sử dụng tham số
                string query = @"
            SELECT 
                h.HoaDonID, 
                k.HoTen AS KhachHang, 
                nv.HoTen AS NhanVien, 
                h.NgayBan, 
                ISNULL(SUM(cthd.SoLuong * cthd.GiaBan), 0) AS TongTien
            FROM 
                HoaDon h
            JOIN 
                KhachHang k ON h.KhachHangID = k.KhachHangID
            JOIN 
                NhanVien nv ON h.NhanVienID = nv.NhanVienID
            LEFT JOIN 
                ChiTietHoaDon cthd ON h.HoaDonID = cthd.HoaDonID
            WHERE 
                h.NgayBan BETWEEN @StartDate AND @EndDate
            GROUP BY 
                h.HoaDonID, k.HoTen, nv.HoTen, h.NgayBan
            ORDER BY 
                h.NgayBan DESC";

                // Sử dụng lớp ketnoi để thực hiện truy vấn
                using (SqlConnection conn = kn.GetConnection())
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

                    // Thêm tham số
                    adapter.SelectCommand.Parameters.AddWithValue("@StartDate", startDate);
                    adapter.SelectCommand.Parameters.AddWithValue("@EndDate", endDate);

                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Gắn dữ liệu vào DataGridView
                    dataGridView1.DataSource = dataTable;

                    // Kiểm tra nếu không có kết quả
                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy hóa đơn nào trong khoảng thời gian này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
