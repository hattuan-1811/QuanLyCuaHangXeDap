using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;


namespace QuanLyCuaHangBanXeDap
{
    public partial class thongke : Form
    {
        private ketnoi kn = new ketnoi();
        public thongke()
        {
            InitializeComponent();
        }

        private void thongke_Load(object sender, EventArgs e)
        {
            
         

        }

      
       

private void ExportToExcel(DataTable dt, string tableName)
    {
      
    }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn mục trong danh sách!");
                return;
            }

            string selectedItem = comboBox1.SelectedItem.ToString();
            string query = "";

            switch (selectedItem)
            {
                case "Nhân viên":
                    query = "SELECT NhanVienID, HoTen, GioiTinh, NgaySinh, SoDienThoai, DiaChi, Email, ChucVu FROM NhanVien";
                    break;
                case "Kho hàng":
                    query = "SELECT KhoHangID, TenKho, DiaChi FROM KhoHang";
                    break;
                case "Sản phẩm":
                    query = "SELECT SanPhamID, TenSanPham, LoaiSanPham, GiaBan, MauSac, KichThuoc, SoLuongTon FROM SanPham";
                    break;
                case "Khách hàng":
                    query = "SELECT KhachHangID, HoTen, SoDienThoai, DiaChi, Email FROM KhachHang";
                    break;
                case "Hoá đơn":
                    query = "SELECT HoaDonID, KhachHangID, NhanVienID, NgayBan, TongTien FROM HoaDon";
                    break;
                default:
                    MessageBox.Show("Vui lòng chọn mục hợp lệ!");
                    return;
            }

            try
            {
                DataTable data = kn.ExecuteQuery(query);
                dataGridView1.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                excelApp = new Excel.Application();
                workbook = excelApp.Workbooks.Add();
                worksheet = workbook.Sheets[1];
                worksheet.Name = "Báo cáo";

                // Xuất tiêu đề cột
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                }

                // Xuất dữ liệu hàng
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] =
                            dataGridView1.Rows[i].Cells[j].Value?.ToString() ?? string.Empty;
                    }
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = $"BaoCao_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Xuất dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Giải phóng tài nguyên Excel
                if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                if (workbook != null)
                {
                    workbook.Close();
                    Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    Marshal.ReleaseComObject(excelApp);
                }
            }
        }
    }
}
