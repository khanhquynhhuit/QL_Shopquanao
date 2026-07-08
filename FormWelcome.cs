using System;
using System.Windows.Forms;

namespace Form_QL_ShopQuanAo
{
    public partial class FormWelcome : Form
    {
        public FormWelcome()
        {
            InitializeComponent();
        }

        private void FormWelcome_Load(object sender, EventArgs e)
        {
            // Code khi form load (nếu cần)
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            // Mở form đăng nhập và ẩn form welcome
            dangnhap loginForm = new dangnhap();
            loginForm.Show();
            this.Hide(); // Ẩn form welcome
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            // Mở form đăng ký và ẩn form welcome
            Dangky registerForm = new Dangky();
            registerForm.Show();
            this.Hide(); // Ẩn form welcome
        }

        // Xử lý sự kiện khi form đóng
        private void FormWelcome_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Hiển thị hộp thoại xác nhận khi đóng form
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát ứng dụng?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true; // Hủy đóng form
            }
            else
            {
                Application.Exit(); // Thoát ứng dụng
            }
        }
    }
}