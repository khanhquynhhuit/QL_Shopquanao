using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Form_QL_ShopQuanAo
{
    public partial class Dangky : Form
    {
        public Dangky()
        {
            InitializeComponent();

            // Ẩn nút đăng nhập lúc đầu
            btnDangNhap.Visible = false;

            // KHÔNG cho nhập vào textbox username, chỉ hiển thị
            txtUsernam.ReadOnly = true;
            txtUsernam.Text = "Hệ thống sẽ tự động tạo username";
            txtUsernam.ForeColor = System.Drawing.Color.Gray;
        }

        string connStr = "Server=DESKTOP-DJ1KDIM\\SQLEXPRESS;Database=QLShopQuanAo;User Id=sa;Password=123;";

        private void Dangky_Load(object sender, EventArgs e)
        {
            // Focus vào textbox password khi form load
            txtPassword.Focus();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string password = txtPassword.Text.Trim();

            // Validate chỉ password
            if (!ValidateInput(password))
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // GỌI STORED PROCEDURE TẠO TÀI KHOẢN TỰ ĐỘNG
                    SqlCommand cmd = new SqlCommand("sp_TaoTaiKhoanTuDong", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Tham số đầu vào - CHỈ CẦN PASSWORD
                    cmd.Parameters.AddWithValue("@MatKhau", password);

                    // Tham số đầu ra
                    SqlParameter outputParam = new SqlParameter("@TenDangNhapOUT", SqlDbType.NVarChar, 50);
                    outputParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputParam);

                    cmd.ExecuteNonQuery();

                    // Lấy tên đăng nhập được tạo tự động
                    string generatedUsername = outputParam.Value?.ToString();

                    if (string.IsNullOrEmpty(generatedUsername))
                    {
                        MessageBox.Show("❌ Không thể tạo tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // HIỆN THÔNG BÁO THÀNH CÔNG
                    MessageBox.Show($"✅ Đăng ký thành công!\nTên đăng nhập: {generatedUsername}\nVai trò: Nhân viên",
                                  "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ẨN NÚT ĐĂNG KÝ, HIỆN NÚT ĐĂNG NHẬP
                    btnDangKy.Visible = false;
                    btnDangNhap.Visible = true;

                    // Hiển thị thông tin tài khoản đã tạo
                    txtUsernam.Text = generatedUsername;
                    txtUsernam.ForeColor = System.Drawing.Color.Black; // Đổi màu chữ
                    txtPassword.Text = ""; // Xóa password
                    txtPassword.Enabled = false; // Không cho nhập lại password
                }
            }
            catch (SqlException ex)
            {
                HandleSqlError(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput(string password)
        {
            // Chỉ kiểm tra password
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("⚠️ Vui lòng nhập Mật khẩu!", "Thông báo");
                txtPassword.Focus();
                return false;
            }

            if (password.Length < 3)
            {
                MessageBox.Show("⚠️ Mật khẩu phải có ít nhất 3 ký tự!", "Thông báo");
                txtPassword.Focus();
                return false;
            }

            return true;
        }

        private void HandleSqlError(SqlException ex)
        {
            string errorMessage = ex.Message.ToLower();

            if (ex.Number == 15007 || errorMessage.Contains("tên đăng nhập đã tồn tại"))
            {
                MessageBox.Show("❌ Tên đăng nhập đã tồn tại! Vui lòng thử lại.", "Lỗi đăng ký", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (errorMessage.Contains("password") || errorMessage.Contains("mật khẩu"))
            {
                MessageBox.Show("❌ Mật khẩu không đủ mạnh hoặc không hợp lệ!", "Lỗi đăng ký", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (errorMessage.Contains("permission") || errorMessage.Contains("quyền"))
            {
                MessageBox.Show("❌ Không có quyền thực hiện thao tác này!", "Lỗi đăng ký", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show($"❌ Lỗi đăng ký: {ex.Message}", "Lỗi đăng ký", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            // Mở form đăng nhập
            dangnhap f = new dangnhap();
            f.Show();
            this.Hide();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnDangKy_Click(sender, e);
            }
        }

        // Các sự kiện không cần xử lý
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
    }
}