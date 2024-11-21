using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHangBanXeDap
{
    public partial class main : Form
    {
        private Button selectedButton;
        private Form currentChildForm;
        public main()
        {
            InitializeComponent();
        }
        private void openChildForm(Form ChildForm)
        {
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }
            currentChildForm = ChildForm;
            ChildForm.TopLevel = false;
            ChildForm.FormBorderStyle = FormBorderStyle.None;
            ChildForm.Dock = DockStyle.Fill;
            panelMain.Controls.Add(ChildForm);
            panelMain.Tag = ChildForm;
            ChildForm.BringToFront();
            ChildForm.Show();
        }

        private void main_Load(object sender, EventArgs e)
        {
       
        }

        private void btnnhanvien_Click(object sender, EventArgs e)
        {
            if (selectedButton != null)
            {
                selectedButton.BackColor = Color.Beige;
            }

            Button clickedButton = (Button)sender;
            clickedButton.BackColor = Color.BurlyWood;
            selectedButton = clickedButton;
            panelMain.AutoScroll = false;
            openChildForm(new frmNhanVien());
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            if (selectedButton != null)
            {
                selectedButton.BackColor = Color.Beige;
            }

            Button clickedButton = (Button)sender;
            clickedButton.BackColor = Color.BurlyWood;
            selectedButton = clickedButton;
            panelMain.AutoScroll = false;
            openChildForm(new Sanpham());
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            if (selectedButton != null)
            {
                selectedButton.BackColor = Color.Beige;
            }

            Button clickedButton = (Button)sender;
            clickedButton.BackColor = Color.BurlyWood;
            selectedButton = clickedButton;
            panelMain.AutoScroll = false;
            openChildForm(new frmKhachHang());
        }

        private void btnKhoHang_Click(object sender, EventArgs e)
        {
            if (selectedButton != null)
            {
                selectedButton.BackColor = Color.Beige;
            }

            Button clickedButton = (Button)sender;
            clickedButton.BackColor = Color.BurlyWood;
            selectedButton = clickedButton;
            panelMain.AutoScroll = false;
            openChildForm(new KhoHang());
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            if (selectedButton != null)
            {
                selectedButton.BackColor = Color.Beige;
            }

            Button clickedButton = (Button)sender;
            clickedButton.BackColor = Color.BurlyWood;
            selectedButton = clickedButton;
            panelMain.AutoScroll = false;
            openChildForm(new HoaDon());
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (selectedButton != null)
            {
                selectedButton.BackColor = Color.Beige;
            }

            Button clickedButton = (Button)sender;
            clickedButton.BackColor = Color.BurlyWood;
            selectedButton = clickedButton;
            panelMain.AutoScroll = false;
            openChildForm(new frmTimKiem());
        }


        private void tìmNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (selectedButton != null)
            {
                selectedButton.BackColor = Color.Beige;
            }
            if (currentChildForm != null)
            {
                currentChildForm.Close();
                panelMain.AutoScroll = true;
            }
        }
    }
}
