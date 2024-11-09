using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyCuaHangBanXeDap
{
    public partial class KhoHang : Form
    {
        public KhoHang()
        {
            InitializeComponent();
        }
        ketnoi dal = new ketnoi();
        private int selectedKhoHangId;

        private void LoadData()
        {
            string query = "SELECT * FROM KhoHang";
            DataTable dt = dal.ExecuteQuery(query);
            dataGridView1.DataSource = dt;
        }

        private void KhoHang_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();

        }
    private void button1_Click(object sender, EventArgs e)
        {
            string tenkhohang = textBox1.Text;
            string diachi = textBox2.Text;

            if (string.IsNullOrEmpty(tenkhohang) || string.IsNullOrEmpty(diachi))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ.");
                return;
            }
            string query = $"INSERT INTO KhoHang (TenKho, DiaChi) " +
                          $"VALUES (N'{tenkhohang}', N'{diachi}')";
            dal.ExecuteNonQuery(query);
            MessageBox.Show("Thêm Sản phẩm thành công ", "Thông báo");
            LoadData();
            ClearFields();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            string tenkhohang = textBox1.Text;
            string diachi = textBox2.Text;
            int khohangID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["KhoHangID"].Value);
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để sửa.");
                return;
            }
            string query = $"UPDATE KhoHang SET TenKho = N'{tenkhohang}', DiaChi = N'{diachi}'"+
                           $"WHERE KhoHangID = {khohangID}";
            dal.ExecuteNonQuery(query);
            MessageBox.Show("Sửa Sản phẩm thành công ", "Thông báo");
            LoadData();
            ClearFields();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["TenKho"].Value.ToString();
                textBox2.Text = row.Cells["DiaChi"].Value.ToString();

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count==0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để xóa.");
                return;
            }

            if (!int.TryParse(dataGridView1.CurrentRow.Cells["KhoHangID"].Value.ToString(), out int khoHangID))
            {
                MessageBox.Show("ID sản phẩm không hợp lệ.", "Lỗi");
                return;
            }

            // Xây dựng câu truy vấn xóa với tham số `KhoHangID`
            string query = $"DELETE FROM KhoHang WHERE KhoHangID = {khoHangID}";

            try
            {
                dal.ExecuteNonQuery(query);
                MessageBox.Show("Xóa sản phẩm thành công", "Thông báo");
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi");
            }


        }
    }
  
}
