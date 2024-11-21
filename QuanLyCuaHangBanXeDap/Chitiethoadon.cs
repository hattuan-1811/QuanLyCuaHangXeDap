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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyCuaHangBanXeDap
{
    public partial class Chitiethoadon : Form
    {
        public Chitiethoadon(int hoaDonID)
        {
            InitializeComponent();
            this.hoaDonID = hoaDonID;
            LoadChiTietHoaDon();
        }
        private int hoaDonID;
        private ketnoi kn = new ketnoi();

        private void Chitiethoadon_Load(object sender, EventArgs e)
        {
            
            LoadSanPhamToComboBox();
            loadlabel();
        }
        private void UpdateTongTien()
        {
            try
            {
               
                string updateQuery = @"
            UPDATE HoaDon
            SET TongTien = (
                SELECT ISNULL(SUM(cthd.SoLuong * cthd.GiaBan), 0)
                FROM ChiTietHoaDon cthd
                WHERE cthd.HoaDonID = HoaDon.HoaDonID
            );";

                kn.ExecuteNonQuery(updateQuery);

             
                if (Application.OpenForms["HoaDon"] is Form hoaDonForm)
                {
                    
                    DataGridView dgvHoaDon = hoaDonForm.Controls["dataGridView1"] as DataGridView;
                    if (dgvHoaDon != null)
                    {
                        
                        string selectQuery = @"
                    SELECT 
                        h.HoaDonID, 
                        k.HoTen AS KhachHang, 
                        nv.HoTen AS NhanVien, 
                        h.NgayBan, 
                        h.TongTien
                    FROM 
                        HoaDon h
                    JOIN 
                        KhachHang k ON h.KhachHangID = k.KhachHangID
                    JOIN 
                        NhanVien nv ON h.NhanVienID = nv.NhanVienID
                    ORDER BY 
                        h.NgayBan DESC;";

                        dgvHoaDon.DataSource = kn.ExecuteQuery(selectQuery);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật tổng tiền: {ex.Message}");
            }
        }

        private void LoadSanPhamToComboBox()
        {
            string sql = "SELECT SanPhamID, TenSanPham FROM SanPham";
            DataTable dt = kn.ExecuteQuery(sql);

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "TenSanPham";
            comboBox1.ValueMember = "SanPhamID";
        }
        private void LoadChiTietHoaDon()
        {
            try
            {
                kn.openConnect();
                string query = @"
            SELECT 
                sp.SanPhamID, 
                sp.TenSanPham, 
                cthd.SoLuong, 
                cthd.GiaBan, 
                (cthd.SoLuong * cthd.GiaBan) AS ThanhTien
            FROM 
                ChiTietHoaDon cthd
            INNER JOIN 
                SanPham sp ON cthd.SanPhamID = sp.SanPhamID
            WHERE 
                cthd.HoaDonID = @hoaDonID";

                SqlCommand command = new SqlCommand(query, kn.GetConnection());
                command.Parameters.AddWithValue("@hoaDonID", hoaDonID);

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
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

        void ClearInputs()
        {
            comboBox1.SelectedIndex = -1;
            textBox1.Clear();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                kn.openConnect();
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Vui lòng nhập số lượng!");
                    return;
                }
                int soLuong;
                if (!int.TryParse(textBox1.Text, out soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Nhập đúng số lượng!");
                    textBox1.Clear();
                    textBox1.Focus();
                    return;
                }

                int sanPhamID = Convert.ToInt32(comboBox1.SelectedValue);

                // Kiểm tra số lượng tồn trước khi thêm
                string checkQuery = $"SELECT SoLuongTon FROM SanPham WHERE SanPhamID = {sanPhamID}";
                int soLuongTon = Convert.ToInt32(kn.ExecuteQuery(checkQuery).Rows[0][0]);

                if (soLuong > soLuongTon)
                {
                    MessageBox.Show($"Số lượng trong kho không đủ! Hiện chỉ còn {soLuongTon} sản phẩm.");
                    return;
                }

                // Lấy giá bán từ bảng SanPham
                string giaQuery = $"SELECT GiaBan FROM SanPham WHERE SanPhamID = {sanPhamID}";
                decimal giaBan = Convert.ToDecimal(kn.ExecuteQuery(giaQuery).Rows[0][0]);

                string insertQuery = $@"
            INSERT INTO ChiTietHoaDon (HoaDonID, SanPhamID, SoLuong, GiaBan)
            VALUES ({hoaDonID}, {sanPhamID}, {soLuong}, {giaBan})";

                kn.ExecuteNonQuery(insertQuery);
                LoadChiTietHoaDon();
                UpdateTongTien();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
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
                if (dataGridView1.SelectedCells == null)
                {
                    MessageBox.Show("Vui lòng chọn dòng cần sửa!");
                    return;
                }

                int soLuong;
                if (!int.TryParse(textBox1.Text, out soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Nhập đúng số lượng!");
                    textBox1.Clear();
                    textBox1.Focus();
                    return;
                }

                // Lấy thông tin sản phẩm cũ
                int sanPhamIDCu = Convert.ToInt32(dataGridView1.CurrentRow.Cells["SanPhamID"].Value);

               
                int sanPhamIDMoi = Convert.ToInt32(comboBox1.SelectedValue);

                string checkQuery = $"SELECT SoLuongTon FROM SanPham WHERE SanPhamID = {sanPhamIDMoi}";
                int soLuongTon = Convert.ToInt32(kn.ExecuteQuery(checkQuery).Rows[0][0]);

                if (soLuong > soLuongTon)
                {
                    MessageBox.Show($"Số lượng trong kho không đủ! Hiện chỉ còn {soLuongTon} sản phẩm.");
                    return;
                }

                // Lấy giá bán mới từ bảng SanPham
                string giaQuery = $"SELECT GiaBan FROM SanPham WHERE SanPhamID = {sanPhamIDMoi}";
                decimal giaBan = Convert.ToDecimal(kn.ExecuteQuery(giaQuery).Rows[0][0]);

                // Cập nhật chi tiết hóa đơn với sản phẩm mới và số lượng mới
                string updateQuery = $@"
            UPDATE ChiTietHoaDon 
            SET SanPhamID = {sanPhamIDMoi},
                SoLuong = {soLuong},
                GiaBan = {giaBan}
            WHERE HoaDonID = {hoaDonID} AND SanPhamID = {sanPhamIDCu}";

                kn.ExecuteNonQuery(updateQuery);
                LoadChiTietHoaDon();
                UpdateTongTien();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
            finally
            {
                kn.closeConnect();
            }

        }
        void loadlabel()
        {
            try
            {
                string query = $@"
            SELECT k.HoTen 
            FROM HoaDon h
            INNER JOIN KhachHang k ON h.KhachHangID = k.KhachHangID
            WHERE h.HoaDonID = {hoaDonID}";

                DataTable dt = kn.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    label1.Text = " Khách Hàng: " + dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải tên khách hàng: {ex.Message}");
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.RowIndex >= 0)
                {

                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    comboBox1.SelectedValue = row.Cells["SanPhamID"].Value;

                    textBox1.Text = row.Cells["SoLuong"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                kn.openConnect();

                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn dòng cần xóa!");
                    return;
                }

                int sanPhamID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["SanPhamID"].Value);

                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string deleteQuery = $@"
                DELETE FROM ChiTietHoaDon 
                WHERE HoaDonID = {hoaDonID} AND SanPhamID = {sanPhamID}";

                    kn.ExecuteNonQuery(deleteQuery);
                    LoadChiTietHoaDon();
                    UpdateTongTien();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
            finally
            {
                kn.closeConnect();
            }
         }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }            
            
    
    
}
