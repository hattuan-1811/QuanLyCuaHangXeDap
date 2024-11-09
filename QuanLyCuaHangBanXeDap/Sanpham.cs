using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHangBanXeDap
{
    public partial class Sanpham : Form
    {
        ketnoi dal= new ketnoi();
        private string hinhAnhPath = string.Empty;

        public Sanpham()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            string query = "SELECT * FROM SanPham";
            DataTable dt = dal.ExecuteQuery(query);
            dataGridView1.DataSource = dt;
        }

        private void Sanpham_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadKhoHang();
        }
        private void LoadKhoHang()
        {
            string query = "SELECT KhoHangID, TenKho FROM KhoHang";
            DataTable dt = dal.ExecuteQuery(query);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "TenKho";
            comboBox1.ValueMember = "KhoHangID";
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["TenSanPham"].Value.ToString();
                textBox2.Text = row.Cells["LoaiSanPham"].Value.ToString();
                textBox3.Text = row.Cells["GiaBan"].Value.ToString();
                textBox4.Text = row.Cells["MauSac"].Value.ToString();
                textBox6.Text = row.Cells["KichThuoc"].Value.ToString();
                textBox5.Text = row.Cells["SoLuongTon"].Value.ToString();
                comboBox1.SelectedValue = row.Cells["KhoHangID"].Value;

                string hinhAnh = row.Cells["HinhAnh"].Value.ToString();
                pictureBox1.ImageLocation = Path.Combine(Application.StartupPath, hinhAnh);
                hinhAnhPath = string.Empty; 
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tenSanPham = textBox1.Text;
            string loaiSanPham = textBox2.Text;
            decimal giaBan;
            if (!decimal.TryParse(textBox3.Text, out giaBan))
            {
                MessageBox.Show("Giá bán không hợp lệ.");
                return;
            }
            string mauSac = textBox4.Text;
            string kichThuoc = textBox6.Text;
            int soLuongTon;
            if (!int.TryParse(textBox5.Text, out soLuongTon))
            {
                MessageBox.Show("Số lượng tồn không hợp lệ.");
                return;
            }
            int khoHangID = Convert.ToInt32(comboBox1.SelectedValue);

            if (string.IsNullOrEmpty(hinhAnhPath))
            {
                MessageBox.Show("Vui lòng chọn hình ảnh cho sản phẩm.");
                return;
            }

      
            string imagesDirectory = Path.Combine(Application.StartupPath, "Images");
            if (!Directory.Exists(imagesDirectory))
            {
                Directory.CreateDirectory(imagesDirectory);
            }
            string fileName = Path.GetFileName(hinhAnhPath);
            string destPath = Path.Combine(imagesDirectory, fileName);
            File.Copy(hinhAnhPath, destPath, true);

     
            string relativePath = Path.Combine("Images", fileName);

            string query = $"INSERT INTO SanPham (TenSanPham, LoaiSanPham, GiaBan, MauSac, KichThuoc, KhoHangID, SoLuongTon, HinhAnh) " +
                           $"VALUES (N'{tenSanPham}', N'{loaiSanPham}', {giaBan}, N'{mauSac}', N'{kichThuoc}', {khoHangID}, {soLuongTon}, N'{relativePath}')";
            dal.ExecuteNonQuery(query);
            MessageBox.Show("Thêm Sản phẩm thành công ", "Thông báo");
            LoadData();
            ClearFields();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để sửa.");
                return;
            }

            int sanPhamID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["SanPhamID"].Value);
            string tenSanPham = textBox1.Text;
            string loaiSanPham = textBox2.Text;
            decimal giaBan;
            if (!decimal.TryParse(textBox3.Text, out giaBan))
            {
                MessageBox.Show("Giá bán không hợp lệ.");
                return;
            }
            string mauSac = textBox4.Text;
            string kichThuoc = textBox6.Text;
            int soLuongTon;
            if (!int.TryParse(textBox5.Text, out soLuongTon))
            {
                MessageBox.Show("Số lượng tồn không hợp lệ.");
                return;
            }
            int khoHangID = Convert.ToInt32(comboBox1.SelectedValue);

        
            string relativePath;
            if (!string.IsNullOrEmpty(hinhAnhPath))
            {
               
                string imagesDirectory = Path.Combine(Application.StartupPath, "Images");
                if (!Directory.Exists(imagesDirectory))
                {
                    Directory.CreateDirectory(imagesDirectory);
                }
                string fileName = Path.GetFileName(hinhAnhPath);
                string destPath = Path.Combine(imagesDirectory, fileName);
                File.Copy(hinhAnhPath, destPath, true);

              
                relativePath = Path.Combine("Images", fileName);
            }
            else
            {
       
                relativePath = dataGridView1.CurrentRow.Cells["HinhAnh"].Value.ToString();
            }

            string query = $"UPDATE SanPham SET TenSanPham = N'{tenSanPham}', LoaiSanPham = N'{loaiSanPham}', GiaBan = {giaBan}, MauSac = N'{mauSac}', " +
                           $"KichThuoc = N'{kichThuoc}', KhoHangID = {khoHangID}, SoLuongTon = {soLuongTon}, HinhAnh = N'{relativePath}' " +
                           $"WHERE SanPhamID = {sanPhamID}";
            dal.ExecuteNonQuery(query);
            MessageBox.Show("Sửa Sản phẩm thành công ", "Thông báo");
            LoadData();
            ClearFields();
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                hinhAnhPath = ofd.FileName;
                pictureBox1.ImageLocation = hinhAnhPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để xóa.");
                return;
            }

            int sanPhamID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["SanPhamID"].Value);
            string hinhAnh = dataGridView1.CurrentRow.Cells["HinhAnh"].Value.ToString();

            string query = $"DELETE FROM SanPham WHERE SanPhamID = {sanPhamID}";
            dal.ExecuteNonQuery(query);

       
            string imagePath = Path.Combine(Application.StartupPath, hinhAnh);
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
            MessageBox.Show("Xóa Sản phẩm thành công ", "Thông báo");

            LoadData();
            ClearFields();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            comboBox1.SelectedIndex = 0;
            pictureBox1.ImageLocation = string.Empty;
            hinhAnhPath = string.Empty;
        }

    }
}
