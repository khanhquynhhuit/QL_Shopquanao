using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Form_QL_ShopQuanAo
{
    public partial class dangnhap : Form
    {
        // Biến toàn cục để lưu ConnectionString
        public static string GlobalConnectionString { get; set; }
        public static string CurrentUsername { get; set; }

        public dangnhap()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, EventArgs e)
        {
            string username = txt_taikhoan.Text.Trim();
            string password = txt_matkhau.Text.Trim();
            string server = "TONY\\SQLEXPRESS";

            // Kiểm tra dữ liệu nhập
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("⚠️ Vui lòng nhập tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_taikhoan.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("⚠️ Vui lòng nhập mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_matkhau.Focus();
                return;
            }

            // Chuỗi kết nối với tài khoản người dùng
            string connStrUser = $"Server={server};Database=QLShopQuanAo;User Id={username};Password={password};";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStrUser))
                {
                    conn.Open(); // Thử kết nối - nếu thành công thì tài khoản/mật khẩu đúng

                    // QUAN TRỌNG: LƯU LẠI CONNECTION STRING ĐỂ DÙNG Ở FORM QUẢN LÝ
                    GlobalConnectionString = connStrUser;
                    CurrentUsername = username;

                    // Hiển thị thông báo thành công
                    MessageBox.Show($"✅ Đăng nhập thành công!\nChào mừng {username}",
                                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở form Quản lý - TRUYỀN ConnectionString qua constructor
                    QuanLy mainForm = new QuanLy(connStrUser, username);
                    mainForm.Show();

                    this.Hide(); // Ẩn form đăng nhập
                }
            }
            catch (SqlException ex)
            {
                // Xử lý lỗi đăng nhập
                if (ex.Number == 18456) // Lỗi đăng nhập SQL Server
                {
                    MessageBox.Show("❌ Sai tên đăng nhập hoặc mật khẩu!",
                                  "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Xóa mật khẩu và focus lại
                    txt_matkhau.Text = "";
                    txt_matkhau.Focus();
                }
                else if (ex.Number == 4060) // Lỗi không thể kết nối database
                {
                    MessageBox.Show("❌ Không thể kết nối đến database!",
                                  "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"❌ Lỗi: {ex.Message}",
                                  "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Thoat_Click(object sender, EventArgs e)
        {
            // Quay về form Welcome
            FormWelcome welcomeForm = new FormWelcome();
            welcomeForm.Show();
            this.Hide(); // Ẩn form đăng nhập
        }

        private void txt_matkhau_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Nhấn Enter để đăng nhập
            if (e.KeyChar == (char)Keys.Enter)
            {
                Login_Click(sender, e);
            }
        }

        private void txt_taikhoan_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Nhấn Enter từ textbox tài khoản sẽ chuyển sang textbox mật khẩu
            if (e.KeyChar == (char)Keys.Enter)
            {
                txt_matkhau.Focus();
            }
        }

        private void dangnhap_Load(object sender, EventArgs e)
        {
            // Focus vào textbox tài khoản khi load form
            txt_taikhoan.Focus();
        }

        private void dangnhap_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Khi đóng form đăng nhập, quay về form welcome
            if (e.CloseReason == CloseReason.UserClosing)
            {
                FormWelcome welcomeForm = new FormWelcome();
                welcomeForm.Show();
            }
        }
    }
}